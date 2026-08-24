using UnityEngine;

public class Practice : Gamemode
{
    public const string DISPLAY_NAME = "연습";
    public const string ATTACK = "공격";
    public const string DEFENSE = "수비";
    public const int PRACTICE_MAX_TURNS = 100;

    private readonly PracticeType type;
    private readonly bool canControlBot;
    private readonly PracticeModeScript script;
    
    public PracticeType Type
    {
        get => type;
    }
    public bool CanControlBot
    {
        get => canControlBot;
    }
    public bool IsReady { get; set; }

    public string DisplayName
    {
        get
        {
            return type switch
            {
                PracticeType.ATTACK => ATTACK,
                PracticeType.DEFENSE => DEFENSE,
                _ => DISPLAY_NAME,
            };
        }
    }

    public int Attempts { get => Mathf.Max(0, turn); }
    public int Goals { get; private set; }
    public int Saves { get; private set; }

    public Practice(PracticeType type, bool canControlBot, PracticeModeScript script)
    {
        numberOfPlayers = 1;
        numberOfSpectators = 0;
        turn = 0;
        Goals = 0;
        Saves = 0;
        this.type = type;
        this.canControlBot = canControlBot;
        this.script = script;
        StaticValues.attacker = Kicker.PracticeKicker();
        StaticValues.defender = Goalkeeper.PracticeKeeper();
        IsReady = false;
    }

    public override void End()
    {
        script.ShowResults();
    }

    public override void Load()
    {
        Debug.Log("Loading Practice mode");
        script.ResetObjects();
    }

    public override void Start()
    {
        Debug.Log("Starting Practice mode");
        script.ResetControls();
        IsReady = true;
    }

    public override void Turn()
    {
        if (turn >= PRACTICE_MAX_TURNS)
        {
            Debug.Log("Maximum turns reached");
            End();
        }
        else
        {
            turn++;
            script.ResetObjects();
            script.ResetControls();
            if (script.Manager.Game.IsGoal) Goals++;
            else if (script.Manager.Game.IsSave) Saves++;
            script.Manager.Game.ResetValues();
            IsReady = true;
        }
    }
}

public enum PracticeType { ATTACK = 1, DEFENSE };
