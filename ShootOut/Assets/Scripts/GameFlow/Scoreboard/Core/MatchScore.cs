using System;

[Serializable]
public sealed class MatchScore
{
    private readonly int totalRounds;
    private readonly bool forceFullRounds;
    private readonly System.Collections.Generic.List<RoundScore> roundList;
    private readonly System.Collections.ObjectModel.ReadOnlyCollection<RoundScore> readOnlyRoundList;

    private int homeScore;
    private int awayScore;
    private int homeCompletedShots;
    private int awayCompletedShots;
    private EMatchResult result;

    internal MatchScore(int totalRounds, bool forceFullRounds)
    {
        if (totalRounds < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(totalRounds));
        }

        this.totalRounds = totalRounds;
        this.forceFullRounds = forceFullRounds;
        roundList = new System.Collections.Generic.List<RoundScore>(totalRounds + 1);
        readOnlyRoundList = roundList.AsReadOnly();
        result = EMatchResult.Ongoing;
        AddRound();
    }

    public int GetTotalRounds()
    {
        return totalRounds;
    }

    public System.Collections.Generic.IReadOnlyList<RoundScore> GetRoundList()
    {
        return readOnlyRoundList;
    }

    public EMatchResult GetResult()
    {
        return result;
    }

    //첫 라운드 번호 = 1
    public int GetCurrentRoundNumber()
    {
        return roundList.Count;
    }

    public RoundScore GetCurrentRound()
    {
        return roundList[^1];
    }

    public int GetHomeScore()
    {
        return homeScore;
    }

    public int GetAwayScore()
    {
        return awayScore;
    }

    //(홈 팀 점수, 원정 팀 점수)
    public (int home, int away) GetScores()
    {
        return (homeScore, awayScore);
    }

    public bool IsExtraRound()
    {
        return GetCurrentRoundNumber() > totalRounds;
    }

    public bool IsCompleted()
    {
        return result.IsCompleted();
    }

    //현재 라운드에 기록
    internal bool RecordScore(ETeam team, EShotResult next)
    {
        if (IsCompleted())
        {
            return false;
        }

        RoundScore currentRound = GetCurrentRound();
        if (!currentRound.TryRecord(team, next, out bool shotFinished, out bool scored))
        {
            return false;
        }

        if (shotFinished)
        {
            if (team == ETeam.Home)
            {
                homeCompletedShots++;

                if (scored)
                {
                    homeScore++;
                }
            }
            else
            {
                awayCompletedShots++;

                if (scored)
                {
                    awayScore++;
                }
            }

            EvaluateResult();
        }

        return true;
    }

    // Round transitions are explicit, allowing UI/gameplay animation between rounds.
    internal bool StartNextRound()
    {
        if (IsCompleted() || !GetCurrentRound().IsCompleted())
        {
            return false;
        }

        AddRound();
        return true;
    }

    internal MatchScoreSnapshot CreateSnapshot()
    {
        RoundScoreSnapshot[] rounds = new RoundScoreSnapshot[roundList.Count];

        for (int i = 0; i < roundList.Count; i++)
        {
            RoundScore round = roundList[i];
            rounds[i] = new RoundScoreSnapshot(
                round.GetRoundNumber(), round.Get(ETeam.Home), round.Get(ETeam.Away));
        }

        return new MatchScoreSnapshot(totalRounds, homeScore, awayScore, result, rounds);
    }

    internal bool TryForfeit(ETeam forfeitingTeam)
    {
        if (IsCompleted() || !forfeitingTeam.IsValid())
        {
            return false;
        }

        result = forfeitingTeam == ETeam.Home
            ? EMatchResult.HomeForfeit
            : EMatchResult.AwayForfeit;
        return true;
    }

    private void AddRound()
    {
        roundList.Add(new RoundScore(roundList.Count + 1));
    }

    private void EvaluateResult()
    {
        RoundScore currentRound = GetCurrentRound();

        if (IsExtraRound())
        {
            // Sudden death: both teams must complete an equal number of extra attempts.
            if (currentRound.IsCompleted() && homeScore != awayScore)
            {
                SetWinnerByGoals();
            }

            return;
        }

        if (!forceFullRounds)
        {
            // Insurmountable lead can end regulation even during an unfinished round.
            int remainingHomeShots = totalRounds - homeCompletedShots;
            int remainingAwayShots = totalRounds - awayCompletedShots;

            if (homeScore > awayScore + remainingAwayShots ||
                awayScore > homeScore + remainingHomeShots)
            {
                SetWinnerByGoals();
                return;
            }
        }

        if (GetCurrentRoundNumber() == totalRounds && currentRound.IsCompleted() &&
            homeScore != awayScore)
        {
            SetWinnerByGoals();
        }
        // If the final regulation round is tied, StartNextRound opens sudden death.
    }

    private void SetWinnerByGoals()
    {
        result = homeScore > awayScore ? EMatchResult.HomeWin : EMatchResult.AwayWin;
    }
}
