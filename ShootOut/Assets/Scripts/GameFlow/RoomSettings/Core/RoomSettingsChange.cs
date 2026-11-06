//방 설정 특정 항목의 값 변경 요청
public readonly struct RoomSettingsChange<T> where T : struct
{
    public readonly ERoomSettingsID id;
    public readonly T value;

    public RoomSettingsChange(ERoomSettingsID id, T value)
    {
        this.id = id;
        this.value = value;
    }
}

public readonly struct RoomSettingsDelta<T> where T : struct
{
    public readonly long revision;
    public readonly RoomSettingsChange<T> change;

    public RoomSettingsDelta(long revision, RoomSettingsChange<T> change)
    {
        this.revision = revision;
        this.change = change;
    }
}
