using System;

[Serializable]
public sealed class RoundScore
{
    private readonly int roundNumber;
    private EShotResult home;
    private EShotResult away;

    internal RoundScore(int roundNumber)
    {
        if (roundNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(roundNumber));
        }

        this.roundNumber = roundNumber;
        home = EShotResult.Pending;
        away = EShotResult.Pending;
    }

    public int GetRoundNumber()
    {
        return roundNumber;
    }

    public EShotResult Get(ETeam team)
    {
        if (team == ETeam.Home)
        {
            return home;
        }

        if (team == ETeam.Away)
        {
            return away;
        }

        throw new ArgumentOutOfRangeException(nameof(team));
    }

    public bool IsCompleted()
    {
        return home.IsCompleted() && away.IsCompleted();
    }

    internal bool TryRecord(ETeam team, EShotResult result, out bool shotFinished, out bool scored)
    {
        shotFinished = false;
        scored = false;

        if (!team.IsValid())
        {
            return false;
        }

        EShotResult current = Get(team);
        if (!current.CanRecord(result))
        {
            return false;
        }

        if (team == ETeam.Home)
        {
            home = result;
        }
        else
        {
            away = result;
        }

        shotFinished = result.IsCompleted();
        scored = result == EShotResult.Goal;
        return true;
    }
}