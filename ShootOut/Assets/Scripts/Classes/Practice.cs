using UnityEngine;

public class Practice : Gamemode
{
    public const string DISPLAY_NAME = "연습";
    public const int PRACTICE_MAX_TURNS = 100;

    public Practice()
    {
        numberOfPlayers = 1;
        numberOfSpectators = 0;
        turn = 0;
    }

    public override void End()
    {
        //load previous scene (main menu => singleplayer)
    }

    public override void Load()
    {
        //load scene
        //show practice options UI
    }

    public override void Start()
    {
        //hide practice options UI
        //show & enable attacker|defender UI
        //(optional) show & enable toggle "bot control UI" button
    }

    public override void Turn()
    {
        if (turn < PRACTICE_MAX_TURNS)
        {
            turn++;
            //reset player controls (don't reset "bot control UI")
        }
        else
        {
            //show end of practice UI (press button => End())
        }
    }
}
