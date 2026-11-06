//방 현재 상태
public enum ERoomPhase
{
    //게임 시작 전
    Waiting,
    //게임 중
    Playing,
    //게임 종료 후
    Finished
}

//방 설정 변경 시도 결과
public enum ERoomSettingsResult
{
    //성공
    Success,
    //실패(방장이 아님)
    NotHost,
    //실패(설정 잠김)
    Locked,
    //실패(올바르지 않은 값)
    InvalidValue,
    //실패(권한 없음)
    NoAuthority,
    //실패(같은 값)
    UnchagedValue
}

//RoomSettingsResult 확장 클래스
public static class RoomSettingsResultExtensions
{
    public static bool IsSuccess(this ERoomSettingsResult result)
    {
        return result == ERoomSettingsResult.Success;
    }
}

public readonly struct RoomSettingsRoomState
{
    public readonly long revision;
    public readonly long hostUserID;
    public readonly ERoomPhase phase;

    public RoomSettingsRoomState(long revision, long hostUserID, ERoomPhase phase)
    {
        this.revision = revision;
        this.hostUserID = hostUserID;
        this.phase = phase;
    }
}

public readonly struct RoomSettingsSnapshot
{
    public readonly RoomSettingsData data;
    public readonly RoomSettingsRoomState state;

    public RoomSettingsSnapshot(RoomSettingsData data, RoomSettingsRoomState state)
    {
        this.data = data;
        this.state = state;
    }
}
