using UnityEngine;

public class Goalkeeper : SoccerPlayer
{
    private int speed;
    public int Speed
    {
        get => speed;
        set { if (value >= Constants.MIN_STAT && value <= Constants.MAX_STAT) speed = value; }
    }

    public Vector3 Goalkeep(Vector3 input)
    {
        Vector3 output = input * (1.0f * speed / Constants.MAX_STAT);
        output.z = -1.0f;
        return output;
    }

    public static Goalkeeper PracticeKeeper()
    {
        Goalkeeper keeper = new() { speed = Constants.MAX_STAT };
        return keeper;
    }
}
