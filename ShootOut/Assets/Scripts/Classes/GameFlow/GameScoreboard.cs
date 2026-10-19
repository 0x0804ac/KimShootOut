using System.Collections.Generic;

//현재 진행 중인 게임의 점수를 관리하는 클래스
public class GameScoreboard
{
    /*
    매치: 한 게임은 1개 이상의 매치로 구성된다. 과반의 매치를 먼저 승리로 가져간 팀이 최종적으로 게임에서 승리한다.
    
    라운드: 한 매치 당 주어지는 공격/수비 횟수를 라운드라고 한다. 더 많은 득점을 기록한 팀이 해당 매치에서 승리한다.

    홈/원정: 전적 상 기록을 위한 이름일 뿐, 게임 플레이 시 실질적인 차이는 없다.
     */

    private int currentMatch; //현재 매치
    private int maxMatch; //총 매치 수
    private int currentRound; //현재 라운드
    private int maxRound; //총 라운드 수

    private bool[] bArrCurrentRoundHomeTeamScore; //현재 라운드의 홈 팀 득점 기록
    private bool[] bArrCurrentRoundAwayTeamScore; //현재 라운드의 원정 팀 득점 기록

    //(maxRound, maxMatch, maxShots: RoomSettings 객체에 저장된 값을 받아오도록 수정해야 함)
    public GameScoreboard(int maxRound, int maxMatch, int maxShots)
    {
        this.maxMatch = maxMatch;
        this.maxRound = maxRound;
        currentMatch = 0;
        currentRound = 0;
        bArrCurrentRoundHomeTeamScore = new bool[maxShots];
        bArrCurrentRoundAwayTeamScore = new bool[maxShots];
    }

    //이번 매치의 득점 기록을 반환하는 함수
    public Dictionary<int, bool[]> GetCurrentRoundScores()
    {
        Dictionary<int, bool[]> scores = new Dictionary<int, bool[]>();
        scores.Add(0, bArrCurrentRoundHomeTeamScore);
        scores.Add(1, bArrCurrentRoundAwayTeamScore);
        return scores;
    }

    //득점을 기록하는 함수 (team도 다른 곳에서 받아올 수 있지 않을까?)
    public void RegisterGoal(Team team)
    {
        switch(team)
        {
            case Team.HOME:
                bArrCurrentRoundHomeTeamScore[currentRound] = true;
                break;
            case Team.AWAY:
                bArrCurrentRoundAwayTeamScore[currentRound] = true;
                break;
        }
    }

    //득점 기록을 초기화하는 함수 (다음 매치로 넘어갈 때 호출)
    public void ClearScores()
    {
        for(int i = 0; i < bArrCurrentRoundHomeTeamScore.Length; i++)
        {
            bArrCurrentRoundHomeTeamScore[i] = false;
            bArrCurrentRoundAwayTeamScore[i] = false;
        }
    }
}
