using UnityEngine;
using UnityEngine.SceneManagement;

public class Practice : Gamemode
{
    public const string DISPLAY_NAME = "연습";
    public const int PRACTICE_MAX_TURNS = 100;

    private PracticeType type;
    private bool canControlBot;
    private PracticeModeScript script;
    private Kicker attacker;
    private Goalkeeper defender;
    public PracticeType Type
    {
        get => type;
    }
    public bool CanControlBot
    {
        get => canControlBot;
    }
    public Kicker Attacker
    {
        get => attacker;
    }
    public Goalkeeper Defender
    {
        get => defender;
    }
    public bool IsReady { get; set; }

    public Practice(PracticeType type, bool canControlBot, PracticeModeScript script)
    {
        numberOfPlayers = 1;
        numberOfSpectators = 0;
        turn = 0;
        this.type = type;
        this.canControlBot = canControlBot;
        this.script = script;
        attacker = Kicker.PracticeKicker();
        defender = Goalkeeper.PracticeKeeper();
        IsReady = false;
    }

    public override void End()
    {
        Debug.Log("Ending Practice mode");
        SceneManager.LoadScene("MainMenu");
    }

    public override void Load()
    {
        Debug.Log("Loading Practice mode");
        script.PlayIdleAnimation();
    }

    public override void Start()
    {
        Debug.Log("Starting Practice mode");
        IsReady = true;
    }

    public override void Turn()
    {
        if (turn > PRACTICE_MAX_TURNS)
        {
            Debug.Log("Maximum turns reached");
            End(); //replace with a panel (label + button with End() attached)
        }
        else
        {
            turn++;
            script.ResetObjects();
            script.ResetControls();
            IsReady = true;
        }
    }
}

public enum PracticeType { ATTACK = 1, DEFENSE };
