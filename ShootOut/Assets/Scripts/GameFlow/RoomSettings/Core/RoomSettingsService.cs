using UnityEngine;

//Mutable authoritative settings during Lobby; no full snapshot on each edit.
public sealed class RoomSettingsService : System.IDisposable, IRoomSettingsReceiver
{
    private readonly RoomSettingsStore store;
    private readonly IRoomSettingsTransport transport;

    private long revision;
    private long hostUserID;
    private ERoomPhase phase;

    private bool initialized;
    private bool waitingForSnapshot;
    private bool disposed;

    private RoomSettingsData finalizedData;
    private bool hasFinalizedData;

    public event System.Action<ERoomSettingsID> SettingsChanged;
    public event System.Action SettingsReplaced;
    public event System.Action RoomStateChanged;
    public event System.Action<ERoomSettingsResult> RequestRejected;
    public event System.Action<RoomSettingsData> GameStarted;

    public RoomSettingsService(IRoomSettingsTransport transport, long initialHostUserID)
    {
        this.transport = transport ?? throw new System.ArgumentNullException(nameof(transport));
        store = new RoomSettingsStore();
        initialized = transport.IsAuthority();
        revision = initialized ? 0L : -1L;
        hostUserID = initialHostUserID;
        phase = ERoomPhase.Waiting;
        transport.Bind(this);
    }

    public long GetRevision()
    {
        return revision;
    }

    public long GetHostUserID()
    {
        return hostUserID;
    }

    public ERoomPhase GetPhase()
    {
        return phase;
    }

    public bool HasSnapshot()
    {
        return initialized;
    }

    public T Get<T>(ERoomSettingsID id) where T : struct
    {
        return store.Get<T>(id);
    }

    public bool TryGet<T>(ERoomSettingsID id, out T value) where T : struct
    {
        return store.TryGet(id, out value);
    }

    public bool CanLocalEdit()
    {
        return initialized && !disposed &&
            RoomSettingsAuthority.CheckEdit(phase, hostUserID, transport.GetLocalUserID()).IsSuccess();
    }

    //변경 요청 (모든 요청이 받아들여지는 것은 아님)
    public bool RequestChange<T>(ERoomSettingsID id, T value) where T : struct
    {
        if (!CanLocalEdit())
        {
            return false;
        }

        transport.SendChangeRequest(new RoomSettingsChange<T>(id, value));
        return true;
    }

    //Sender ID must be supplied by the authenticated networking connection.
    public ERoomSettingsResult TryApplyOnAuthority<T>(
        long authenticatedRequesterID,
        RoomSettingsChange<T> change)
        where T : struct
    {
        if (!transport.IsAuthority() || disposed)
        {
            return ERoomSettingsResult.NoAuthority;
        }

        ERoomSettingsResult permission = RoomSettingsAuthority.CheckEdit(
            phase, hostUserID, authenticatedRequesterID);
        if (!permission.IsSuccess())
        {
            return permission;
        }

        if (!store.TrySet(change, out bool changed))
        {
            return ERoomSettingsResult.InvalidValue;
        }

        if (changed)
        {
            revision = checked(revision + 1L);

            //Send before notifying user code, which may issue another edit reentrantly.
            transport.PublishDelta(new RoomSettingsDelta<T>(revision, change));

            SettingsChanged?.Invoke(change.id);
            return ERoomSettingsResult.Success;
        }
        return ERoomSettingsResult.UnchagedValue;
    }

    public ERoomSettingsResult TryStartGameOnAuthority(long authenticatedRequesterID)
    {
        if (!transport.IsAuthority() || disposed)
        {
            return ERoomSettingsResult.NoAuthority;
        }

        ERoomSettingsResult permission = RoomSettingsAuthority.CheckEdit(
            phase, hostUserID, authenticatedRequesterID);
        if (!permission.IsSuccess())
        {
            return permission;
        }

        //The host game-room layer must verify team occupancy and readiness first.
        finalizedData = store.CreateData();
        hasFinalizedData = true;
        phase = ERoomPhase.Playing;
        revision = checked(revision + 1L);
        RoomSettingsSnapshot snapshot = new RoomSettingsSnapshot(
            finalizedData, CreateState());
        transport.PublishSnapshot(snapshot);

        RoomStateChanged?.Invoke();
        GameStarted?.Invoke(finalizedData);
        return ERoomSettingsResult.Success;
    }

