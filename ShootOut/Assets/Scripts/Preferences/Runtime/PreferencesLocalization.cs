using UnityEngine;

//Shared display language for the preferences UI; other UI can subscribe too.
public static class PreferencesLocalization
{
    private static EPreferencesLanguage sLanguage = EPreferencesLanguage.Korean;

    public static event System.Action<EPreferencesLanguage> LanguageChanged;

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
        LanguageChanged?.Invoke(language);
    }

    public static string GetWindowModeName(EPreferencesWindowMode mode)
    {
        bool isKorean = sLanguage == EPreferencesLanguage.Korean;

        switch (mode)
        {
            case EPreferencesWindowMode.Windowed:
                return isKorean ? "창 모드" : "Windowed";
            case EPreferencesWindowMode.Fullscreen:
                return isKorean ? "전체 화면" : "Fullscreen";
            case EPreferencesWindowMode.Borderless:
                return isKorean ? "테두리 없는 창" : "Borderless";
            default:
                return mode.ToString();
        }
    }

    public static string GetPercent(float value)
    {
        return Mathf.RoundToInt(value * 100f).ToString("0") + "%";
    }
}
