using UnityEngine;

public abstract class Versus : Gamemode
{
    protected Player[] players;

    public Versus()
    {
        players = new Player[2];
    }

    public abstract GamemodeType GetGamemodeType();
    public abstract string GetDisplayName();
    public abstract GameState GetGameState();
}