    public bool TryFinishGameOnAuthority()
    {
        if (!transport.IsAuthority() || disposed || phase != ERoomPhase.Playing)
        {
            return false;
        }

        phase = ERoomPhase.Finished;
        revision = checked(revision + 1L);
        transport.PublishState(CreateState());

        RoomStateChanged?.Invoke();
        return true;
    }

    //Trusted room-management code only; do not expose as a client RPC.
    public bool TryTransferHostOnAuthority(long newHostUserID)
    {
        if (!transport.IsAuthority() || disposed)
        {
            return false;
        }
        if (hostUserID == newHostUserID)
        {
            return true;
        }
        hostUserID = newHostUserID;
        revision = checked(revision + 1L);
        transport.PublishState(CreateState());

        RoomStateChanged?.Invoke();
        return true;
    }

    //Construct a snapshot only on demand, not once per modified setting.
    public RoomSettingsSnapshot CreateSnapshot()
    {
        if (!initialized)
        {
            throw new System.InvalidOperationException("No authoritative snapshot received yet.");
        }

        RoomSettingsData data = hasFinalizedData ? finalizedData : store.CreateData();
        return new RoomSettingsSnapshot(data, CreateState());
    }

    public bool TryGetFinalizedData(out RoomSettingsData data)
    {
        data = finalizedData;
        return hasFinalizedData;
    }

    public bool SendCurrentSnapshotTo(long targetUserID)
    {
        if (!transport.IsAuthority() || disposed)
        {
            return false;
        }

        transport.SendSnapshotTo(targetUserID, CreateSnapshot());
        return true;
    }

    public void ReceiveDelta<T>(RoomSettingsDelta<T> delta) where T : struct
    {
        if (transport.IsAuthority() || disposed || delta.revision <= revision)
        {
            return;
        }
        if (!initialized || waitingForSnapshot || delta.revision != revision + 1L ||
            !store.TrySet(delta.change, out bool changed))
        {
            RequireResync();
            return;
        }

        revision = delta.revision;
        if (changed)
        {
            SettingsChanged?.Invoke(delta.change.id);
        }
    }

    public void ReceiveState(RoomSettingsRoomState state)
    {
        if (transport.IsAuthority() || disposed || state.revision <= revision)
        {
            return;
        }
        if (!initialized || waitingForSnapshot || state.revision != revision + 1L ||
            !IsValidPhase(state.phase))
        {
            RequireResync();
            return;
        }

        revision = state.revision;
        hostUserID = state.hostUserID;
        phase = state.phase;
        RoomStateChanged?.Invoke();
    }

    public void ReceiveSnapshot(RoomSettingsSnapshot snapshot)
    {
        if (transport.IsAuthority() || disposed)
        {
            return;
        }
        if (snapshot.state.revision < 0 ||
            !IsValidPhase(snapshot.state.phase))
        {
            Debug.Log("Invalid");
            return;
        }
        if (initialized && snapshot.state.revision <= revision)
        {
            Debug.Log("Old data");
            return;
        }
        if (!store.TryLoadData(snapshot.data))
        {
            RequireResync();
            return;
        }

        ERoomPhase oldPhase = phase;
        revision = snapshot.state.revision;
        hostUserID = snapshot.state.hostUserID;
        phase = snapshot.state.phase;
        initialized = true;
        waitingForSnapshot = false;
        hasFinalizedData = phase != ERoomPhase.Waiting;
        finalizedData = hasFinalizedData ? snapshot.data : default;

        SettingsReplaced?.Invoke();
        RoomStateChanged?.Invoke();
        if (oldPhase == ERoomPhase.Waiting && phase == ERoomPhase.Playing)
        {
            GameStarted?.Invoke(finalizedData);
        }
    }

    public void ReceiveRejection(ERoomSettingsResult result)
    {
        if (!disposed)
        {
            RequestRejected?.Invoke(result);
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        transport.Unbind(this);
    }

    private static bool IsValidPhase(ERoomPhase value)
    {
        return value == ERoomPhase.Waiting ||
            value == ERoomPhase.Playing || value == ERoomPhase.Finished;
    }

    private RoomSettingsRoomState CreateState()
    {
        return new RoomSettingsRoomState(revision, hostUserID, phase);
    }

    private void RequireResync()
    {
        if (waitingForSnapshot)
        {
            return;
        }

        waitingForSnapshot = true;
        transport.RequestSnapshot();
    }
}
