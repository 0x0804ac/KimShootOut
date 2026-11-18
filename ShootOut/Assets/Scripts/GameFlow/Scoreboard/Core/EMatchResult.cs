public enum EMatchResult
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

//EMatchResult 확장 클래스
public static class MatchResultExtensions
{
    public static bool IsCompleted(this EMatchResult result)
    {
        return result != EMatchResult.Ongoing;
    }

    public static bool IsWinFor(this EMatchResult result, ETeam team)
    {
        if (team == ETeam.Home)
        {
            return result == EMatchResult.HomeWin || result == EMatchResult.AwayForfeit;
        }

        if (team == ETeam.Away)
        {
            return result == EMatchResult.AwayWin || result == EMatchResult.HomeForfeit;
        }

        return false;
    }
}