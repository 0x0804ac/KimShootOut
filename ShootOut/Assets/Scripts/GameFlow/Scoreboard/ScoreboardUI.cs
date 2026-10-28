using UnityEngine;
using TMPro;

public sealed class ScoreboardUIExample : MonoBehaviour
{
    [SerializeField] private TMP_Text homeScoreText;
    [SerializeField] private TMP_Text awayScoreText;
    [SerializeField] private TMP_Text matchText;
    [SerializeField] private TMP_Text roundText;

    private Scoreboard scoreboard;

    public void Initialize(Scoreboard scoreboard)
    {
        if (this.scoreboard != null)
            this.scoreboard.OnScoreboardChange -= Refresh;

        this.scoreboard = scoreboard;

        if (this.scoreboard != null)
            this.scoreboard.OnScoreboardChange += Refresh;

        Refresh(this.scoreboard);
    }

    private void OnDestroy()
    {
        if (scoreboard != null)
            scoreboard.OnScoreboardChange -= Refresh;
    }

    private void Refresh(Scoreboard scoreboard)
    {
        if (scoreboard == null)
            return;

        var (home, away) = scoreboard.GetCurrentScores();

        homeScoreText.text = home.ToString();
        awayScoreText.text = away.ToString();

        matchText.text =
            $"매치 {scoreboard.CurrentMatchNumber}" +
            $" / {scoreboard.TotalMatches}";

        if (scoreboard.CurrentMatch != null)
        {
            roundText.text =
                $"라운드 {scoreboard.CurrentMatch.CurrentRoundNumber}" +
                $" / {scoreboard.CurrentMatch.TotalRounds}";

            if (scoreboard.CurrentMatch.IsExtraRound)
                roundText.text += " (연장)";
        }
        else
        {
            roundText.text = "-";
        }
    }
}