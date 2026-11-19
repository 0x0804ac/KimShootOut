using System;
using UnityEngine;

// Shared display language for the preferences UI; other UI can subscribe too.
public static class GamePreferencesLocalization
{
    private static EPreferencesLanguage sLanguage = EPreferencesLanguage.Korean;

    public static event Action LanguageChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        sLanguage = EPreferencesLanguage.Korean;
        LanguageChanged = null;
    }

    public static EPreferencesLanguage GetLanguage()
    {
        return sLanguage;
    }

    public static void SetLanguage(EPreferencesLanguage language)
    {
        if (sLanguage == language)
        {
            return;
        }

        sLanguage = language;
        LanguageChanged?.Invoke();
    }

    public static string GetWindowModeName(EPreferencesWindowMode mode)
    {
        bool korean = sLanguage == EPreferencesLanguage.Korean;

        switch (mode)
        {
            case EPreferencesWindowMode.Windowed:
                return korean ? "창 모드" : "Windowed";
            case EPreferencesWindowMode.Fullscreen:
                return korean ? "전체 화면" : "Fullscreen";
            case EPreferencesWindowMode.Borderless:
                return korean ? "테두리 없는 창" : "Borderless";
            default:
                return mode.ToString();
        }
    }

    public static string GetPercent(float value)
    {
        return Math.Round(value * 100f).ToString("0") + "%";
    }
}
