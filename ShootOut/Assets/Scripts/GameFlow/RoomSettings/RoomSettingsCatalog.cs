using System;
using System.Collections.Generic;

//방 기본 설정 목록
public static class RoomSettingsCatalog
{
    //목록 (private 필드)
    private static readonly RoomSettingsDefinition[] catalog =
    {
        RoomSettingsDefinition.Int(RoomSettingsID.PlayersPerTeam, "팀 인원", 1, 5, 1),
        RoomSettingsDefinition.Int(RoomSettingsID.MatchesPerGame, "매치 수", 1, 5, 1),
        RoomSettingsDefinition.Int(RoomSettingsID.RoundsPerMatch, "매치 당 키커 수", 5, 10, 5),
        RoomSettingsDefinition.Bool(RoomSettingsID.ForceFullRounds, "무조건 마지막 키커까지 진행", false),
        RoomSettingsDefinition.Int(RoomSettingsID.InputTimeLimit, "입력 제한 시간(초)", 5, 30, 10, 5),
        RoomSettingsDefinition.Float(RoomSettingsID.KickerChargeSpeed, "키커 조작 시 최대 파워까지 걸리는 시간", 0.5f, 2f, 1.25f, 0.25f),
        RoomSettingsDefinition.Float(RoomSettingsID.KeeperChargeSpeed, "골키퍼 조작 시 최대 속력까지 걸리는 시간", 0.5f, 2f, 1.25f, 0.25f),
        RoomSettingsDefinition.Bool(RoomSettingsID.EnableItems, "아이템 사용", false), //아이템 구현 이후 true로 바꿔야 함
        RoomSettingsDefinition.Int(RoomSettingsID.ItemSelectionTimeLimit, "아이템 개당 선택 제한 시간", 5, 60, 15, 5),
        RoomSettingsDefinition.Bool(RoomSettingsID.AllowSpectators, "관전 허용", false)
    };

    //테이블
    private static readonly Dictionary<RoomSettingsID, RoomSettingsDefinition> map = BuildMap();

    //목록 (public 속성)
    public static IReadOnlyList<RoomSettingsDefinition> CatalogList
    {
        get
        {
            return catalog;
        }
    }

    //테이블 생성 함수
    private static Dictionary<RoomSettingsID, RoomSettingsDefinition> BuildMap()
    {
        Dictionary<RoomSettingsID, RoomSettingsDefinition> result = new Dictionary<RoomSettingsID, RoomSettingsDefinition>();
        foreach (RoomSettingsDefinition item in catalog)
        {
            result.Add(item.ID, item);
        }
        return result;
    }

    //테이블 검색
    public static bool TryGet(RoomSettingsID id, out RoomSettingsDefinition definition)
    {
        return map.TryGetValue(id, out definition);
    }

    //기본 데이터 생성
    public static RoomSettingsData CreateDefaultData()
    {
        RoomSettingsData data = new RoomSettingsData();
        foreach (RoomSettingsDefinition item in catalog)
        {
            //기본값으로 변경하는 데이터 생성
            RoomSettingsChange change = item.ValueType switch
            {
                RoomSettingsValueType.Int => RoomSettingsChange.FromInt(item.ID, item.DefaultInt),
                RoomSettingsValueType.Float => RoomSettingsChange.FromFloat(item.ID, item.DefaultFloat),
                RoomSettingsValueType.Bool => RoomSettingsChange.FromBool(item.ID, item.DefaultBool),
                RoomSettingsValueType.String => RoomSettingsChange.FromString(item.ID, item.DefaultString),
                _ => throw new InvalidOperationException()
            };
            //변경 사항 적용 시도
            if (!RoomSettingsValidator.TryApply(ref data, change))
            {
                throw new InvalidOperationException($"Invalid default: {item.ID}");
            }
        }
        return data;
    }
}