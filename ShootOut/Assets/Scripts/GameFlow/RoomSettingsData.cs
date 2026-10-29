public enum RoomSettingsValueType
{
    Int, Float, Bool, String, Enum
}

public enum RoomSettingsID
{
    PlayersPerTeam,
    RoundsPerMatch,
    MatchesPerGame,
    ShouldEnforceFullRounds,
    InputTimeLimitInSeconds,
    KickerFullChargeInSeconds,
    KeeperFullChargeInSeconds,
    IsItemEnabled,
    ItemSelectionTimeLimitInSeconds
}

public readonly struct RoomSettingsData
{
    public readonly RoomSettingsID Id;
    public readonly string DisplayName;
    public readonly RoomSettingsValueType ValueType;

    public readonly float Min;
    public readonly float Max;
    public readonly float Step;

    public RoomSettingsData(
        RoomSettingsID id,
        string displayName,
        RoomSettingsValueType valueType,
        float min = 0,
        float max = 0,
        float step = 1)
    {
        Id = id;
        DisplayName = displayName;
        ValueType = valueType;
        Min = min;
        Max = max;
        Step = step;
    }
}