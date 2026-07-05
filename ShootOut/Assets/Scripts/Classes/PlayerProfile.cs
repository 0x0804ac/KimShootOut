using UnityEngine;

public class PlayerProfile
{
    private long id;
    private string name;
    private bool isBot;
    private double rating;
    //skins, uniforms, cosmetics, statistics

    public long ID
    {
        get => id;
    }
    public string Name
    {
        get => name;
    }
    public bool IsBot
    {
        get => isBot;
    }
    public double Rating
    {
        get => rating;
        set => rating = value < 0.0 ? 0.0 : value;
    }

    private PlayerProfile()
    {
        id = 0L; //randomize
        name = "Bot"; //random name
        isBot = true;
    }

    public PlayerProfile(long playerID)
    {
        id = playerID;
        //get profile data from player ID
        name = "Player"; //get name from player ID
        isBot = false;
        rating = 0.0; //get rating from player ID
    }

    public static PlayerProfile BotProfile(double difficulty)
    {
        PlayerProfile bot = new();
        bot.rating = difficulty;
        return bot;
    }
}
