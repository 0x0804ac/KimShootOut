public enum EGameResult
{
    //진행 중
    Ongoing = 0,
    //홈 팀 승리
    HomeWin = 1,
    //원정 팀 승리
    AwayWin = 2,
    //무승부
    Draw = 3,
    //홈 팀 기권(포기, 연결 끊김 등)
    HomeForfeit = 4,
    //원정 팀 기권(포기, 연결 끊김 등)
    AwayForfeit = 5
}

//EGameResult 확장 클래스
public static class GameResultExtensions
{
    public static bool IsCompleted(this EGameResult result)
    {
        return result != EGameResult.Ongoing;
    }

    public static bool IsWinFor(this EGameResult result, ETeam team)
    {
        if (team == ETeam.Home)
        {
            return result == EGameResult.HomeWin || result == EGameResult.AwayForfeit;
        }

        if (team == ETeam.Away)
        {
            return result == EGameResult.AwayWin || result == EGameResult.HomeForfeit;
        }

        return false;
    }
}