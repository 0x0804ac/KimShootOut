using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
//매치의 점수를 기록하는 클래스
public sealed class MatchScore
{
    //총 라운드 수
    public int TotalRounds { get; }

    //현재 라운드 번호 (1라운드부터 시작)
    public int CurrentRoundNumber
    {
        get
        {
            return Rounds.Count;
        }
    }

    //라운드 점수 목록 (수정 불가)
    public IReadOnlyList<RoundScore> Rounds
    {
        get
        {
            return roundList;
        }
    }

    //라운드 점수 목록 (클래스 내부에서만 수정 가능)
    private readonly List<RoundScore> roundList = new List<RoundScore>();

    //매치 결과
    public MatchResult Result { get; private set; }

    //홈 팀 점수
    public int HomeScore
    {
        get
        {
            return roundList.Count(r => r.Home == ShotResult.Goal);
        }
    }

    //원정 팀 점수
    public int AwayScore
    {
        get
        {
            return roundList.Count(r => r.Away == ShotResult.Goal);
        }
    }

    //연장 라운드 여부
    public bool IsExtraRound
    {
        get
        {
            return CurrentRoundNumber > TotalRounds;
        }
    }

    //매치가 완료되었는지 여부
    public bool IsCompleted
    {
        get
        {
            return Result.IsCompleted();
        }
    }

    //라운드 수를 받아서 매치를 생성
    public MatchScore(int totalRounds)
    {
        if (totalRounds < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(totalRounds));
        }
        TotalRounds = totalRounds;
        roundList.Add(new RoundScore(1));
        Result = MatchResult.Ongoing;
    }

    //현재 라운드의 득점 결과를 기록 (성공 여부를 반환)
    public bool RecordScore(TeamSide team, ShotResult result)
    {
        //완료된 매치일 경우 더 기록하지 않음
        if (IsCompleted)
        {
            return false;
        }
        RoundScore currentRound = roundList[^1];
        return currentRound.Record(team, result);
    }

    //다음 라운드로 진행 (성공 여부를 반환)
    public bool StartNextRound()
    {
        //완료된 매치인 경우 더 진행하지 않음
        if (IsCompleted)
        {
            return false;
        }
        //마지막 라운드가 완료되어야 진행할 수 있음
        if (!roundList[^1].IsCompleted)
        {
            return false;
        }
        //다음 라운드 생성
        roundList.Add(new RoundScore(CurrentRoundNumber + 1));
        return true;
    }

    //매치를 완료하여 결과 기록 (성공 여부를 반환)
    public bool CompleteMatch(MatchResult result)
    {
        //이미 완료된 매치일 경우
        if (IsCompleted)
        {
            return false;
        }
        //결과 기록
        Result = result;
        return true;
    }

    //(홈 팀 점수, 원정 팀 점수) 반환
    public (int Home, int Away) GetScores()
    {
        return (HomeScore, AwayScore);
    }
}