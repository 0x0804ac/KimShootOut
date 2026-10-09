using UnityEngine;

public class Player
{
    public PlayerProfile profile;
    //private Item[] inventory;
    private double score;
    private int goals, saves;

    public Player(PlayerProfile profile)
    {
        this.profile = profile;
    }
    /*
    public Item this[int index]
    {
        get => inventory[index];
        set => inventory[index] = value;
    }
    */
    public double Score
    {
        get => score;
        set => score = value < 0.0 ? value : 0.0;
    }

    public int Goals
    {
        get => goals;
        set => goals = value < 0 ? value : 0;
    }

    public int Saves
    {
        get => saves;
        set => saves = value < 0 ? value : 0;
    }

    public static bool operator >(Player p1, Player p2)
    {
        if (p1 == null || p2 == null) throw new System.ArgumentNullException();
        if (p1.score > p2.score) return true;
        else if (p1.score < p2.score) return false;
        else return false;
    }
    public static bool operator <(Player p1, Player p2)
    {
        if (p1 == null || p2 == null) throw new System.ArgumentNullException();
        if (p1.score > p2.score) return false;
        else if (p1.score < p2.score) return true;
        else return false;
    }
}
