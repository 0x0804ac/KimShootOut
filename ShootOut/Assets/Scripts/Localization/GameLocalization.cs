using UnityEngine;

public static class GameLocalization
{
    private static EDisplayLanguage sLanguage = EDisplayLanguage.Korean;
    //private static readonly var sKoreanCulture = System.Globalization.CultureInfo.GetCultureInfo("ko-KR");
    //private static readonly var sEnglishCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

    public static event System.Action LanguageChanged;

    public static EDisplayLanguage GetLanguage()
    {
        return sLanguage;
    }

    public static void SetLanguage(EDisplayLanguage newLanguage)
    {
        if (sLanguage == newLanguage)
        {
            return;
        }
        Debug.Log($"언어 설정 변경: {sLanguage} -> {newLanguage}");
        sLanguage = newLanguage;
        LanguageChanged?.Invoke();
    }
    /*
    public static string FormatNumber(RoomSettingsMetadata metadata, double value)
    {
        var culture = language == RoomLanguage.Korean ? sKoreanCulture : sEnglishCulture;
        string unit = metadata.GetUnit(language);
        string number = value.ToString("0.##", culture);
        return string.IsNullOrEmpty(unit) ? number : number + " " + unit;
    }
    */
    public static string FormatToggle(bool enabled)
    {
        if (sLanguage == EDisplayLanguage.Korean) return enabled ? "켜짐" : "꺼짐";
        return enabled ? "On" : "Off";
    }

    public static void Reset()
    {
        sLanguage = EDisplayLanguage.Korean;
        LanguageChanged = null;
        Debug.Log("언어 설정이 초기화되었습니다.");
    }
}
