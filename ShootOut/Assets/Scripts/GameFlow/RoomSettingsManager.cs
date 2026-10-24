using System;
using UnityEngine;

public sealed class RoomSettingsManager : MonoBehaviour
{
    [SerializeField]
    private RoomSettings settings = new RoomSettings();

    public RoomSettings Settings => settings;

    public event Action<RoomSettings> OnSettingsChanged;

    public bool SetPlayersPerTeam(int value)
    {
        if (settings.PlayersPerTeam == value) return true;
        else if (!settings.SetPlayersPerTeam(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }

    public bool SetRoundsPerMatch(int value)
    {
        if (settings.RoundsPerMatch == value) return true;
        else if (!settings.SetRoundsPerMatch(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }

    public bool SetMatchesPerGame(int value)
    {
        if (settings.MatchesPerGame == value) return true;
        else if (!settings.SetMatchesPerGame(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }

    public bool SetEnforceFullRounds(bool value)
    {
        if (settings.ShouldEnforceFullRounds == value) return true;
        else if (settings.SetEnforceFullRounds(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }

    public bool SetInputTimeLimitInSeconds(int value)
    {
        if (settings.InputTimeLimitInSeconds == value) return true;
        else if (!settings.SetInputTimeLimitInSeconds(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }

    public bool SetKickerFullChargeInSeconds(float value)
    {
        if (Mathf.Approximately(settings.KickerFullChargeInSeconds, value)) return true;
        else if (!settings.SetKickerFullChargeInSeconds(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }

    public bool SetKeeperFullChargeInSeconds(float value)
    {
        if (Mathf.Approximately(settings.KeeperFullChargeInSeconds, value)) return true;
        else if (!settings.SetKeeperFullChargeInSeconds(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }

    public bool SetItemEnabled(bool value)
    {
        if (settings.IsItemEnabled == value) return true;
        else if (!settings.SetItemEnabled(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }

    public bool SetItemSelectionTimeLimitInSeconds(int value)
    {
        if (settings.ItemSelectionTimeLimitInSeconds == value) return true;
        else if (!settings.SetItemSelectionTimeLimitInSeconds(value)) return false;
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }
    /*
    AI 코드 조각이니 무시하세요
    public void SetPrivate(bool value)
    {
        if (settings.IsPrivate == value)
            return;

        settings.SetPrivate(value);
        OnSettingsChanged?.Invoke(settings);
    }

    public void SetAllowRespawn(bool value)
    {
        if (settings.AllowRespawn == value)
            return;

        settings.SetAllowRespawn(value);
        OnSettingsChanged?.Invoke(settings);
    }

    public void SetGameMode(GameMode value)
    {
        if (settings.Mode == value)
            return;

        if (!Enum.IsDefined(typeof(GameMode), value))
            return;

        settings.SetGameMode(value);
        OnSettingsChanged?.Invoke(settings);
    }
    */
}