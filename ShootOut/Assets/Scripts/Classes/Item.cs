using UnityEngine;

public class Item
{
    public readonly string NAME;
    public readonly string DISPLAY_NAME;
    public readonly ItemType TYPE;
    private int cost;

    public Item(string name, string displayName, ItemType type, int cost)
    {
        NAME = name;
        DISPLAY_NAME = displayName;
        TYPE = type;
        Cost = cost;
    }

    public int Cost
    {
        get => cost;
        set => cost = Mathf.Max(0, value);
    }
}
public class ItemList
{
    public const int ITEM_COUNT = 9;

    public readonly Item[] items;

    public ItemList()
    {
        items = new Item[ITEM_COUNT];
        //for (int i=0; i<items.Length; i++) items[i] = new Item(...);
    }
}
public enum ItemType { SELF_BUFF = 100, ENEMY_DEBUFF, OBSTRUCTION, CONDITIONAL }
