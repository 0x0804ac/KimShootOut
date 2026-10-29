//방 설정 값 자료형
public enum RoomSettingsValueType
{
    Int,
    Float,
    Bool,
    String
}

//방 설정 항목
public sealed class RoomSettingsDefinition
{
    //설정 번호
    public RoomSettingsID ID { get; }

    //설정이 표시될 때 쓰이는 이름
    public string DisplayName { get; }

    //설정값 자료형
    public RoomSettingsValueType ValueType { get; }

    //설정 최솟값
    public float Min { get; }
    //설정 최댓값
    public float Max { get; }
    //설정값 변경 시 허용 간격
    public float Step { get; }

    //int 형태의 설정 기본값
    public int DefaultInt { get; }
    //float 형태의 설정 기본값
    public float DefaultFloat { get; }
    //bool 형태의 설정 기본값
    public bool DefaultBool { get; }
    //string 형태의 설정 기본값
    public string DefaultString { get; }

    //방 설정 항목 생성
    private RoomSettingsDefinition(RoomSettingsID id, string displayName, RoomSettingsValueType valueType, float min, float max, float step, int defaultInt, float defaultFloat, bool defaultBool, string defaultString)
    {
        ID = id;
        DisplayName = displayName;
        ValueType = valueType;
        Min = min;
        Max = max;
        Step = step;
        DefaultInt = defaultInt;
        DefaultFloat = defaultFloat;
        DefaultBool = defaultBool;
        DefaultString = defaultString;
    }

    //int 형태의 설정 항목 생성
    public static RoomSettingsDefinition Int(RoomSettingsID id, string displayName, int min, int max, int defaultValue, int step = 1)
    {
        return new RoomSettingsDefinition(id, displayName, RoomSettingsValueType.Int, min, max, step, defaultValue, RoomSettingsConstants.dummyFloat, RoomSettingsConstants.dummyBool, RoomSettingsConstants.dummyString);
    }

    //float 형태의 설정 항목 생성
    public static RoomSettingsDefinition Float(RoomSettingsID id, string displayName, float min, float max, float defaultValue, float step = 1f)
    {
        return new RoomSettingsDefinition(id, displayName, RoomSettingsValueType.Float, min, max, step, RoomSettingsConstants.dummyInt, defaultValue, RoomSettingsConstants.dummyBool, RoomSettingsConstants.dummyString);
    }

    //bool 형태의 설정 항목 생성
    public static RoomSettingsDefinition Bool(RoomSettingsID id, string displayName, bool defaultValue)
    {
        return new RoomSettingsDefinition(id, displayName, RoomSettingsValueType.Bool, 0f, 1f, 1f, RoomSettingsConstants.dummyInt, RoomSettingsConstants.dummyFloat, defaultValue, RoomSettingsConstants.dummyString);
    }

    //string 형태의 설정 항목 생성
    public static RoomSettingsDefinition String(RoomSettingsID id, string displayName, string defaultValue)
    {
        return new RoomSettingsDefinition(id, displayName, RoomSettingsValueType.String, 0f, 1f, 1f, RoomSettingsConstants.dummyInt, RoomSettingsConstants.dummyFloat, RoomSettingsConstants.dummyBool, defaultValue);
    }
}
