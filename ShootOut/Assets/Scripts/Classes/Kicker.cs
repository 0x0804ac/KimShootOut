using UnityEngine;

public class Kicker : SoccerPlayer
{
    private int power;
    private int accuracy;
    private bool isLeftFooted;

    public int Power
    {
        get => power;
        set { if (value >= Constants.MIN_STAT && value <= Constants.MAX_STAT) power = value; }
    }
    public int Accuracy
    {
        get => accuracy;
        set { if (value >= Constants.MIN_STAT && value <= Constants.MAX_STAT) accuracy = value; }
    }
    public bool IsLeftFooted
    {
        get => isLeftFooted;
    }

    public Kicker(int height, int weight, int maxStamina, int number, bool isCaptain, bool isInjured, bool isLeftFooted) : base(height, weight, maxStamina, number, isCaptain, isInjured)
    {
        RandomizeStats();
        this.isLeftFooted = isLeftFooted;
    }

    public Vector3 Kick(Vector3 input)
    {
        Vector3 output = input.normalized;
        float speed = input.magnitude;
        float inaccuracy = Mathf.Sqrt(Constants.MAX_STAT - accuracy);
        if (inaccuracy > 0f)
        {
            float randomX = Random.Range(-inaccuracy, inaccuracy);
            float randomY = Random.Range(-inaccuracy, inaccuracy);
            Quaternion spread = Quaternion.Euler(randomX, randomY, 0f);
            output = spread * output;
        }
        output *= speed;
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
            accuracy = Mathf.Min(Constants.MAX_STAT, accuracy * 11 / 10);
        }
    }

    public static Kicker PracticeKicker()
    {
        Kicker kicker = new(175, 75, Constants.MAX_STAT, 7, false, false, false);
        kicker.power = Constants.MAX_STAT;
        kicker.accuracy = Constants.MAX_STAT;
        return kicker;
    }
}
