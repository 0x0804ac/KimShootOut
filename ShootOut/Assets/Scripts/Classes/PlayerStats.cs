public class PlayerStats
{
    private PlayerProfile profile;
    private int matches, wins, loses;
    private MatchStats lastMatch;

    public float WinRate => (0f + wins) / matches;

    public string WinRateText()
    {
        return (WinRate * 100).ToString("P2");
    }
}
