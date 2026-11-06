using UnityEngine;
//테스트용 클래스 (로컬 서버, 단일 프로세스 동기 시뮬레이션) 온라인 네트워크 아님
public sealed class LocalRoomSettingsTransport : IRoomSettingsTransport
{
    private readonly long localUserID;
    private IRoomSettingsReceiver receiver;
    private RoomSettingsService authorityService;

    public LocalRoomSettingsTransport(long localUserID)
    {
        this.localUserID = localUserID;
    }

    public long GetLocalUserID()
    {
        return localUserID;
    }

    public bool IsAuthority()
    {
        return true; //테스트용 로컬 서버이기 때문에 별도의 서버 확인 로직이 없으니 주의
    }

    public void Bind(IRoomSettingsReceiver receiver)
    {
        this.receiver = receiver;
        authorityService = receiver as RoomSettingsService;
    }

    public void Unbind(IRoomSettingsReceiver receiver)
    {
        if (ReferenceEquals(this.receiver, receiver))
        {
            this.receiver = null;
            authorityService = null;
        }
    }

    public void SendChangeRequest<T>(RoomSettingsChange<T> change) where T : struct
    {
        if (authorityService == null)
        {
            throw new System.InvalidOperationException("서비스가 아직 bind 되지 않았습니다.");
        }

        ERoomSettingsResult result = authorityService.TryApplyOnAuthority(localUserID, change);
        if (!result.IsSuccess())
        {
            receiver?.ReceiveRejection(result);
        }
        else
        {
            Debug.Log($"방 설정 항목 {change.id}의 값이 {change.value}으로 변경되었습니다.");
        }
    }

    public void PublishDelta<T>(RoomSettingsDelta<T> delta) where T : struct
    {
        //Authority already applied the change; the loopback is ignored.
        receiver?.ReceiveDelta(delta);
    }

    public void PublishState(RoomSettingsRoomState state)
    {
        receiver?.ReceiveState(state);
    }

    public void PublishSnapshot(RoomSettingsSnapshot snapshot)
    {
        receiver?.ReceiveSnapshot(snapshot);
    }

    public void SendSnapshotTo(long targetUserID, RoomSettingsSnapshot snapshot)
    {
        if (targetUserID == localUserID)
        {
            receiver?.ReceiveSnapshot(snapshot);
        }
    }

    public void RequestSnapshot()
    {
        if (authorityService != null)
        {
            receiver?.ReceiveSnapshot(authorityService.CreateSnapshot());
        }
    }
}
