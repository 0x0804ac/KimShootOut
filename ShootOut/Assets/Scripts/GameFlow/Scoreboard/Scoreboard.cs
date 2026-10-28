using System;
using System.Collections.Generic;
using System.Linq;

//점수판 클래스
public sealed class Scoreboard
{
    //매치 리스트
    private readonly List<MatchScore> matchList = new List<MatchScore>();
    
    //총 매치 수
    public int TotalMatches { get; }
}
/*
    public int CurrentMatchNumber => _matches.Count;

    public IReadOnlyList<MatchScore> Matches => _matches;

    public MatchScore CurrentMatch =>
        _matches.Count > 0 ? _matches[^1] : null;

    public event Action<Scoreboard> OnScoreboardChanged;

    public Scoreboard(int totalMatches)
    {
        if (totalMatches < 1)
            throw new ArgumentOutOfRangeException(
                nameof(totalMatches));

        TotalMatches = totalMatches;
    }

    public int HomeMatchWins =>
        _matches.Count(m => m.Result == MatchResult.HomeWin);

    public int AwayMatchWins =>
        _matches.Count(m => m.Result == MatchResult.AwayWin);

    public GameResult Result
    {
        get
        {
            int majority = TotalMatches / 2 + 1;

            if (HomeMatchWins >= majority)
                return GameResult.HomeWin;

            if (AwayMatchWins >= majority)
                return GameResult.AwayWin;

            if (_matches.Count == TotalMatches &&
                _matches.All(m => m.IsCompleted))
            {
                return GameResult.Draw;
            }

            return GameResult.Ongoing;
        }
    }

    // 첫 매치 시작 및 다음 매치 시작
    public bool StartNextMatch(int totalRounds)
    {
        if (Result != GameResult.Ongoing)
            return false;

        if (_matches.Count >= TotalMatches)
            return false;

        if (CurrentMatch != null &&
            !CurrentMatch.IsCompleted)
        {
            return false;
        }

        _matches.Add(new MatchScore(totalRounds));
        NotifyChanged();

        return true;
    }

    // 득점 기록
    public bool RecordScore(TeamSide team, bool scored)
    {
        if (Result != GameResult.Ongoing ||
            CurrentMatch == null)
        {
            return false;
        }

        if (!CurrentMatch.RecordScore(team, scored))
            return false;

        NotifyChanged();
        return true;
    }

    // 다음 라운드로 진행
    public bool AdvanceRound()
    {
        if (Result != GameResult.Ongoing ||
            CurrentMatch == null)
        {
            return false;
        }

        if (!CurrentMatch.AdvanceRound())
            return false;

        NotifyChanged();
        return true;
    }

    // 매치 결과 확정
    public bool CompleteCurrentMatch(MatchResult result)
    {
        if (Result != GameResult.Ongoing ||
            CurrentMatch == null)
        {
            return false;
        }

        if (!CurrentMatch.Complete(result))
            return false;

        NotifyChanged();
        return true;
    }

    // 현재 매치의 홈/원정 점수 반환
    public (int Home, int Away) GetCurrentScores()
    {
        if (CurrentMatch == null)
            return (0, 0);

        return CurrentMatch.GetScores();
    }

    private void NotifyChanged()
    {
        OnScoreboardChanged?.Invoke(this);
    }
}
 */