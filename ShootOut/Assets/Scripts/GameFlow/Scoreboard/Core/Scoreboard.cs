public sealed class Scoreboard
{
    private readonly int totalMatches;
    private readonly int roundsPerMatch;
    private readonly bool forceFullRounds;
    private readonly int matchesToWin;
    private readonly System.Collections.Generic.List<MatchScore> matchList;
    private readonly System.Collections.ObjectModel.ReadOnlyCollection<MatchScore> readOnlyMatchList;

    private int homeWins;
    private int awayWins;
    private EGameResult result;

    public event System.Action<Scoreboard> ScoreboardChanged;

    public Scoreboard(int totalMatches, int roundsPerMatch, bool forceFullRounds)
    {
        if (totalMatches < 1)
        {
            throw new System.ArgumentOutOfRangeException(nameof(totalMatches));
        }

        if (roundsPerMatch < 1)
        {
            throw new System.ArgumentOutOfRangeException(nameof(roundsPerMatch));
        }

        this.totalMatches = totalMatches;
        this.roundsPerMatch = roundsPerMatch;
        this.forceFullRounds = forceFullRounds;
        matchesToWin = totalMatches / 2 + 1;
        matchList = new System.Collections.Generic.List<MatchScore>(totalMatches);
        readOnlyMatchList = matchList.AsReadOnly();
        result = EGameResult.Ongoing;
    }

    public int GetTotalMatches()
    {
        return totalMatches;
    }

    public int GetRoundsPerMatch()
    {
        return roundsPerMatch;
    }

    public bool GetForceFullRounds()
    {
        return forceFullRounds;
    }

    public int GetMatchesToWin()
    {
        return matchesToWin;
    }

    public System.Collections.Generic.IReadOnlyList<MatchScore> GetMatchList()
    {
        return readOnlyMatchList;
    }

    //첫 매치 시작 전 = 0, 첫 매치 번호 = 1
    public int GetCurrentMatchNumber()
    {
        return matchList.Count;
    }

    public MatchScore GetCurrentMatch()
    {
        return matchList.Count == 0 ? null : matchList[matchList.Count - 1];
    }

    public RoundScore GetCurrentRound()
    {
        MatchScore currentMatch = GetCurrentMatch();
        return currentMatch == null ? null : currentMatch.GetCurrentRound();
    }

    public int GetHomeWins()
    {
        return homeWins;
    }

    public int GetAwayWins()
    {
        return awayWins;
    }

    public EGameResult GetResult()
    {
        return result;
    }

    public (int home, int away) GetCurrentScores()
    {
        MatchScore current = GetCurrentMatch();
        return current == null ? (0, 0) : current.GetScores();
    }

    public bool IsCompleted()
    {
        return result.IsCompleted();
    }

    public bool StartNextMatch()
    {
        if (IsCompleted() || matchList.Count >= totalMatches)
        {
            return false;
        }

        MatchScore current = GetCurrentMatch();

        if (current != null && !current.IsCompleted())
        {
            return false;
        }

        matchList.Add(new MatchScore(roundsPerMatch, forceFullRounds));
        NotifyChange();
        return true;
    }

    public bool RecordScore(ETeam team, EShotResult shotResult)
    {
        MatchScore current = GetCurrentMatch();

        if (IsCompleted() || current == null || current.IsCompleted())
        {
            return false;
        }

        if (!current.RecordScore(team, shotResult))
        {
            return false;
        }

        if (current.IsCompleted())
        {
            ApplyCompletedMatch(current);
        }

        NotifyChange();
        return true;
    }

    public bool StartNextRound()
    {
        MatchScore current = GetCurrentMatch();

        if (IsCompleted() || current == null || !current.StartNextRound())
        {
            return false;
        }

        NotifyChange();
        return true;
    }

    //Explicit, allocation-bearing snapshot for save files or late-joining clients.
    public ScoreboardSnapshot CreateSnapshot()
    {
        MatchScoreSnapshot[] matches = new MatchScoreSnapshot[matchList.Count];

        for (int i = 0; i < matchList.Count; i++)
        {
            matches[i] = matchList[i].CreateSnapshot();
        }

        return new ScoreboardSnapshot(
            totalMatches, roundsPerMatch, forceFullRounds,
            homeWins, awayWins, result, matches);
    }

    //A match-level forfeit awards one match win to the other team.
    public bool TryForfeitCurrentMatch(ETeam forfeitingTeam)
    {
        MatchScore current = GetCurrentMatch();

        if (IsCompleted() || current == null || !current.TryForfeit(forfeitingTeam))
        {
            return false;
        }

        ApplyCompletedMatch(current);
        NotifyChange();
        return true;
    }

    //A game-level forfeit ends the entire game immediately.
    //If a match is active, its result records the same forfeit.
    public bool TryForfeitGame(ETeam forfeitingTeam)
    {
        if (IsCompleted() || !forfeitingTeam.IsValid())
        {
            return false;
        }

        MatchScore current = GetCurrentMatch();

        if (current != null && !current.IsCompleted())
        {
            current.TryForfeit(forfeitingTeam);
            ApplyCompletedMatch(current);
        }

        result = forfeitingTeam == ETeam.Home
            ? EGameResult.HomeForfeit
            : EGameResult.AwayForfeit;

        NotifyChange();
        return true;
    }

    private void ApplyCompletedMatch(MatchScore completedMatch)
    {
        EMatchResult matchResult = completedMatch.GetResult();

        if (matchResult.IsWinFor(ETeam.Home))
        {
            homeWins++;
        }
        else if (matchResult.IsWinFor(ETeam.Away))
        {
            awayWins++;
        }

        if (homeWins >= matchesToWin)
        {
            result = EGameResult.HomeWin;
        }
        else if (awayWins >= matchesToWin)
        {
            result = EGameResult.AwayWin;
        }
        else if (matchList.Count >= totalMatches)
        {
            if (homeWins > awayWins)
            {
                result = EGameResult.HomeWin;
            }
            else if (awayWins > homeWins)
            {
                result = EGameResult.AwayWin;
            }
            else
            {
                //Only possible with even match counts or administrative draws.
                result = EGameResult.Draw;
            }
        }
    }

    private void NotifyChange()
    {
        ScoreboardChanged?.Invoke(this);
    }
}
