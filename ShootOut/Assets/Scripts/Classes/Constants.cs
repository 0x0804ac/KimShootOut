using UnityEngine;

public static class Constants
{
    public const string PRACTICE_MODE_CPU_BUTTON_PANEL = "cpu-button-panel";
    public const string PRACTICE_MODE_PLAYER_BUTTON_PANEL = "player-button-panel";
    public const string PRACTICE_MODE_CPU_PANEL = "cpu-control-panel";
    public const string PRACTICE_MODE_TOGGLE_BUTTON = "toggle-visibility";
    public const string PRACTICE_MODE_CPU_SLIDER = "cpu-power-slider";
    public const string PRACTICE_MODE_PLAYER_SLIDER = "player-power-slider";
    public const string CONTROLS_BUTTON_BOUND = "button-bound";
    public const string CONTROLS_DIRECTION_BUTTON = "direction-button";

    public const int MIN_STAT = 0;
    public const int MAX_STAT = 100;

    public static readonly Vector3 PENALTY_SPOT = new(0f, 0.11f, 41f);
    public static readonly Vector3 GOAL_LINE = new(0f, 0f, 52f);
    public static readonly Vector3 KICKER_OFFSET_LEFT = new(-2f, -0.11f, -3f);
    public static readonly Vector3 KICKER_OFFSET_RIGHT = new(2f, -0.11f, -3f);
}
