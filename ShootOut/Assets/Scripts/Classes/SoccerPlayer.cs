using UnityEngine;

public class SoccerPlayer
{
    protected int height;
    protected int weight;
    protected int stamina;
    protected int number;
    protected bool isCaptain;
    protected bool isInjured;

    public int Height => height;
    public int Weight => weight;
    public int Stamina
    {
        get => stamina;
        set => stamina = Mathf.Max(Constants.MIN_STAT, value);
    }
    public int Number => number;
    public bool IsCaptain => isCaptain;
    public bool IsInjured
    {
        get => isInjured;
        set => isInjured = value;
    }

    public SoccerPlayer()
    {
        height = 175;
        weight = 75;
        stamina = 10;
        number = 5;
        isCaptain = false;
        isInjured = false;
    }

    public SoccerPlayer(int height, int weight, int maxStamina, int number, bool isCaptain, bool isInjured)
    {
        this.height = height;
        this.weight = weight;
        Stamina = maxStamina;
        this.number = number;
        this.isCaptain = isCaptain;
        IsInjured = isInjured;
    }

    public bool IsExhausted() => stamina == Constants.MIN_STAT;
}
