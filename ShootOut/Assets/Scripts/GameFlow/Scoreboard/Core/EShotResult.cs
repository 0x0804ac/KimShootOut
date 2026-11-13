public enum EShotResult
{
    //입력 대기 중
    Pending = 0,
    //입력 후 진행 중
    Ongoing = 1,
    //득점
    Goal = 2,
    //골키퍼 선방
    Saved = 3,
    //골대 빗나감
    Missed = 4,
    //공 이동 시간 초과
    TimeOut = 5,
    //키커가 입력하지 않음
    NoInput = 6
}

//EShotResult 확장 클래스
public static class ShotResultExtensions
{
    public static bool IsCompleted(this EShotResult result)
    {
        return result == EShotResult.Goal ||
            result == EShotResult.Saved ||
            result == EShotResult.Missed ||
            result == EShotResult.TimeOut ||
            result == EShotResult.NoInput;
    }

    //current 상태에서 next로 변할 수 있는지 여부
    public static bool CanRecord(this EShotResult current, EShotResult next)
    {
        if (current == EShotResult.Pending)
        {
            return next == EShotResult.Ongoing ||
                next == EShotResult.Goal ||
                next == EShotResult.Saved ||
                next == EShotResult.Missed ||
                next == EShotResult.NoInput;
        }

        if (current == EShotResult.Ongoing)
        {
            return next == EShotResult.Goal ||
                next == EShotResult.Saved ||
                next == EShotResult.Missed ||
                next == EShotResult.TimeOut;
        }

        return false;
    }
}
