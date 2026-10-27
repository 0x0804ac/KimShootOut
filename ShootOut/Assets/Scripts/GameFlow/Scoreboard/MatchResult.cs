//매치 결과
public enum MatchResult
{
    //진행 중
    Ongoing,
    //홈 팀 승리
    HomeWin,
    //원정 팀 승리
    AwayWin,
    //무승부
    Draw,
    //홈 팀 기권(포기, 연결 끊김 등)
    HomeGiveUp,
    //원정 팀 기권(포기, 연결 끊김 등)
    AwayGiveUp
}

//MatchResult 확장 클래스
public static class MatchResultExtensions
{
    //표시 이름 반환
    public static string GetDisplayName(this MatchResult result)
    {
        switch (result)
        {
            case MatchResult.Ongoing:
                return "진행 중";
            case MatchResult.HomeWin:
                return "홈 팀 승리";
            case MatchResult.AwayWin:
                return "원정 팀 승리";
            case MatchResult.Draw:
                return "무승부";
            case MatchResult.HomeGiveUp:
                return "홈 팀 기권";
            case MatchResult.AwayGiveUp:
                return "원정 팀 기권";
            default:
                return result.ToString();
        }
    }

    //완료된 결과인지 확인
    public static bool IsCompleted(this MatchResult result)
    {
        return result != MatchResult.Ongoing;
    }
}