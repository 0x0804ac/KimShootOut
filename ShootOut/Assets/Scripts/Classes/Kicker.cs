using UnityEngine;

public class Kicker : SoccerPlayer
{
    private int power;
    private int accuracy;
    private bool isLeftFooted;

    public int Power
    {
        get => power;
        set => power = value;
    }
    public int Accuracy
    {
        get => accuracy;
        set => accuracy = value;
    }

    public Kicker(int height, int weight, int maxStamina, int number, bool isCaptain, bool isInjured, bool isLeftFooted) : base(height, weight, maxStamina, number, isCaptain, isInjured)
    {
        RandomizeStats();
        this.isLeftFooted = isLeftFooted;
    }

    public Vector3 Kick(Vector3 input)
    {
        Vector3 output = input;
        return output;
    }

    private void RandomizeStats()
    {
        var rng = new System.Random();
        power = rng.Next(50) + 50;
        accuracy = rng.Next(50) + 50;
        if (isCaptain)
        {
            power = Mathf.Max(Constants.MAX_STAT, power * 11 / 10);
            accuracy = Mathf.Max(Constants.MAX_STAT, accuracy * 11 / 10);
        }
    }
}
