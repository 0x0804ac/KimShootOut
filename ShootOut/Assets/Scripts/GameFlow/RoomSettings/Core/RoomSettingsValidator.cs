using System;

//방 설정 변경 검증
public static class RoomSettingsValidator
{
    //설정 변경 시도 (성공 여부를 반환)
    public static bool TryApply(ref RoomSettingsData settings, RoomSettingsChange change)
    {
        //존재하지 않는 설정
        if (!RoomSettingsCatalog.TryGet(change.ID, out RoomSettingsDefinition definition))
        {
            return false;
        }
        //자료형 불일치
        if (change.ValueType != definition.ValueType)
        {
            return false;
        }
        //올바르지 않은 설정값
        if (!IsValidValue(definition, change))
        {
            return false;
        }
        RoomSettingsData updatedSettings = settings;
        switch (change.ID)
        {
            case RoomSettingsID.PlayersPerTeam:
                updatedSettings.PlayersPerTeam = change.IntValue;
                break;
            case RoomSettingsID.MatchesPerGame:
                updatedSettings.MatchesPerGame = change.IntValue;
                break;
            case RoomSettingsID.RoundsPerMatch:
                updatedSettings.RoundsPerMatch = change.IntValue;
                break;
            case RoomSettingsID.ForceFullRounds:
                updatedSettings.ShouldForceFullRounds = change.BoolValue;
                break;
            case RoomSettingsID.InputTimeLimit:
                updatedSettings.InputTimeLimitInSeconds = change.IntValue;
                break;
            case RoomSettingsID.KickerChargeSpeed:
                updatedSettings.KickerFullChargeTimeInSeconds = change.FloatValue;
                break;
            case RoomSettingsID.GoalkeeperChargeSpeed:
                updatedSettings.GoalkeeperFullChargeTimeInSeconds = change.FloatValue;
                break;
            case RoomSettingsID.EnableItems:
                updatedSettings.IsItemEnabled = change.BoolValue;
                break;
            case RoomSettingsID.ItemSelectionTimeLimit:
                updatedSettings.ItemSelectionTimeLimitInSeconds = change.IntValue;
                break;
            case RoomSettingsID.AllowSpectators:
                updatedSettings.AllowSpectators = change.BoolValue;
                break;
            default:
                return false;
        }
        settings = updatedSettings;
        return true;
    }

    //올바른 설정값인지 검사
    private static bool IsValidValue(RoomSettingsDefinition definition, RoomSettingsChange change)
    {
        //bool형 값은 true, false 모두 사용됨
        if (definition.ValueType == RoomSettingsValueType.Bool)
        {
            return true;
        }
        //string형 값
        if (definition.ValueType == RoomSettingsValueType.String)
        {
            return true; //추후 검증 로직 추가 예정
        }
        //int 또는 float형 값
        double value = definition.ValueType == RoomSettingsValueType.Int ? change.IntValue : change.FloatValue;
        //NaN, 무한대
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            return false;
        }
        //(최솟값 ~ 최댓값) 범위 벗어남
        if (value < definition.Min || value > definition.Max)
        {
            return false;
        }
        //올바르지 않은 간격(단위)
        if (definition.Step <= 0)
        {
            return false;
        }
        //간격에 맞는 값인지 검사
        double steps = (value - definition.Min) / definition.Step;
        double nearestValue = Math.Round(steps);
        return Math.Abs(steps - nearestValue) < 0.0001;
    }
}