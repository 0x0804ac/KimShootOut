using System;

[Serializable]
//라운드의 점수를 기록하는 클래스
public sealed class RoundScore
{
    //몇번째 라운드인지 기록
    public int RoundNumber { get; }

    //홈 팀의 슛 결과
    public ShotResult Home { get; private set; }
    //원정 팀의 슛 결과
    public ShotResult Away { get; private set; }

    //진행 완료된 라운드인지 확인
    public bool IsCompleted
    {
        get
        {
            return Home != ShotResult.Pending && Away != ShotResult.Pending;
        }
    }

    //생성자(라운드 초기 상태)
    public RoundScore(int roundNumber)
    {
        RoundNumber = roundNumber;
        Home = ShotResult.Pending;
        Away = ShotResult.Pending;
    }

    //결과를 기록 후 기록 성공 여부를 반환
    public bool Record(TeamSide team, ShotResult result)
    {
        //홈 팀 결과 기록
        if (team == TeamSide.Home)
        {
            //결과가 이미 기록되어 있다면 기록하지 않음
            if (!CanRecord(result, Home))
            {
                return false;
            }
            Home = result;
            return true;
        }
        //원정 팀 점수 기록
        if (team == TeamSide.Away)
        {
            //결과가 이미 기록되어 있다면 기록하지 않음
            if (!CanRecord(result, Away))
            {
                return false;
            }
            Away = result;
            return true;
        }
        return false;
    }

    //currentRecord에서 newRecord으로 기록 가능한 상황인지 확인
    private bool CanRecord(ShotResult newRecord, ShotResult currentRecord)
    {
        //입력 대기 후 진행 중인 상황을 기록
        if (newRecord == ShotResult.Ongoing)
        {
            return currentRecord == ShotResult.Pending;
        }
        //공이 이동하다가 시간 초과된 상황을 기록
        if (newRecord == ShotResult.TimeOut)
        {
            return currentRecord == ShotResult.Ongoing;
        }
        //입력 대기 시간 동안 입력되지 않은 상황을 기록
        if (newRecord == ShotResult.NoInput)
        {
            return currentRecord == ShotResult.Pending;
        }
        //진행 완료된 상황을 기록
        if (newRecord != ShotResult.Pending)
        {
            return currentRecord.IsCompleted();
        }
        //이외의 상황에서는 기록하지 않음
        return false;
    }
}