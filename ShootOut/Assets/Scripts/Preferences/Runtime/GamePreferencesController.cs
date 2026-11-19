using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

// Scene owner for applying preferences to Unity APIs. Service and storage are ordinary C# classes.
public sealed class GamePreferencesController : MonoBehaviour
{
    private const string musicVolumeParameter = "MusicVolumeDb";
    private const string effectsVolumeParameter = "EffectsVolumeDb";
    private const float muteDecibels = -80f;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GamePreferencesShakePreview shakePreview;

    private GamePreferencesStorage storage;
    private GamePreferencesService service;
    private GamePreferencesBindings bindings;
    private bool hasStarted;

    public event Action<EPreferencesLanguage> LanguageApplied;

    private void Awake()
    {
        storage = new GamePreferencesStorage();
        service = new GamePreferencesService(storage.Load());

        if (playerInput != null)
        {
            bindings = new GamePreferencesBindings(playerInput.actions, service);
        }

        service.PreferencesChanged += HandleSettingsChanged;
    }

    private void Start()
    {
        hasStarted = true;
        ApplyAll();
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnDestroy()
    {
        if (service != null)
        {
            service.PreferencesChanged -= HandleSettingsChanged;
        }

        bindings?.Dispose();
    }

    public GamePreferencesService GetService()
    {
        return service;
    }

    public GamePreferencesBindings GetBindings()
    {
        return bindings;
    }

    public bool Save()
    {
        return storage != null && service != null && storage.Save(service.CreateSaveData());
    }

    public void ApplyAll()
    {
        ApplyLanguage();
        ApplyVideo();
        ApplyAudio();
    }

    public void ApplyVideo()
    {
        if (service == null)
        {
            return;
        }

        EPreferencesWindowMode selected = service.GetWindowMode();
        service.GetResolution(out int width, out int height);
        FullScreenMode unityMode = FullScreenMode.Windowed;

        switch (selected)
        {
            case EPreferencesWindowMode.Windowed:
                unityMode = FullScreenMode.Windowed;
                break;
            case EPreferencesWindowMode.Borderless:
                unityMode = FullScreenMode.FullScreenWindow;
                width = Screen.currentResolution.width;
                height = Screen.currentResolution.height;
                break;
            case EPreferencesWindowMode.Fullscreen:
#if UNITY_STANDALONE_WIN
                unityMode = FullScreenMode.ExclusiveFullScreen;
#else
                unityMode = FullScreenMode.FullScreenWindow;
                width = Screen.currentResolution.width;
                height = Screen.currentResolution.height;
                Debug.LogWarning("Exclusive fullscreen is Windows-only; using borderless fullscreen.");
#endif
                break;
        }

        Screen.SetResolution(width, height, unityMode);
    }

    public void ApplyAudio()
    {
        if (audioMixer == null || service == null)
        {
            return;
        }

        SetChannelVolume(EPreferencesAudioChannel.Music, musicVolumeParameter);
        SetChannelVolume(EPreferencesAudioChannel.Effects, effectsVolumeParameter);
    }

    public void PreviewShake()
    {
        if (shakePreview != null && service != null)
        {
            shakePreview.Play(service.GetEffectiveShakeIntensity());
        }
    }

    private void HandleSettingsChanged(EPreferencesCategory category)
    {
        if (!hasStarted)
        {
            return;
        }

        switch (category)
        {
            case EPreferencesCategory.Language:
                ApplyLanguage();
                break;
            case EPreferencesCategory.Audio:
                ApplyAudio();
                break;
            case EPreferencesCategory.Video:
                // Video changes are explicitly applied from the UI Apply button.
                break;
            case EPreferencesCategory.Controls:
            case EPreferencesCategory.Accessibility:
                break;
        }
    }

    private void ApplyLanguage()
    {
        if (service == null)
        {
            return;
        }

        EPreferencesLanguage language = service.GetLanguage();
        GamePreferencesLocalization.SetLanguage(language);
        LanguageApplied?.Invoke(language);
    }

    private void SetChannelVolume(EPreferencesAudioChannel channel, string exposedName)
    {
        service.GetAudio(channel, out bool enabled, out float volume);
        float db = enabled && volume > 0f ? Mathf.Max(muteDecibels, 20f * Mathf.Log10(volume)) : muteDecibels;

        if (!audioMixer.SetFloat(exposedName, db))
        {
            Debug.LogWarning($"AudioMixer missing exposed parameter: {exposedName}");
        }
    }
}
