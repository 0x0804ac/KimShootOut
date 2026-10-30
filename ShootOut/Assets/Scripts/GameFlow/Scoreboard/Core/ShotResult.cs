//슛 결과
public enum ShotResult
{
    //입력 대기 중
    Pending,
    //입력 후 진행 중
    Ongoing,
    //득점
    Goal,
    //골키퍼 선방
    Saved,
    //골대 빗나감
    Missed,
    //공 이동 시간 초과
    TimeOut,
    //키커가 입력하지 않음
    NoInput
}

//ShotResult 확장 클래스
public static class ShotResultExtensions
{
    //표시 이름 반환
    public static string GetDisplayName(this ShotResult result)
    {
        switch (result)
        {
            case ShotResult.Pending:
                return "대기 중";
            case ShotResult.Ongoing:
                return "진행 중";
            case ShotResult.Goal:
                return "득점";
            case ShotResult.Saved:
                return "선방";
            case ShotResult.Missed:
                return "실축";
            case ShotResult.TimeOut:
                return "공 시간 초과";
            case ShotResult.NoInput:
                return "입력 시간 초과";
            default:
                return result.ToString();
        }
    }

    //완료된 결과인지 확인
    public static bool IsCompleted(this ShotResult result)
    {
        return result != ShotResult.Pending && result != ShotResult.Ongoing;
    }
}
