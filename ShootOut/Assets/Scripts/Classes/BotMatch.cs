using UnityEngine;

public class BotMatch : Versus
{
    public const GamemodeType TYPE = GamemodeType.SINGLEPLAYER;
    public const string DISPLAY_NAME = "봇 대전";

    private GameState state;

    public override void End()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public override void Start()
    {
        throw new System.NotImplementedException();
    }

    public override void Turn()
    {
        throw new System.NotImplementedException();
    }

    public override GameState GetGameState()
    {
        return state;
    }

    public BotMatch(Player player, double difficulty)
    {
        players[0] = player;
        players[1] = new Player(PlayerProfile.BotProfile(difficulty));
        state = GameState.LOADING;
    }
}
