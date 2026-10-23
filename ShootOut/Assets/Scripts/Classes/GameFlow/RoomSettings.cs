using System;
using UnityEngine;

//방 설정 정보가 담겨있는 클래스
[Serializable]
public class RoomSettings
{
    public const int MinPlayersPerTeam = 1;
    public const int MaxPlayersPerTeam = 5;
    public const int MinRoundsPerMatch = 5;
    public const int MaxRoundsPerMatch = 10;
    public const int MinMatchesPerGame = 1;
    public const int MaxMatchesPerGame = 5;
    public const int MinInputTimeLimitInSeconds = 5;
    public const int MaxInputTimeLimitInSeconds = 30;
    public const float MinKickerFullChargeInSeconds = 0.5f;
    public const float MaxKickerFullChargeInSeconds = 2.0f;
    public const float MinKeeperFullChargeInSeconds = 0.5f;
    public const float MaxKeeperFullChargeInSeconds = 2.0f;
    public const int MinItemSelectionTimeLimitInSeconds = 5;
    public const int MaxItemSelectionTimeLimitInSeconds = 60;

    [SerializeField, Range(MinPlayersPerTeam, MaxPlayersPerTeam)]
    private int playersPerTeam; //팀 당 최대 플레이어 수
    [SerializeField, Range(MinRoundsPerMatch, MaxRoundsPerMatch)]
    private int roundsPerMatch; //매치 당 라운드 수
    [SerializeField, Range(MinMatchesPerGame, MaxMatchesPerGame)]
    private int matchesPerGame; //게임 당 매치 수

    [SerializeField]
    private bool shouldEnforceFullRounds; //승패가 결정된 상황에서도 마지막 라운드까지 진행할지 여부
    [SerializeField, Range(MinInputTimeLimitInSeconds, MaxInputTimeLimitInSeconds)]
    private int inputTimeLimitInSeconds; //플레이어 조작 제한 시간
    [SerializeField, Range(MinKickerFullChargeInSeconds, MaxKickerFullChargeInSeconds)]
    private float kickerFullChargeInSeconds; //키커 파워 조작 시 최대 충전까지 걸리는 시간
    [SerializeField, Range(MinKeeperFullChargeInSeconds, MaxKeeperFullChargeInSeconds)]
    private float keeperFullChargeInSeconds; //골키퍼 속력 조작 시 최대 충전까지 걸리는 시간

    [SerializeField]
    private bool isItemEnabled; //아이템 사용 여부
    [SerializeField, Range(MinItemSelectionTimeLimitInSeconds, MaxItemSelectionTimeLimitInSeconds)]
    private int itemSelectionTimeLimitInSeconds; //아이템 개당 선택 제한 시간

    private bool canEditValues; //설정 값 수정 가능 여부 (경기 시작 여부)

    public int PlayersPerTeam => playersPerTeam;
    public int RoundsPerMatch => roundsPerMatch;
    public int MatchesPerGame => matchesPerGame;
    public bool ShouldEnforceFullRounds => shouldEnforceFullRounds;
    public int InputTimeLimitInSeconds => inputTimeLimitInSeconds;
    public float KickerFullChargeInSeconds => kickerFullChargeInSeconds;
    public float KeeperFullChargeInSeconds => keeperFullChargeInSeconds;
    public bool IsItemEnabled => isItemEnabled;
    public int ItemSelectionTimeLimitInSeconds => itemSelectionTimeLimitInSeconds;

    public RoomSettings()
    {
        ApplyDefaultSettings();
    }

    private void ApplyDefaultSettings()
    {
        playersPerTeam = 1;
        roundsPerMatch = 5;
        matchesPerGame = 1;
        shouldEnforceFullRounds = false;
        inputTimeLimitInSeconds = 10;
        kickerFullChargeInSeconds = 1.25f;
        keeperFullChargeInSeconds = 1.25f;
        isItemEnabled = false;
        itemSelectionTimeLimitInSeconds = 10;
        canEditValues = true;
    }

    public void DisableEditing()
    {
        canEditValues = false;
    }

    internal void SetPlayersPerTeam(int value)
    {
        if (canEditValues) playersPerTeam = Mathf.Clamp(value, MinPlayersPerTeam, MaxPlayersPerTeam);
    }

    internal void SetRoundsPerMatch(int value)
    {
        if (canEditValues) roundsPerMatch = Mathf.Clamp(value, MinRoundsPerMatch, MaxRoundsPerMatch);
    }

    internal void SetMatchesPerGame(int value)
    {
        if (canEditValues) matchesPerGame = Mathf.Clamp(value, MinMatchesPerGame, MaxMatchesPerGame);
    }

    internal void SetShouldEnforceFullRounds(bool value)
    {
        if (canEditValues) shouldEnforceFullRounds = value;
    }

    internal void SetInputTimeLimitInSeconds(int value)
    {
        if (canEditValues) inputTimeLimitInSeconds = Mathf.Clamp(value, MinInputTimeLimitInSeconds, MaxInputTimeLimitInSeconds);
    }

    internal void SetKickerFullChargeInSeconds(float value)
    {
        if (canEditValues) kickerFullChargeInSeconds = Mathf.Clamp(value, MinKickerFullChargeInSeconds, MaxKickerFullChargeInSeconds);
    }

    internal void SetKeeperFullChargeInSeconds(float value)
    {
        if (canEditValues) keeperFullChargeInSeconds = Mathf.Clamp(value, MinKeeperFullChargeInSeconds, MaxKeeperFullChargeInSeconds);
    }

    internal void SetIsItemEnabled(bool value)
    {
        if (canEditValues) isItemEnabled = value;
    }

    internal void SetItemSelectionTimeLimitInSeconds(int value)
    {
        if (canEditValues && isItemEnabled) itemSelectionTimeLimitInSeconds = Mathf.Clamp(value, MinItemSelectionTimeLimitInSeconds, MaxItemSelectionTimeLimitInSeconds);
    }
}
