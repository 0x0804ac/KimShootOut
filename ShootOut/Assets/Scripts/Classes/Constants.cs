using UnityEngine;

public static class Constants
{
    public const string TAG_KICKER = "Kicker";
    public const string TAG_GOALKEEPER = "Goalkeeper";
    public const string TAG_BALL = "Ball";

    public const string PRACTICE_MODE_CPU_BUTTON_PANEL = "cpu-button-panel";
    public const string PRACTICE_MODE_PLAYER_BUTTON_PANEL = "player-button-panel";
    public const string PRACTICE_MODE_CPU_PANEL = "cpu-control-panel";
    public const string PRACTICE_MODE_TOGGLE_BUTTON = "toggle-visibility";
    public const string PRACTICE_MODE_CPU_SLIDER = "cpu-power-slider";
    public const string PRACTICE_MODE_PLAYER_SLIDER = "player-power-slider";
    public const string CONTROLS_BUTTON_BOUND = "button-bound";
    public const string CONTROLS_DIRECTION_BUTTON = "direction-button";
    public const string CONTROLS_POWER_SLIDER = "power-slider";

    public const int MIN_STAT = 0;
    public const int MAX_STAT = 100;

    public const float MULTIPLIER_X = 0.1f;
    public const float MULTIPLIER_Y = -0.1f;
    public const float MULTIPLIER_Z = 0.4f;
    public const float MULTIPLIER = 0.01f;

    public static readonly int ANIMATOR_KICKER_IDLE = Animator.StringToHash("Base Layer.Idle");
    public static readonly int ANIMATOR_KICKER_SHOOT = Animator.StringToHash("Base Layer.Shoot");
    public static readonly int ANIMATOR_KICKER_SHOOT_WEAK_LEFT = Animator.StringToHash("Base Layer.Shoot.Weak Left");
    public static readonly int ANIMATOR_KICKER_SHOOT_WEAK_RIGHT = Animator.StringToHash("Base Layer.Shoot.Weak Right");
    public static readonly int ANIMATOR_KICKER_SHOOT_NORMAL = Animator.StringToHash("Base Layer.Shoot.Normal");
    public static readonly int ANIMATOR_KICKER_SHOOT_STRONG = Animator.StringToHash("Base Layer.Shoot.Strong");
    public static readonly int ANIMATOR_GOALKEEPER_IDLE = Animator.StringToHash("Base Layer.Idle");
    public static readonly int ANIMATOR_GOALKEEPER_IDLE_ARMS_SIDE = Animator.StringToHash("Base Layer.Idle.Arms Side");
    public static readonly int ANIMATOR_GOALKEEPER_IDLE_ARMS_FRONT = Animator.StringToHash("Base Layer.Idle.Arms Front");
    public static readonly int ANIMATOR_GOALKEEPER_SIDESTEP = Animator.StringToHash("Base Layer.Sidestep");
    public static readonly int ANIMATOR_GOALKEEPER_SIDESTEP_LEFT = Animator.StringToHash("Base Layer.Sidestep.Left");
    public static readonly int ANIMATOR_GOALKEEPER_SIDESTEP_RIGHT = Animator.StringToHash("Base Layer.Sidestep.Right");
    public static readonly int ANIMATOR_GOALKEEPER_DIVE = Animator.StringToHash("Base Layer.Dive");
    public static readonly int ANIMATOR_GOALKEEPER_DIVE_SHORT_LEFT = Animator.StringToHash("Base Layer.Dive.Short Left");
    public static readonly int ANIMATOR_GOALKEEPER_DIVE_SHORT_RIGHT = Animator.StringToHash("Base Layer.Dive.Short Right");
    public static readonly int ANIMATOR_GOALKEEPER_DIVE_NORMAL_LEFT = Animator.StringToHash("Base Layer.Dive.Normal Left");
    public static readonly int ANIMATOR_GOALKEEPER_DIVE_LONG_LEFT = Animator.StringToHash("Base Layer.Dive.Long Left");
    public static readonly int ANIMATOR_GOALKEEPER_DIVE_LONG_RIGHT = Animator.StringToHash("Base Layer.Dive.Long Right");
    public static readonly int ANIMATOR_GOALKEEPER_CATCH = Animator.StringToHash("Base Layer.Catch");
    public static readonly int ANIMATOR_GOALKEEPER_CATCH_JUMP = Animator.StringToHash("Base Layer.Catch.Jump");
    public static readonly int ANIMATOR_GOALKEEPER_CATCH_HIGH = Animator.StringToHash("Base Layer.Catch.High");
    public static readonly int ANIMATOR_GOALKEEPER_CATCH_NORMAL = Animator.StringToHash("Base Layer.Catch.Normal");
    public static readonly int ANIMATOR_GOALKEEPER_CATCH_LOW = Animator.StringToHash("Base Layer.Catch.Low");
    public static readonly int ANIMATOR_GOALKEEPER_CATCH_JUMP_MISS = Animator.StringToHash("Base Layer.Catch.Jump Miss");

    public const string ANIMATOR_TRIGGER_IDLE = "Idle";
    public const string ANIMATOR_TRIGGER_SHOOT = "Shoot";
    public const string ANIMATOR_TRIGGER_GOALKEEP = "Goalkeep";
    public const string ANIMATOR_VELOCITY_X = "Velocity X";
    public const string ANIMATOR_VELOCITY_Y = "Velocity Y";
    public const string ANIMATOR_VELOCITY_Z = "Velocity Z";

    public static readonly Vector3 PENALTY_SPOT = new(0f, 0.11f, 41f);
    public static readonly Vector3 GOAL_LINE = new(0f, 0f, 52f);
    public static readonly Vector3 KICKER_OFFSET_LEFT = new(-1f, -0.11f, -2f);
    public static readonly Vector3 KICKER_OFFSET_RIGHT = new(1f, -0.11f, -2f);
}
