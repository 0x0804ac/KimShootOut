//팀 구분 (게임플레이 차이 없음)
public enum TeamSide
{
    //홈
    Home,
    //원정
    Away
}

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