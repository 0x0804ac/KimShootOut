//방 현재 상태
public enum RoomPhase
{
    //게임 시작 전
    PreGame,
    //게임 중
    Playing,
    //게임 종료 후
    PostGame
}

//방 설정 변경 시도 결과
public enum RoomSettingsResult
{
    //성공
    Success,
    //실패(방장이 아님)
    NotHost,
    //실패(게임 시작됨)
    GameAlreadyStarted,
    //실패(올바르지 않은 값)
    InvalidValue,
    //실패(권한 없음)
    NoAuthority
}

//RoomSettingsResult 확장 클래스
public static class RoomSettingsResultExtensions
{
    public static bool IsSuccess(this RoomSettingsResult result)
    {
        return result == RoomSettingsResult.Success;
    }
}

//방 설정 권한
public sealed class RoomSettingsAuthority
{
    //방장 ID
    public long HostID { get; private set; }
    //방 현재 상태
    public RoomPhase Phase { get; private set; }

    //생성자
    public RoomSettingsAuthority(long hostId)
    {
        HostID = hostId;
        Phase = RoomPhase.PreGame;
    }

    //방장의 ID인지 검사
    public bool IsHost(long playerId)
    {
        return playerId == HostID;
    }

    //방 설정을 변경할 수 있는지 검사
    public bool CanEdit(long playerId)
    {
        return Phase == RoomPhase.PreGame && IsHost(playerId);
    }

    //게임 시작 시도
    public bool TryStartGame(long playerId)
    {
        if (!CanEdit(playerId))
        {
            return false;
        }
        Phase = RoomPhase.Playing;
        return true;
    }

    //방장 넘기기
    public void TransferHost(long newHostId)
    {
        HostID = newHostId;
    }
}