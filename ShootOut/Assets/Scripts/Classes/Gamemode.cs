using UnityEngine;

public abstract class Gamemode
{
    protected int numberOfPlayers;
    protected int numberOfSpectators;
    protected int turn;

    public abstract void Load();
    public abstract void Start();
    public abstract void Turn();
    public abstract void End();
}
public enum GamemodeType { SINGLEPLAYER = 1, MULTIPLAYER, CUSTOM }
public enum GameState { LOADING = 10, STARTING, ONGOING, ENDING }