//Adapter boundary: only the networking SDK can authenticate incoming senders.
//Every incoming receiver callback must be invoked only for authoritative messages.
public interface IRoomSettingsReceiver
{
    void ReceiveDelta<T>(RoomSettingsDelta<T> delta) where T : struct;
    void ReceiveState(RoomSettingsRoomState state);
    void ReceiveSnapshot(RoomSettingsSnapshot snapshot);
    void ReceiveRejection(ERoomSettingsResult result);
}

public interface IRoomSettingsTransport
{
    bool IsAuthority();
    long GetLocalUserID();

    void Bind(IRoomSettingsReceiver receiver);
    void Unbind(IRoomSettingsReceiver receiver);

    void SendChangeRequest<T>(RoomSettingsChange<T> change) where T : struct;
    void PublishDelta<T>(RoomSettingsDelta<T> delta) where T : struct;
    void PublishState(RoomSettingsRoomState state);
    void PublishSnapshot(RoomSettingsSnapshot snapshot);
    void SendSnapshotTo(long targetUserID, RoomSettingsSnapshot snapshot);
    void RequestSnapshot();
}
