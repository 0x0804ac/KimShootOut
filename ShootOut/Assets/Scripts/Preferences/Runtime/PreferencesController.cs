using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

//Scene owner for applying preferences to Unity APIs. Service and storage are ordinary C# classes.
public sealed class PreferencesController : MonoBehaviour
{
    private const string MUSIC_VOLUME_PARAMETER = "MusicVolumeDb";
    private const string EFFECTS_VOLUME_PARAMETER = "EffectsVolumeDb";
    private const float MUTE_DECIBELS = -80f;

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private PreferencesScreenShakePreview screenShakePreview;

    private PreferencesStorage storage;
    private PreferencesService service;
    private PreferencesBindings bindings;
    private bool hasStarted;

    public event System.Action<EPreferencesLanguage> LanguageApplied;

    private void Awake()
    {
        storage = new PreferencesStorage();
        service = new PreferencesService(storage.Load());

        if (playerInput != null)
        {
            bindings = new PreferencesBindings(playerInput.actions, service);
        }

        service.PreferencesChanged += HandlePreferencesChanged;
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
            service.PreferencesChanged -= HandlePreferencesChanged;
        }

        bindings?.Dispose();
    }

    public PreferencesService GetService()
    {
        return service;
    }

    public PreferencesBindings GetBindings()
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
                Debug.LogWarning("전체 화면 모드를 지원하지 않는 환경이므로 테두리 없는 창 모드로 전환합니다.");
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

        SetChannelVolume(EPreferencesAudioChannel.Music, MUSIC_VOLUME_PARAMETER);
        SetChannelVolume(EPreferencesAudioChannel.Effects, EFFECTS_VOLUME_PARAMETER);
    }

    public void PreviewScreenShake()
    {
        if (screenShakePreview != null && service != null)
        {
            screenShakePreview.Play(service.GetEffectiveScreenShakeIntensity());
        }
    }

    private void HandlePreferencesChanged(EPreferencesCategory category)
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
                //비디오 설정은 UI의 적용 버튼을 눌러야 적용됨
                break;
            case EPreferencesCategory.Controls:
            case EPreferencesCategory.Accessibility:
                break;
        }
    }

    public void ApplyLanguage()
    {
        if (service == null)
        {
            return;
        }

        EPreferencesLanguage language = service.GetLanguage();
        PreferencesLocalization.SetLanguage(language);
        LanguageApplied?.Invoke(language);
    }

    private void SetChannelVolume(EPreferencesAudioChannel channel, string exposedName)
    {
        service.GetAudio(channel, out bool enabled, out float volume);
        float db = enabled && volume > 0f ?
            Mathf.Max(MUTE_DECIBELS, 20f * Mathf.Log10(volume)) : MUTE_DECIBELS;
        if (!audioMixer.SetFloat(exposedName, db))
        {
            Debug.LogWarning($"AudioMixer에 해당 인자가 없습니다: {exposedName}");
        }
    }
}
