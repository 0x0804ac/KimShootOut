using UnityEngine;
using TMPro;

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
    [SerializeField] private TMP_Text roundText;

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

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void Refresh(Scoreboard scoreboard)
    {
        if (scoreboard == null)
        {
            return;
        }

        (int home, int away) = scoreboard.GetCurrentScores();

        homeScoreText.text = $"|{home}|";
        awayScoreText.text = $"|{away}|";

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
    }

    //점수판 이벤트 추가
    private void AddEvent()
    {
        if (scoreboard == null)
        {
            return;
        }
        scoreboard.OnScoreboardChange += Refresh;
    }

    //기존 점수판 이벤트 제거
    private void RemoveEvent()
    {
        if (scoreboard == null)
        {
            return;
        }
        scoreboard.OnScoreboardChange -= Refresh;
    }
}