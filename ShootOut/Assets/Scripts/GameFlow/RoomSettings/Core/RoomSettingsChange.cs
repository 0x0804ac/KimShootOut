//방 설정 변경 요청 구조체
public readonly struct RoomSettingsChange
{
    //설정 ID
    public RoomSettingsID ID { get; }
    //설정값 자료형
    public RoomSettingsValueType ValueType { get; }

    //int형 값
    public int IntValue { get; }
    //float형 값
    public float FloatValue { get; }
    //bool형 값
    public bool BoolValue { get; }
    //string형 값
    public string StringValue { get; }

    //새 요청 생성 (private)
    private RoomSettingsChange(RoomSettingsID id, RoomSettingsValueType valueType,  int intValue, float floatValue, bool boolValue, string stringValue)
    {
        ID = id;
        ValueType = valueType;
        IntValue = intValue;
        FloatValue = floatValue;
        BoolValue = boolValue;
        StringValue = stringValue;
    }

    //int형 값 변경 요청 생성 (public)
    public static RoomSettingsChange FromInt(RoomSettingsID id, int value)
    {
        return new RoomSettingsChange(id, RoomSettingsValueType.Int, value, RoomSettingsConstants.dummyFloat, RoomSettingsConstants.dummyBool, RoomSettingsConstants.dummyString);
    }

    //float형 값 변경 요청 생성 (public)
    public static RoomSettingsChange FromFloat(RoomSettingsID id, float value)
    {
        return new RoomSettingsChange(id, RoomSettingsValueType.Float, RoomSettingsConstants.dummyInt, value, RoomSettingsConstants.dummyBool, RoomSettingsConstants.dummyString);
    }

    //bool형 값 변경 요청 생성 (public)
    public static RoomSettingsChange FromBool(RoomSettingsID id, bool value)
    {
        return new RoomSettingsChange(id, RoomSettingsValueType.Bool, RoomSettingsConstants.dummyInt, RoomSettingsConstants.dummyFloat, value, RoomSettingsConstants.dummyString);
    }

    //string형 값 변경 요청 생성 (public)
    public static RoomSettingsChange FromString(RoomSettingsID id, string value)
    {
        return new RoomSettingsChange(id, RoomSettingsValueType.String, RoomSettingsConstants.dummyInt, RoomSettingsConstants.dummyFloat, RoomSettingsConstants.dummyBool, value);
    }
}