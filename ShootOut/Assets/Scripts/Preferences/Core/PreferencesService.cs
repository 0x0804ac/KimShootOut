using UnityEngine;

//Orchestrates generic scalar values. Complex operations retain focused APIs.
//No network replication: these are personal local preferences.
public sealed class PreferencesService
{
    private readonly PreferencesStore store;
    private string bindingOverridesJson;

    public event System.Action<EPreferencesCategory> PreferencesChanged;

    public PreferencesService(PreferencesData savedData)
    {
        store = new PreferencesStore();
        bindingOverridesJson = string.Empty;
        LoadData(savedData);
    }

    public T Get<T>(EPreferencesID id) where T : struct
    {
        return store.Get<T>(id);
    }

    public bool TrySet<T>(EPreferencesID id, T value) where T : struct
    {
        if (!PreferencesCatalog.TryGet(id, out PreferencesDefinition<T> definition))
        {
            return false;
        }

        //비디오 설정은 TrySetVideo()에서 처리
        if (definition.GetCategory() == EPreferencesCategory.Video)
        {
            return false;
        }

        if (!store.TrySet(id, value, out bool changed))
        {
            return false;
        }

        if (changed)
        {
            PreferencesChanged?.Invoke(definition.GetCategory());
        }

        return true;
    }

    public EPreferencesLanguage GetLanguage()
    {
        return Get<EPreferencesLanguage>(EPreferencesID.Language);
    }

    public EPreferencesWindowMode GetWindowMode()
    {
        return Get<EPreferencesWindowMode>(EPreferencesID.WindowMode);
    }

    public void GetResolution(out int width, out int height)
    {
        width = Get<int>(EPreferencesID.ScreenWidth);
        height = Get<int>(EPreferencesID.ScreenHeight);
    }

    public void GetAudio(EPreferencesAudioChannel channel, out bool enabled, out float volume)
    {
        GetAudioIDs(channel, out EPreferencesID enabledId, out EPreferencesID volumeId);
        enabled = Get<bool>(enabledId);
        volume = Get<float>(volumeId);
    }

    public void GetScreenShake(out bool enabled, out float intensity)
    {
        enabled = Get<bool>(EPreferencesID.ScreenShakeEnabled);
        intensity = Get<float>(EPreferencesID.ScreenShakeIntensity);
    }

    public float GetEffectiveScreenShakeIntensity()
    {
        return Get<bool>(EPreferencesID.ScreenShakeEnabled) ?
            Get<float>(EPreferencesID.ScreenShakeIntensity) : 0f;
    }

    public string GetBindingOverridesJson()
    {
        return bindingOverridesJson;
    }

    public void SetLanguage(EPreferencesLanguage language)
    {
        TrySet(EPreferencesID.Language, language);
    }

    //A multi-value operation is validated before any field changes.
    //The user sees one coherent Video category change event.
    public bool TrySetVideo(EPreferencesWindowMode mode, int width, int height)
    {
        if (!store.CanSet(EPreferencesID.WindowMode, mode) ||
            !store.CanSet(EPreferencesID.ScreenWidth, width) ||
            !store.CanSet(EPreferencesID.ScreenHeight, height))
        {
            return false;
        }

        bool modeChanged;
        bool widthChanged;
        bool heightChanged;
        store.TrySet(EPreferencesID.WindowMode, mode, out modeChanged);
        store.TrySet(EPreferencesID.ScreenWidth, width, out widthChanged);
        store.TrySet(EPreferencesID.ScreenHeight, height, out heightChanged);

        if (modeChanged || widthChanged || heightChanged)
        {
            PreferencesChanged?.Invoke(EPreferencesCategory.Video);
        }

        return true;
    }

    public void SetAudioEnabled(EPreferencesAudioChannel channel, bool enabled)
    {
        GetAudioIDs(channel, out EPreferencesID enabledId, out _);
        TrySet(enabledId, enabled);
    }

    public void SetAudioVolume(EPreferencesAudioChannel channel, float volume)
    {
        if (!IsFinite(volume))
        {
            return;
        }

        GetAudioIDs(channel, out _, out EPreferencesID volumeId);
        TrySet(volumeId, Mathf.Clamp01(volume));
    }

