using System;
using UnityEngine;

public sealed class RoomSettingsManager : MonoBehaviour
{
    [SerializeField]
    private RoomSettings settings = new RoomSettings();

    public RoomSettings Settings => settings;

    public event Action<RoomSettings> OnSettingsChanged;

    public bool SetMaxPlayers(int value)
    {
        if (settings.PlayersPerTeam == value)
        {
            return false;
        }
        else
        {
            OnSettingsChanged?.Invoke(settings);
            return true;
        }
    }
    /*
    public bool SetMaxPlayers(int value)
    {
        if (settings.MaxPlayers == value)
            return true;

        if (!settings.SetMaxPlayers(value))
            return false;

        OnSettingsChanged?.Invoke(settings);
        return true;
    }

    public bool SetTimeLimit(float value)
    {
        if (settings.TimeLimit == value)
            return true;

        if (!settings.SetTimeLimit(value))
            return false;

        OnSettingsChanged?.Invoke(settings);
        return true;
    }

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