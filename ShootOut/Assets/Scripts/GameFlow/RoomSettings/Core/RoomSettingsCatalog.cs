/*
using System;
using System.Collections.Generic;
*/
//설정 항목과 기본값이 저장된 클래스
public static class RoomSettingsCatalog
{
    private static readonly IRoomSettingsDefinition[] sDefinitions =
    {
        new RoomSettingsDefinition<int>(
            ERoomSettingsID.PlayersPerTeam,
            new LocalizationMetadata("팀 인원", "Players per team"),
            1, new RoomSettingsRange<int>(1, 5, 1)),
        new RoomSettingsDefinition<int>(
            ERoomSettingsID.MatchesPerGame,
            new LocalizationMetadata("매치 수", "Matches per game"),
            1, new RoomSettingsRange<int>(1, 5, 2)),
        new RoomSettingsDefinition<int>(
            ERoomSettingsID.RoundsPerMatch,
            new LocalizationMetadata("매치 당 슛 횟수", "Shots per match"),
            5, new RoomSettingsRange<int>(5, 10, 1)),
        new RoomSettingsDefinition<bool>(
            ERoomSettingsID.ForceFullRounds,
            new LocalizationMetadata("무조건 마지막 키커까지 진행", "Enforce full rounds"),
            false),
        new RoomSettingsDefinition<int>(
            ERoomSettingsID.InputTimeLimit,
            new LocalizationMetadata("입력 제한 시간 (초)", "Input time limit (seconds)"),
            10, new RoomSettingsRange<int>(5, 30, 5)),
        new RoomSettingsDefinition<float>(
            ERoomSettingsID.KickerChargeSpeed,
            new LocalizationMetadata("키커 최대 파워 충전 시간 (초)", "Kicker full charge time (seconds)"),
            1.25f, new RoomSettingsRange<float>(0.5f, 2f, 0.25f)),
        new RoomSettingsDefinition<float>(
            ERoomSettingsID.GoalkeeperChargeSpeed,
            new LocalizationMetadata("골키퍼 최대 속력 충전 시간 (초)", "Goalkeeper full charge time (seconds)"),
            1.25f, new RoomSettingsRange<float>(0.5f, 2f, 0.25f)),
        new RoomSettingsDefinition<bool>(
            ERoomSettingsID.EnableItems,
            new LocalizationMetadata("아이템 사용", "Enable items"),
            false), //나중에 true로 변경
        new RoomSettingsDefinition<int>(
            ERoomSettingsID.ItemSelectionTimeLimit,
            new LocalizationMetadata("아이템 개당 선택 제한 시간 (초)", "Item selection time limit (seconds)"),
            15, new RoomSettingsRange<int>(5, 60, 5)),
        new RoomSettingsDefinition<bool>(
            ERoomSettingsID.AllowSpectators,
            new LocalizationMetadata("관전 허용", "Allow spectators"),
            false)
    };

    private static readonly System.Collections.Generic.IReadOnlyList<IRoomSettingsDefinition> sCatalog =
        System.Array.AsReadOnly(sDefinitions);

    static RoomSettingsCatalog()
    {
        if (sDefinitions.Length != System.Enum.GetValues(typeof(ERoomSettingsID)).Length)
        {
            throw new System.InvalidOperationException("중복되거나 누락된 항목이 있습니다.");
        }

        for (int i = 0; i < sDefinitions.Length; i++)
        {
            if ((int)sDefinitions[i].GetID() != i)
            {
                throw new System.InvalidOperationException("설정 항목 목록과 ID 목록이 일치하지 않습니다.");
            }
        }
    }

    public static System.Collections.Generic.IReadOnlyList<IRoomSettingsDefinition> GetAll()
    {
        return sCatalog;
    }

    public static bool TryGet<T>(ERoomSettingsID id, out RoomSettingsDefinition<T> definition)
        where T : struct
    {
        int index = (int)id;

        if (index >= 0 && index < sDefinitions.Length &&
            sDefinitions[index] is RoomSettingsDefinition<T> typed)
        {
            definition = typed;
            return true;
        }

        definition = null;
        return false;
    }

    public static bool TryGet(ERoomSettingsID id, out IRoomSettingsDefinition definition)
    {
        int index = (int)id;

        if (index >= 0 && index < sDefinitions.Length)
        {
            definition = sDefinitions[index];
            return true;
        }

        definition = null;
        return false;
    }
}
