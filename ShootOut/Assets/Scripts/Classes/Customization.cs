using UnityEngine;

public class Customization
{
    private PlayerProfile profile;
    private Clothes shirt, pants, band;
    private Effect ball, player, goal, save;
}

public class Clothes
{
    private Color primary, secondary;
    private string pattern;
}

public class Effect
{
    private string passive, impact, trail; //particle
}