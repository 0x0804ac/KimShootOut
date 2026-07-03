using UnityEngine;

public abstract class Versus : Gamemode
{
    protected Player[] players;

    public Versus()
    {
        players = new Player[2];
        AddPlayers();
    }

    protected abstract void AddPlayers();
}
