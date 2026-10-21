using System;

//슛 결과 열거형
public enum ShootResult
{
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

//열거형 자체에서는 메서드를 정의할 수 없어서 확장 메서드를 사용함
public static class ShootResultExtensions
{
    //한글로 변환
    public static string GetDisplayName(this ShootResult result)
    {
        return result switch
        {
            ShootResult.Goal => "골",
            ShootResult.Saved => "선방",
            ShootResult.Missed => "빗나감",
            ShootResult.TimeOut => "시간 초과",
            ShootResult.NoInput => "입력하지 않음",
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }

    //득점 성공 여부
    public static bool IsGoal(this ShootResult result)
    {
        return result == ShootResult.Goal;
    }

    //획득 점수
    public static int GetScore(this ShootResult result)
    {
        if (result == ShootResult.Goal) return 1;
        else return 0;
    }
}