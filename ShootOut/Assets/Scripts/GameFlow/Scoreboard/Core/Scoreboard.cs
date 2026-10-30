using System;
using System.Collections.Generic;
using System.Linq;

//점수 관리 클래스
public sealed class Scoreboard
{
    //매치 리스트
    private readonly List<MatchScore> matchList = new List<MatchScore>();

    //총 매치 수
    public int TotalMatches { get; }

    //현재 매치 번호 (첫 매치 번호 = 1)
    public int CurrentMatchNumber
    {
        get
        {
            return matchList.Count;
        }
    }

    //매치 목록 (수정 불가)
    public IReadOnlyList<MatchScore> Matches
    {
        get
        {
            return matchList;
        }
    }

    //현재 진행 중인 매치 반환
    public MatchScore CurrentMatch
    {
        get
        {
            return matchList.Count > 0 ? matchList[^1] : null;
        }
    }

    //점수 변동 이벤트
    public event Action<Scoreboard> OnScoreboardChange;

    //총 매치 수를 받아서 점수판을 생성
    public Scoreboard(int totalMatches)
    {
        if (totalMatches < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(totalMatches));
        }
        TotalMatches = totalMatches;
    }

    //홈 팀 매치 승수
    public int HomeMatchWins
    {
        get
        {
            return matchList.Count(m => m.Result == MatchResult.HomeWin);
        }
    }

    //원정 팀 매치 승수
    public int AwayMatchWins
    {
        get
        {
            return matchList.Count(m => m.Result == MatchResult.AwayWin);
        }
    }

    //게임 결과 판정
    public GameResult Result
    {
        get
        {
            //과반의 매치를 승리하면 게임 승리 조건 만족
            int matchesToWin = TotalMatches / 2 + 1;

            if (HomeMatchWins >= matchesToWin)
            {
                return GameResult.HomeWin;
            }
            if (AwayMatchWins >= matchesToWin)
            {
                return GameResult.AwayWin;
            }
            if (matchList.Count == TotalMatches && matchList.All(m => m.IsCompleted))
            {
                return GameResult.Draw;
            }
            //나중에 여기에서 기권 여부 확인해야 함
            return GameResult.Ongoing;
        }
    }

    //첫 매치 시작 또는 다음 매치로 진행 (성공 여부를 반환)
    public bool StartNextMatch(int totalRounds)
    {
        //완료된 게임인 경우 더 진행하지 않음
        if (Result.IsCompleted())
        {
            return false;
        }
        //총 매치 수 제한
        if (matchList.Count >= TotalMatches)
        {
            return false;
        }
        //마지막 매치가 완료되어야 진행할 수 있음
        if (CurrentMatch != null && !CurrentMatch.IsCompleted)
        {
            return false;
        }
        //새 매치 생성
        matchList.Add(new MatchScore(totalRounds));
        NotifyChange();
        return true;
    }

    //득점 기록 (성공 여부를 반환)
    public bool RecordScore(TeamSide team, ShotResult result)
    {
        //기록 가능한 상태가 아님
        if (!IsPlaying())
        {
            return false;
        }
        //기록 시도
        if (!CurrentMatch.RecordScore(team, result))
        {
            return false;
        }
        NotifyChange();
        return true;
    }

    //다음 라운드로 진행 (성공 여부를 반환)
    public bool StartNextRound()
    {
        //진행 가능한 상태가 아님
        if (!IsPlaying())
        {
            return false;
        }
        //진행 시도
        if (!CurrentMatch.StartNextRound())
        {
            return false;
        }
        NotifyChange();
        return true;
    }

    //매치 완료 (성공 여부를 반환)
    public bool CompleteCurrentMatch(MatchResult result)
    {
        //완료 가능한 상태가 아님
        if (!IsPlaying())
        {
            return false;
        }
        //완료 시도
        if (!CurrentMatch.CompleteMatch(result))
        {
            return false;
        }
        NotifyChange();
        return true;
    }
    
    //현재 매치의 (홈 팀 점수, 원정 팀 점수) 반환
    public (int Home, int Away) GetCurrentScores()
    {
        if (CurrentMatch == null)
        {
            return (0, 0);
        }
        return CurrentMatch.GetScores();
    }

    //이벤트 호출
    private void NotifyChange()
    {
        OnScoreboardChange?.Invoke(this);
    }

    //게임 진행 중인지 확인
    private bool IsPlaying()
    {
        return !Result.IsCompleted() && CurrentMatch != null;
    }
}