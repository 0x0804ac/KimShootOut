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

        matchText.text = $"매치 {scoreboard.GetCurrentMatchNumber()} / {scoreboard.GetTotalMatches()}";

        if (scoreboard.GetCurrentMatch() != null)
        {
            roundText.text = $"라운드 {scoreboard.GetCurrentMatch().GetCurrentRoundNumber()} / {scoreboard.GetCurrentMatch().GetTotalRounds()}";
            if (scoreboard.GetCurrentMatch().IsExtraRound())
            {
                roundText.text += " (연장)";
            }
        }
        else
        {
            roundText.text = "-";
        }

        bool canHomeShoot = scoreboard != null && scoreboard.GetCurrentMatch() != null && !scoreboard.GetCurrentMatch().GetRoundList()[^1].GetHome().IsCompleted();
        bool canAwayShoot = scoreboard != null && scoreboard.GetCurrentMatch() != null && !scoreboard.GetCurrentMatch().GetRoundList()[^1].GetAway().IsCompleted();
        homeGoalButton.interactable = canHomeShoot;
        homeMissButton.interactable = canHomeShoot;
        awayGoalButton.interactable = canAwayShoot;
        awayMissButton.interactable = canAwayShoot;
        nextRoundButton.interactable = scoreboard != null && scoreboard.GetCurrentMatch() != null && !scoreboard.GetCurrentMatch().IsCompleted() && scoreboard.GetCurrentMatch().GetRoundList()[^1].IsCompleted();
        nextMatchButton.interactable = scoreboard != null && scoreboard.GetCurrentMatch() != null && scoreboard.GetCurrentMatch().IsCompleted();
        //completeMatchButton.interactable = scoreboard.CurrentMatch.Rounds[scoreboard.TotalMatches / 2].IsCompleted;
    }

    private void OnHomeGoalButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.RecordScore(ETeam.Home, EShotResult.Goal));
    }

    private void OnAwayGoalButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.RecordScore(ETeam.Away, EShotResult.Goal));
    }

    private void OnHomeMissButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.RecordScore(ETeam.Home, EShotResult.Saved));
    }

    private void OnAwayMissButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.RecordScore(ETeam.Away, EShotResult.Saved));
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
        int home = scoreboard.GetCurrentMatch().GetHomeScore();
        int away = scoreboard.GetCurrentMatch().GetAwayScore();
        if (home > away)
        {
            scoreboard.CompleteCurrentMatch(EMatchResult.HomeWin);
        }
        else if (home < away)
        {
            scoreboard.CompleteCurrentMatch(EMatchResult.AwayWin);
        }
        else scoreboard.CompleteCurrentMatch(EMatchResult.Draw);
    }

    private void OnGameResultButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }
        print(scoreboard.GetResult());
    }
    
    //점수판 이벤트 추가
    private void AddEvent()
    {
        if (scoreboard == null)
        {
            return;
        }
        scoreboard.ScoreboardChanged += Refresh;
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
        scoreboard.ScoreboardChanged -= Refresh;
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