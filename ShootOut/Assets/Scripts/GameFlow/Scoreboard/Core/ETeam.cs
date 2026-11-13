//팀 구분 (게임플레이 차이 없음)
public enum ETeam
{
    //홈
    Home = 0,
    //원정
    Away = 1
}

//ETeam 확장 클래스
public static class TeamExtensions
{
    public static bool IsValid(this ETeam team)
    {
        return team == ETeam.Home || team == ETeam.Away;
    }
}
