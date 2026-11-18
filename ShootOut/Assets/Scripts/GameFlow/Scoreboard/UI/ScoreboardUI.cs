using TMPro;
using UnityEngine;
using UnityEngine.UI;

//점수판 테스트용 임시 클래스
public sealed class ScoreboardUIExample : MonoBehaviour
{
    [SerializeField]
    private TMP_Text homeScoreText;
    [SerializeField]
    private TMP_Text awayScoreText;
    [SerializeField]
    private TMP_Text matchText;
    [SerializeField]
    private TMP_Text roundText;
    [SerializeField]
    private Button homeGoalButton;
    [SerializeField]
    private Button awayGoalButton;
    [SerializeField]
    private Button homeMissButton;
    [SerializeField]
    private Button awayMissButton;
    [SerializeField]
    private Button nextRoundButton;
    [SerializeField]
    private Button nextMatchButton;
    [SerializeField]
    private Button completeMatchButton;
    [SerializeField]
    private Button gameResultButton;

    private Scoreboard scoreboard;

    public void Initialize(Scoreboard scoreboard)
    {
        RemoveEvent();

        this.scoreboard = scoreboard;

        AddEvent();

        Refresh(this.scoreboard);
    }

    private void Start()
    {
        Initialize(new Scoreboard(3, 5, false));
        scoreboard.StartNextMatch();
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

        MatchScore currentMatch = scoreboard.GetCurrentMatch();

        if (currentMatch != null)
        {
            roundText.text = $"라운드 {currentMatch.GetCurrentRoundNumber()} / {currentMatch.GetTotalRounds()}";
            if (currentMatch.IsExtraRound())
            {
                roundText.text += " (연장)";
            }
        }
        else
        {
            roundText.text = "-";
        }

        bool canHomeShoot = scoreboard != null &&
            currentMatch != null &&
            !currentMatch.GetRoundList()[^1].Get(ETeam.Home).IsCompleted();
        bool canAwayShoot = scoreboard != null &&
            currentMatch != null &&
            !currentMatch.GetRoundList()[^1].Get(ETeam.Away).IsCompleted();
        homeGoalButton.interactable = canHomeShoot;
        homeMissButton.interactable = canHomeShoot;
        awayGoalButton.interactable = canAwayShoot;
        awayMissButton.interactable = canAwayShoot;
        nextRoundButton.interactable = scoreboard != null &&
            currentMatch != null &&
            !currentMatch.IsCompleted() &&
            currentMatch.GetRoundList()[^1].IsCompleted();
        nextMatchButton.interactable = scoreboard != null &&
            currentMatch != null &&
            currentMatch.IsCompleted();
        completeMatchButton.interactable = scoreboard.GetResult().IsCompleted();
    }

    private void OnHomeGoalButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }

        if (!scoreboard.RecordScore(ETeam.Home, EShotResult.Goal))
        {
            print("득점 기록 실패");
        }
    }

    private void OnAwayGoalButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }

        if (!scoreboard.RecordScore(ETeam.Away, EShotResult.Goal))
        {
            print("득점 기록 실패");
        }
    }

    private void OnHomeMissButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }

        if (!scoreboard.RecordScore(ETeam.Home, EShotResult.Saved))
        {
            print("선방 기록 실패");
        }
    }

    private void OnAwayMissButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }

        if (!scoreboard.RecordScore(ETeam.Away, EShotResult.Saved))
        {
            print("선방 기록 실패");
        }
    }

    private void OnNextRoundButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }

        PrintRoundResult(scoreboard.GetCurrentRound());

        if (!scoreboard.StartNextRound())
        {
            print("다음 라운드를 시작하지 못했습니다.");
        }
    }

    private void OnNextMatchButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }

        MatchScore match = scoreboard.GetCurrentMatch();
        print($"매치 {scoreboard.GetCurrentMatchNumber()} 결과 {match.GetHomeScore()} : {match.GetAwayScore()}");

        if (!scoreboard.StartNextMatch())
        {
            print("다음 매치를 시작하지 못했습니다.");
        }
    }

    private void OnCompleteMatchButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }

        print($"게임 결과 {scoreboard.GetHomeWins()} : {scoreboard.GetAwayWins()} - {scoreboard.GetResult()}");

        Debug.Break();
    }

    private void OnGameResultButtonClick()
    {
        if (scoreboard == null)
        {
            return;
        }

        print(scoreboard.GetResult());
    }
    
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

    private void PrintRoundResult(RoundScore round)
    {
        if (round == null)
        {
            return;
        }

        print($"라운드 {round.GetRoundNumber()} 결과 {Convert(round.Get(ETeam.Home))} : {Convert(round.Get(ETeam.Away))}");
    }

    private string Convert(EShotResult value)
    {
        return value == EShotResult.Goal ? "O" : "X";
    }
}