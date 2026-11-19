//Snapshots are created on demand (late join/save), never per shot update.
public readonly struct RoundScoreSnapshot : System.IEquatable<RoundScoreSnapshot>
{
    public readonly int roundNumber;
    public readonly EShotResult home;
    public readonly EShotResult away;
    private readonly int cachedHashCode;

    internal RoundScoreSnapshot(int roundNumber, EShotResult home, EShotResult away)
    {
        this.roundNumber = roundNumber;
        this.home = home;
        this.away = away;

        unchecked
        {
            int hash = 17;
            hash = hash * 31 + roundNumber;
            hash = hash * 31 + (int)home;
            hash = hash * 31 + (int)away;
            cachedHashCode = hash;
        }
    }

    public bool Equals(RoundScoreSnapshot other)
    {
        return roundNumber == other.roundNumber && home == other.home && away == other.away;
    }

    public override bool Equals(object other)
    {
        return other is RoundScoreSnapshot snapshot && Equals(snapshot);
    }

    public override int GetHashCode()
    {
        return cachedHashCode;
    }
}

public readonly struct MatchScoreSnapshot : System.IEquatable<MatchScoreSnapshot>
{
    private readonly RoundScoreSnapshot[] rounds;
    private readonly int cachedHashCode;

    public readonly int totalRounds;
    public readonly int homeScore;
    public readonly int awayScore;
    public readonly EMatchResult result;

    internal MatchScoreSnapshot(
        int totalRounds,
        int homeScore,
        int awayScore,
        EMatchResult result,
        RoundScoreSnapshot[] ownedRounds)
    {
        if (ownedRounds == null)
        {
            throw new System.ArgumentNullException(nameof(ownedRounds));
        }

        this.totalRounds = totalRounds;
        this.homeScore = homeScore;
        this.awayScore = awayScore;
        this.result = result;
        rounds = ownedRounds;

        unchecked
        {
            int hash = 17;
            hash = hash * 31 + totalRounds;
            hash = hash * 31 + homeScore;
            hash = hash * 31 + awayScore;
            hash = hash * 31 + (int)result;

            for (int i = 0; i < rounds.Length; i++)
            {
                hash = hash * 31 + rounds[i].GetHashCode();
            }

            cachedHashCode = hash;
        }
    }

    public int GetRoundCount()
    {
        return rounds == null ? 0 : rounds.Length;
    }

    public RoundScoreSnapshot GetRound(int index)
    {
        return rounds[index];
    }

    public bool Equals(MatchScoreSnapshot other)
    {
        if (totalRounds != other.totalRounds || homeScore != other.homeScore ||
            awayScore != other.awayScore || result != other.result ||
            GetRoundCount() != other.GetRoundCount())
        {
            return false;
        }

        for (int i = 0; i < GetRoundCount(); i++)
        {
            if (!rounds[i].Equals(other.rounds[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object other)
    {
        return other is MatchScoreSnapshot snapshot && Equals(snapshot);
    }

    public override int GetHashCode()
    {
        return cachedHashCode;
    }
}

public readonly struct ScoreboardSnapshot : System.IEquatable<ScoreboardSnapshot>
{
    private readonly MatchScoreSnapshot[] matches;
    private readonly int cachedHashCode;

    public readonly int totalMatches;
    public readonly int roundsPerMatch;
    public readonly bool forceFullRounds;
    public readonly int homeWins;
    public readonly int awayWins;
    public readonly EGameResult result;

    internal ScoreboardSnapshot(
        int totalMatches,
        int roundsPerMatch,
        bool forceFullRounds,
        int homeWins,
        int awayWins,
        EGameResult result,
        MatchScoreSnapshot[] ownedMatches)
    {
        matches = ownedMatches ?? throw new System.ArgumentNullException(nameof(ownedMatches));
        this.totalMatches = totalMatches;
        this.roundsPerMatch = roundsPerMatch;
        this.forceFullRounds = forceFullRounds;
        this.homeWins = homeWins;
        this.awayWins = awayWins;
        this.result = result;

        unchecked
        {
            int hash = 17;
            hash = hash * 31 + totalMatches;
            hash = hash * 31 + roundsPerMatch;
            hash = hash * 31 + (forceFullRounds ? 1 : 0);
            hash = hash * 31 + homeWins;
            hash = hash * 31 + awayWins;
            hash = hash * 31 + (int)result;

            for (int i = 0; i < matches.Length; i++)
            {
                hash = hash * 31 + matches[i].GetHashCode();
            }

            cachedHashCode = hash;
        }
    }

    public int GetMatchCount()
    {
        return matches == null ? 0 : matches.Length;
    }

    public MatchScoreSnapshot GetMatch(int index)
    {
        return matches[index];
    }

    public bool Equals(ScoreboardSnapshot other)
    {
        if (totalMatches != other.totalMatches || roundsPerMatch != other.roundsPerMatch ||
            forceFullRounds != other.forceFullRounds || homeWins != other.homeWins ||
            awayWins != other.awayWins || result != other.result ||
            GetMatchCount() != other.GetMatchCount())
        {
            return false;
        }

        for (int i = 0; i < GetMatchCount(); i++)
        {
            if (!matches[i].Equals(other.matches[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object other)
    {
        return other is ScoreboardSnapshot snapshot && Equals(snapshot);
    }

    public override int GetHashCode()
    {
        return cachedHashCode;
    }
}
