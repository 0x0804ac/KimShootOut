using UnityEngine;

public class Tutorial : Gamemode
{
    public const string DISPLAY_NAME = "튜토리얼";

    public Tutorial()
    {
        numberOfPlayers = 1;
        numberOfSpectators = 0;
        turn = 0;
    }

    public override void End()
    {
        //complete tutorial
        //load previous scene(main menu => singleplayer)
    }

    public override void Load()
    {
        //load scene
        //show attacker UI
        //show tutorial guide text
    }

    public override void Start()
    {
        //enable attacker UI
    }

    public override void Turn()
    {
        switch (turn)
        {
            case 0:
                //hide attacker UI
                //show defender UI
                turn++;
                break;
            case 1:
                //hide defender UI
                //show item UI
                turn++;
                break;
            default:
                End();
                break;
        }
    }
}
