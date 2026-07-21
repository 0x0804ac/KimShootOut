using UnityEngine;

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
    }

    public override void End()
    {
        //load previous scene (main menu => singleplayer)
    }

    public override void Load()
    {
        //load scene
    }

    public override void Start()
    {
        //show & enable attacker|defender UI
        //show & enable show/hide "bot controls" button
    }

    public override void Turn()
    {
        if (turn > PRACTICE_MAX_TURNS)
        {
            //show end of practice UI (press button => End())
        }
        else
        {
            turn++;
            script.ResetObjects();
            //reset player controls (don't reset "bot control UI")
        }
    }
}

public enum PracticeType { ATTACK = 1, DEFENSE };
