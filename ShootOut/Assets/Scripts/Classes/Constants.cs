using UnityEngine;

public class Constants
{
    public const int MIN_STAT = 0;
    public const int MAX_STAT = 100;

    public static readonly Vector3 PENALTY_SPOT = new(0f, 0.11f, 41f);
    public static readonly Vector3 GOAL_LINE = new(0f, 0f, 52f);
    public static readonly Vector3 KICKER_OFFSET_LEFT = new(-2f, -0.11f, -3f);
    public static readonly Vector3 KICKER_OFFSET_RIGHT = new(2f, -0.11f, -3f);
}
