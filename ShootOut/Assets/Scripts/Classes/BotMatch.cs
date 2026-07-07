using UnityEngine;

public class BotMatch : Versus
{
    public const GamemodeType TYPE = GamemodeType.SINGLEPLAYER;
    public const string DISPLAY_NAME = "봇 대전";

    private GameState state;

    public override void End()
    {
        state = GameState.ENDING;
    }

    public override string GetDisplayName()
    {
        return DISPLAY_NAME;
    }

    public override GamemodeType GetGamemodeType()
    {
        return TYPE;
    }

    public override void Load()
    {
        state = GameState.LOADING;
        //load items if enabled
        //load scene
    }

    public override void Start()
    {
        state = GameState.STARTING;
    }

    public override void Turn()
    {
        turn++;
    }

    public override GameState GetGameState()
    {
        return state;
    }

    public BotMatch(Player player, double difficulty)
    {
        players[0] = player;
        players[1] = new Player(PlayerProfile.BotProfile(difficulty));
        Load();
    }
}
