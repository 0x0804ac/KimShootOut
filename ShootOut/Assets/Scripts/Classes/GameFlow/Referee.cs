//매치의 진행을 담당하는 클래스
public class Referee
{
    private bool isGoal = false; //이번 턴에 찬 공의 골 여부

    //매치를 시작하는 함수
    public void StartMatch()
    {
        //매치 시작 로직 구현
    }

    //다음 라운드로 진행하는 함수
    public void NextRound()
    {
        NextTurn();
        //다음 라운드로 진행하는 로직 구현
    }

    //다음 턴으로 진행(공수교대)하는 함수
    public void NextTurn()
    {
        //GameScoreboard의 RegisterScore()를 호출하여 이번 턴의 득점 여부를 기록
        isGoal = false;
    }

    //이번 턴에 찬 공을 골로 등록하는 함수 (별도의 클래스로 분리할 수도 있음)
    public void RegisterGoal()
    {
        isGoal = true;
        //내용 추가 예정
    }
}
