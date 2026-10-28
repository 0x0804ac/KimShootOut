//게임 결과
public enum GameResult
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

//GameResult 확장 클래스
public static class GameResultExtensions
{
    public static bool IsCompleted(this GameResult result)
    {
        return result != GameResult.Ongoing;
    }
}