    public void SetScreenShakeEnabled(bool enabled)
    {
        TrySet(EPreferencesID.ScreenShakeEnabled, enabled);
    }

    public void SetScreenShakeIntensity(float intensity)
    {
        if (!IsFinite(intensity))
        {
            return;
        }

        TrySet(EPreferencesID.ScreenShakeIntensity, Mathf.Clamp01(intensity));
    }

    //JSON is a specialized Input System serialization, not a generic setting.
    public void SetBindingOverridesJson(string json)
    {
        string updated = json ?? string.Empty;
        if (bindingOverridesJson == updated)
        {
            return;
        }

        bindingOverridesJson = updated;
        PreferencesChanged?.Invoke(EPreferencesCategory.Controls);
    }

    //An explicit, stable DTO keeps JsonUtility compatibility and file schema.
    //Conversion happens only when saving, not on every edit.
    public PreferencesData CreateSaveData()
    {
        GetResolution(out int width, out int height);
        GetAudio(EPreferencesAudioChannel.Music,
            out bool musicEnabled,
            out float musicVolume);
        GetAudio(EPreferencesAudioChannel.Effects,
            out bool effectsEnabled,
            out float effectsVolume);
        GetScreenShake(out bool shakeEnabled, out float shakeIntensity);

        return new PreferencesData
        {
            version = 1,
            language = GetLanguage(),
            windowMode = GetWindowMode(),
            screenWidth = width,
            screenHeight = height,
            music = new PreferencesAudioData
            {
                enabled = musicEnabled,
                volume = musicVolume
            },
            effects = new PreferencesAudioData
            {
                enabled = effectsEnabled,
                volume = effectsVolume
            },
            screenShakeEnabled = shakeEnabled,
            screenShakeIntensity = shakeIntensity,
            bindingOverridesJson = bindingOverridesJson
        };
    }

    private void LoadData(PreferencesData data)
    {
        if (data == null)
        {
            return;
        }

        //Invalid enum values retain catalog defaults.
        store.TrySet(EPreferencesID.Language,
            data.language, out _);
        store.TrySet(EPreferencesID.WindowMode,
            data.windowMode, out _);
        store.TrySet(EPreferencesID.ScreenWidth,
            Mathf.Max(640, data.screenWidth), out _);
        store.TrySet(EPreferencesID.ScreenHeight,
            Mathf.Max(480, data.screenHeight), out _);
        PreferencesAudioData music = data.music ?? new PreferencesAudioData();
        PreferencesAudioData effects = data.effects ?? new PreferencesAudioData();
        store.TrySet(EPreferencesID.MusicEnabled,
            music.enabled, out _);
        store.TrySet(EPreferencesID.EffectsEnabled,
            effects.enabled, out _);
        store.TrySet(EPreferencesID.ScreenShakeEnabled,
            data.screenShakeEnabled, out _);
        if (IsFinite(music.volume))
        {
            store.TrySet(EPreferencesID.MusicVolume,
                Mathf.Clamp01(music.volume), out _);
        }
        if (IsFinite(effects.volume))
        {
            store.TrySet(EPreferencesID.EffectsVolume,
                Mathf.Clamp01(effects.volume), out _);
        }
        if (IsFinite(data.screenShakeIntensity))
        {
            store.TrySet(EPreferencesID.ScreenShakeIntensity,
                Mathf.Clamp01(data.screenShakeIntensity), out _);
        }
        bindingOverridesJson = data.bindingOverridesJson ?? string.Empty;
    }

    private static void GetAudioIDs(
        EPreferencesAudioChannel channel,
        out EPreferencesID enabledId,
        out EPreferencesID volumeId)
    {
        switch (channel)
        {
            case EPreferencesAudioChannel.Music:
                enabledId = EPreferencesID.MusicEnabled;
                volumeId = EPreferencesID.MusicVolume;
                return;
            case EPreferencesAudioChannel.Effects:
                enabledId = EPreferencesID.EffectsEnabled;
                volumeId = EPreferencesID.EffectsVolume;
                return;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(channel));
        }
    }

    private static bool IsFinite(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
