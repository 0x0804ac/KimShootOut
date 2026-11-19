public static class PreferencesCatalog
{
    private static readonly IPreferencesDefinition[] sDefinitions =
    {
        new PreferencesDefinition<EPreferencesLanguage>(
            EPreferencesID.Language, EPreferencesCategory.Language,
            EPreferencesLanguage.Korean,
            value => value == EPreferencesLanguage.Korean || value == EPreferencesLanguage.English,
            "언어", "Language"),
        new PreferencesDefinition<EPreferencesWindowMode>(
            EPreferencesID.WindowMode, EPreferencesCategory.Video,
            EPreferencesWindowMode.Borderless,
            value => value == EPreferencesWindowMode.Windowed ||
                value == EPreferencesWindowMode.Fullscreen ||
                value == EPreferencesWindowMode.Borderless,
            "화면 모드", "Display mode"),
        new PreferencesDefinition<int>(
            EPreferencesID.ScreenWidth, EPreferencesCategory.Video, 1280,
            value => value >= 640, "화면 너비", "Screen width"),
        new PreferencesDefinition<int>(
            EPreferencesID.ScreenHeight, EPreferencesCategory.Video, 720,
            value => value >= 480, "화면 높이", "Screen height"),
        new PreferencesDefinition<bool>(
            EPreferencesID.MusicEnabled, EPreferencesCategory.Audio, true,
            value => true, "배경 음악 사용", "Music enabled"),
        new PreferencesDefinition<float>(
            EPreferencesID.MusicVolume, EPreferencesCategory.Audio, 1f,
            IsNormalized, "배경 음악 음량", "Music volume"),
        new PreferencesDefinition<bool>(
            EPreferencesID.EffectsEnabled, EPreferencesCategory.Audio, true,
            value => true, "효과음 사용", "Sound effects enabled"),
        new PreferencesDefinition<float>(
            EPreferencesID.EffectsVolume, EPreferencesCategory.Audio, 1f,
            IsNormalized, "효과음 음량", "Sound effects volume"),
        new PreferencesDefinition<bool>(
            EPreferencesID.ScreenShakeEnabled, EPreferencesCategory.Accessibility, true,
            value => true, "화면 흔들림 사용", "Screen shake enabled"),
        new PreferencesDefinition<float>(
            EPreferencesID.ScreenShakeIntensity, EPreferencesCategory.Accessibility, 1f,
            IsNormalized, "흔들림 강도", "Shake intensity")
    };

    private static readonly System.Collections.Generic.IReadOnlyList<IPreferencesDefinition> sReadOnlyDefinitions =
        System.Array.AsReadOnly(sDefinitions);

    static PreferencesCatalog()
    {
        if (sDefinitions.Length != System.Enum.GetValues(typeof(EPreferencesID)).Length)
        {
            throw new System.InvalidOperationException("중복되거나 누락된 항목이 있습니다.");
        }

        for (int i = 0; i < sDefinitions.Length; i++)
        {
            if ((int)sDefinitions[i].GetID() != i)
            {
                throw new System.InvalidOperationException("개인 설정 항목 목록과 ID 목록이 일치하지 않습니다.");
            }
        }
    }

    public static System.Collections.Generic.IReadOnlyList<IPreferencesDefinition> GetAll()
    {
        return sReadOnlyDefinitions;
    }

    public static bool TryGet(EPreferencesID id, out IPreferencesDefinition definition)
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

    public static bool TryGet<T>(EPreferencesID id, out PreferencesDefinition<T> definition)
        where T : struct
    {
        if (TryGet(id, out IPreferencesDefinition raw) &&
            raw is PreferencesDefinition<T> typed)
        {
            definition = typed;
            return true;
        }

        definition = null;
        return false;
    }

    private static bool IsNormalized(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value) &&
            value >= 0f && value <= 1f;
    }
}
