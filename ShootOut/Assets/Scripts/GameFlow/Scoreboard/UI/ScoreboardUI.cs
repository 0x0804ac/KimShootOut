using TMPro;
using UnityEngine;
using UnityEngine.UI;

//점수판 UI (예시)
public sealed class ScoreboardUIExample : MonoBehaviour
{
    //홈 팀 점수
    [SerializeField]
    private TMP_Text homeScoreText;
    //원정 팀 점수
    [SerializeField]
    private TMP_Text awayScoreText;
    //매치 정보
    [SerializeField]
    private TMP_Text matchText;
    //라운드 정보
    [SerializeField]
    private TMP_Text roundText;
    //홈 팀 득점 성공
    [SerializeField]
    private Button homeGoalButton;
    //원정 팀 득점 성공
    [SerializeField]
    private Button awayGoalButton;
    //홈 팀 득점 실패
    [SerializeField]
    private Button homeMissButton;
    //원정 팀 득점 실패
    [SerializeField]
    private Button awayMissButton;
    //다음 라운드 시작
    [SerializeField]
    private Button nextRoundButton;
    //다음 매치 시작
    [SerializeField]
    private Button nextMatchButton;
    //매치 완료
    [SerializeField]
    private Button completeMatchButton;
    //경기 결과 출력
    [SerializeField]
    private Button gameResultButton;

    private Scoreboard scoreboard;

    //점수판 초기화
    public void Initialize(Scoreboard scoreboard)
    {
        //기존 점수판 이벤트 제거
        RemoveEvent();

        this.scoreboard = scoreboard;

        //새 점수판 이벤트 추가
        AddEvent();

        Refresh(this.scoreboard);
    }

    private void Start()
    {
        Initialize(new Scoreboard(3));
        scoreboard.StartNextMatch(5);
        scoreboard.StartNextRound();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    //점수판 새로고침
    private void Refresh(Scoreboard scoreboard)
    {
        if (scoreboard == null)
        {
            return;
        }

        (int home, int away) = scoreboard.GetCurrentScores();

        homeScoreText.text = $"{home}";
        awayScoreText.text = $"{away}";

        matchText.text = $"매치 {scoreboard.CurrentMatchNumber} / {scoreboard.TotalMatches}";

        if (scoreboard.CurrentMatch != null)
        {
            roundText.text = $"라운드 {scoreboard.CurrentMatch.CurrentRoundNumber} / {scoreboard.CurrentMatch.TotalRounds}";
            if (scoreboard.CurrentMatch.IsExtraRound)
            {
                roundText.text += " (연장)";
            }
        }
        else
        {
            roundText.text = "-";
        }

        bool canHomeShoot = scoreboard != null && scoreboard.CurrentMatch != null && !scoreboard.CurrentMatch.Rounds[^1].Home.IsCompleted();
        bool canAwayShoot = scoreboard != null && scoreboard.CurrentMatch != null && !scoreboard.CurrentMatch.Rounds[^1].Away.IsCompleted();
        homeGoalButton.interactable = canHomeShoot;
        homeMissButton.interactable = canHomeShoot;
        awayGoalButton.interactable = canAwayShoot;
        awayMissButton.interactable = canAwayShoot;
        nextRoundButton.interactable = scoreboard != null && scoreboard.CurrentMatch != null && !scoreboard.CurrentMatch.IsCompleted && scoreboard.CurrentMatch.Rounds[^1].IsCompleted;
        nextMatchButton.interactable = scoreboard != null && scoreboard.CurrentMatch != null && scoreboard.CurrentMatch.IsCompleted;
        //completeMatchButton.interactable = scoreboard.CurrentMatch.Rounds[scoreboard.TotalMatches / 2].IsCompleted;
    }

    private void OnHomeGoalButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.RecordScore(TeamSide.Home, ShotResult.Goal));
    }

    private void OnAwayGoalButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.RecordScore(TeamSide.Away, ShotResult.Goal));
    }

    private void OnHomeMissButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.RecordScore(TeamSide.Home, ShotResult.Saved));
    }

    private void OnAwayMissButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.RecordScore(TeamSide.Away, ShotResult.Saved));
    }

    private void OnNextRoundButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.StartNextRound());
    }

    private void OnNextMatchButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        scoreboard.StartNextMatch(5);
    }

    private void OnCompleteMatchButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        int home = scoreboard.CurrentMatch.HomeScore;
        int away = scoreboard.CurrentMatch.AwayScore;
        if (home > away)
        {
            scoreboard.CompleteCurrentMatch(MatchResult.HomeWin);
        }
        else if (home < away)
        {
            scoreboard.CompleteCurrentMatch(MatchResult.AwayWin);
        }
        else scoreboard.CompleteCurrentMatch(MatchResult.Draw);
    }

    private void OnGameResultButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.Result);
    }
    
    //점수판 이벤트 추가
    private void AddEvent()
    {
        if (scoreboard == null)
        {
            return;
        }
        scoreboard.OnScoreboardChange += Refresh;
        homeGoalButton.onClick.AddListener(OnHomeGoalButtonClick);
        awayGoalButton.onClick.AddListener(OnAwayGoalButtonClick);
        homeMissButton.onClick.AddListener(OnHomeMissButtonClick);
        awayMissButton.onClick.AddListener(OnAwayMissButtonClick);
        nextRoundButton.onClick.AddListener(OnNextRoundButtonClick);
        nextMatchButton.onClick.AddListener(OnNextMatchButtonClick);
        completeMatchButton.onClick.AddListener(OnCompleteMatchButtonClick);
        gameResultButton.onClick.AddListener(OnGameResultButtonClick);
    }

    //기존 점수판 이벤트 제거
    private void RemoveEvent()
    {
        if (scoreboard == null)
        {
            return;
        }
        scoreboard.OnScoreboardChange -= Refresh;
        homeGoalButton.onClick.RemoveListener(OnHomeGoalButtonClick);
        awayGoalButton.onClick.RemoveListener(OnAwayGoalButtonClick);
        homeMissButton.onClick.RemoveListener(OnHomeMissButtonClick);
        awayMissButton.onClick.RemoveListener(OnAwayMissButtonClick);
        nextRoundButton.onClick.RemoveListener(OnNextRoundButtonClick);
        nextMatchButton.onClick.RemoveListener(OnNextMatchButtonClick);
        completeMatchButton.onClick.RemoveListener(OnCompleteMatchButtonClick);
        gameResultButton.onClick.RemoveListener(OnGameResultButtonClick);
    }
}