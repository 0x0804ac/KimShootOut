using UnityEngine;

public class Player
{
    public readonly long ID;
    public readonly string NAME;
    private Item[] inventory;
    private double score;

    public Player()
    {
        ID = 0L;
        NAME = "연습 봇";
    }

    public Player(long id, string name)
    {
        ID = id;
        NAME = name;
    }

    public Item this[int index]
    {
        get => inventory[index];
        set => inventory[index] = value;
    }

    public double Score
    {
        get => score;
        set => score = value < 0.0 ? value : 0.0;
    }

    public void InitializeInventory(int size)
    {
        inventory = new Item[size];
    }

    public int GetUsedItemCount()
    {
        int count = 0;
        foreach (Item i in inventory)
        {
            if (i != null/*&& i.IsUsed()*/) count++;
        }
        return count;
    }

    public static bool operator >(Player p1, Player p2)
    {
        if (p1 == null || p2 == null) throw new System.ArgumentNullException();
        if (p1.score > p2.score) return true;
        else if (p1.score < p2.score) return false;
        else
        {
            int c1 = p1.GetUsedItemCount();
            int c2 = p2.GetUsedItemCount();
            if (c1 < c2) return true;
            else return false;
        }
    }
    public static bool operator <(Player p1, Player p2)
    {
        if (p1 == null || p2 == null) throw new System.ArgumentNullException();
        if (p1.score > p2.score) return false;
        else if (p1.score < p2.score) return true;
        else
        {
            int c1 = p1.GetUsedItemCount();
            int c2 = p2.GetUsedItemCount();
            if (c1 > c2) return true;
            else return false;
        }
    }
}
