using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public sealed class GamePreferencesAudioView
{
    public Toggle enabledToggle;
    public Slider volumeSlider;
    public TMP_Text volumeText;
}

[Serializable]
public sealed class GamePreferencesBindingView
{
    public string actionName;
    public int bindingIndex;
    public Button changeButton;
    public TMP_Text bindingText;
}

[Serializable]
public sealed class GamePreferencesLocalizedLabel
{
    public EPreferencesTextKey key;
    public TMP_Text label;
}

public sealed class GamePreferencesUI : MonoBehaviour
{
    [SerializeField] private GamePreferencesController controller;
    [SerializeField] private Button koreanButton;
    [SerializeField] private Button englishButton;
    [SerializeField] private TMP_Text currentLanguageText;
    [SerializeField] private TMP_Dropdown windowModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button applyVideoButton;
    [SerializeField] private GamePreferencesAudioView music;
    [SerializeField] private GamePreferencesAudioView effects;
    [SerializeField] private Toggle shakeEnabledToggle;
    [SerializeField] private Slider shakeIntensitySlider;
    [SerializeField] private TMP_Text shakeIntensityText;
    [SerializeField] private Button shakeTestButton;
    [SerializeField] private GamePreferencesBindingView[] bindingViews;
    [SerializeField] private Button resetBindingsButton;
    [SerializeField] private TMP_Text rebindingStatusText;
    [SerializeField] private Button saveButton;
    [SerializeField] private GamePreferencesLocalizedLabel[] localizedLabels;

    private readonly List<Action> detachListeners = new List<Action>();
    private readonly List<Vector2Int> resolutions = new List<Vector2Int>();
    private GamePreferencesService service;
    private GamePreferencesBindings bindings;
    private bool initialized;
    private bool listening;

    private void OnEnable()
    {
        if (initialized)
        {
            Subscribe();
            RefreshAll();
        }
    }

    private void Start()
    {
        if (controller == null)
        {
            Debug.LogError("GamePreferencesUI needs a GamePreferencesController.", this);
            enabled = false;
            return;
        }

        service = controller.GetService();
        bindings = controller.GetBindings();
        BuildResolutions();
        BuildListeners();
        initialized = true;
        Subscribe();
        RefreshAll();
    }

    private void OnDisable()
    {
        Unsubscribe();
        bindings?.CancelRebind();
    }

    private void OnDestroy()
    {
        Unsubscribe();

        foreach (Action detach in detachListeners)
        {
            detach();
        }

        detachListeners.Clear();
    }

    private void Subscribe()
    {
        service.PreferencesChanged -= OnPreferencesChanged;
        service.PreferencesChanged += OnPreferencesChanged;
        GamePreferencesLocalization.LanguageChanged -= RefreshLanguage;
        GamePreferencesLocalization.LanguageChanged += RefreshLanguage;

        if (bindings != null)
        {
            bindings.BindingsChanged -= RefreshBindings;
            bindings.BindingsChanged += RefreshBindings;
            bindings.ListeningChanged -= OnListeningChanged;
            bindings.ListeningChanged += OnListeningChanged;
            bindings.RebindRejected -= OnRebindRejected;
            bindings.RebindRejected += OnRebindRejected;
        }
    }

    private void Unsubscribe()
    {
        if (service != null)
        {
            service.PreferencesChanged -= OnPreferencesChanged;
        }

        GamePreferencesLocalization.LanguageChanged -= RefreshLanguage;

        if (bindings != null)
        {
            bindings.BindingsChanged -= RefreshBindings;
            bindings.ListeningChanged -= OnListeningChanged;
            bindings.RebindRejected -= OnRebindRejected;
        }
    }

    private void BuildListeners()
    {
        AttachButton(koreanButton, () => service.SetLanguage(EPreferencesLanguage.Korean));
        AttachButton(englishButton, () => service.SetLanguage(EPreferencesLanguage.English));
        AttachButton(applyVideoButton, ApplyVideo);
        AttachButton(shakeTestButton, controller.PreviewShake);
        AttachButton(resetBindingsButton, () => bindings?.ResetAllBindings());
        AttachButton(saveButton, () => controller.Save());
        AttachAudio(music, EPreferencesAudioChannel.Music);
        AttachAudio(effects, EPreferencesAudioChannel.Effects);

        if (windowModeDropdown != null)
        {
            UnityAction<int> handler = index => RefreshVideoModeState();
            windowModeDropdown.onValueChanged.AddListener(handler);
            detachListeners.Add(() => windowModeDropdown.onValueChanged.RemoveListener(handler));
        }

        if (shakeEnabledToggle != null)
        {
            UnityAction<bool> handler = service.SetShakeEnabled;
            shakeEnabledToggle.onValueChanged.AddListener(handler);
            detachListeners.Add(() => shakeEnabledToggle.onValueChanged.RemoveListener(handler));
        }

        if (shakeIntensitySlider != null)
        {
            shakeIntensitySlider.minValue = 0f;
            shakeIntensitySlider.maxValue = 1f;
            UnityAction<float> handler = service.SetShakeIntensity;
            shakeIntensitySlider.onValueChanged.AddListener(handler);
            detachListeners.Add(() => shakeIntensitySlider.onValueChanged.RemoveListener(handler));
        }

        if (bindingViews != null)
        {
            foreach (GamePreferencesBindingView view in bindingViews)
            {
                if (view == null || view.changeButton == null)
                {
                    continue;
                }

                GamePreferencesBindingView bound = view;
                AttachButton(bound.changeButton, () =>
                {
                    if (bindings != null &&
                        !bindings.TryStartRebind(bound.actionName, bound.bindingIndex))
                    {
                        OnRebindRejected("Unable to rebind the selected action.");
                    }
                });
            }
        }
    }

    private void AttachButton(Button button, UnityAction handler)
    {
        if (button == null)
        {
            return;
        }

        button.onClick.AddListener(handler);
        detachListeners.Add(() => button.onClick.RemoveListener(handler));
    }

    private void AttachAudio(GamePreferencesAudioView view, EPreferencesAudioChannel channel)
    {
        if (view == null)
        {
            return;
        }

        if (view.enabledToggle != null)
        {
            UnityAction<bool> handler = enabled => service.SetAudioEnabled(channel, enabled);
            view.enabledToggle.onValueChanged.AddListener(handler);
            detachListeners.Add(() => view.enabledToggle.onValueChanged.RemoveListener(handler));
        }

        if (view.volumeSlider != null)
        {
            view.volumeSlider.minValue = 0f;
            view.volumeSlider.maxValue = 1f;
            UnityAction<float> handler = value => service.SetAudioVolume(channel, value);
            view.volumeSlider.onValueChanged.AddListener(handler);
            detachListeners.Add(() => view.volumeSlider.onValueChanged.RemoveListener(handler));
        }
    }

    private void BuildResolutions()
    {
        resolutions.Clear();
        Resolution[] available = Screen.resolutions;

        foreach (Resolution option in available)
        {
            if (option.width < 640 || option.height < 480)
            {
                continue;
            }

            Vector2Int size = new Vector2Int(option.width, option.height);

            if (!resolutions.Contains(size))
            {
                resolutions.Add(size);
            }
        }

        service.GetResolution(out int savedWidth, out int savedHeight);
        Vector2Int saved = new Vector2Int(savedWidth, savedHeight);

        if (!resolutions.Contains(saved))
        {
            resolutions.Add(saved);
        }

        resolutions.Sort((left, right) =>
        {
            int widthComparison = left.x.CompareTo(right.x);
            return widthComparison != 0 ? widthComparison : left.y.CompareTo(right.y);
        });

        if (resolutionDropdown != null)
        {
            var options = new List<TMP_Dropdown.OptionData>();

            foreach (Vector2Int size in resolutions)
            {
                options.Add(new TMP_Dropdown.OptionData($"{size.x} × {size.y}"));
            }

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);
        }
    }

    private void ApplyVideo()
    {
        if (windowModeDropdown == null || resolutionDropdown == null || resolutions.Count == 0)
        {
            return;
        }

        EPreferencesWindowMode mode = (EPreferencesWindowMode)windowModeDropdown.value;
        Vector2Int selected = resolutions[Mathf.Clamp(resolutionDropdown.value, 0, resolutions.Count - 1)];

        if (service.TrySetVideo(mode, selected.x, selected.y))
        {
            controller.ApplyVideo();
        }
    }

    private void OnPreferencesChanged(EPreferencesCategory category)
    {
        switch (category)
        {
            case EPreferencesCategory.Language:
                RefreshLanguage();
                break;
            case EPreferencesCategory.Video:
                RefreshVideo();
                break;
            case EPreferencesCategory.Audio:
                RefreshAudio();
                break;
            case EPreferencesCategory.Accessibility:
                RefreshShake();
                break;
            case EPreferencesCategory.Controls:
                RefreshBindings();
                break;
        }
    }

    private void RefreshAll()
    {
        RefreshLanguage();
        RefreshVideo();
        RefreshAudio();
        RefreshShake();
        RefreshBindings();
    }

    private void RefreshLanguage()
    {
        if (service == null)
        {
            return;
        }

        EPreferencesLanguage language = service.GetLanguage();

        if (currentLanguageText != null)
        {
            currentLanguageText.text = language == EPreferencesLanguage.Korean ? "한국어" : "English";
        }

        if (localizedLabels != null)
        {
            foreach (GamePreferencesLocalizedLabel item in localizedLabels)
            {
                if (item != null && item.label != null)
                {
                    item.label.text = GamePreferencesText.Get(item.key);
                }
            }
        }

        if (windowModeDropdown != null)
        {
            int selection = windowModeDropdown.value;
            var options = new List<TMP_Dropdown.OptionData>();

            for (int i = 0; i < 3; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(
                    GamePreferencesLocalization.GetWindowModeName((EPreferencesWindowMode)i)));
            }

            windowModeDropdown.ClearOptions();
            windowModeDropdown.AddOptions(options);
            windowModeDropdown.SetValueWithoutNotify(selection);
            windowModeDropdown.RefreshShownValue();
        }

        if (rebindingStatusText != null && listening)
        {
            rebindingStatusText.text = GamePreferencesText.Get(EPreferencesTextKey.Rebinding);
        }
    }

    private void RefreshVideo()
    {
        if (windowModeDropdown != null)
        {
            windowModeDropdown.SetValueWithoutNotify((int)service.GetWindowMode());
        }

        if (resolutionDropdown != null)
        {
            service.GetResolution(out int width, out int height);
            int index = resolutions.IndexOf(new Vector2Int(width, height));
            resolutionDropdown.SetValueWithoutNotify(Mathf.Max(0, index));
        }

        RefreshVideoModeState();
    }

    private void RefreshVideoModeState()
    {
        if (resolutionDropdown != null && windowModeDropdown != null)
        {
            resolutionDropdown.interactable =
                (EPreferencesWindowMode)windowModeDropdown.value != EPreferencesWindowMode.Borderless;
        }
    }

    private void RefreshAudio()
    {
        RefreshAudioView(music, EPreferencesAudioChannel.Music);
        RefreshAudioView(effects, EPreferencesAudioChannel.Effects);
    }

    private void RefreshAudioView(GamePreferencesAudioView view, EPreferencesAudioChannel channel)
    {
        if (view == null)
        {
            return;
        }

        service.GetAudio(channel, out bool enabled, out float volume);

        if (view.enabledToggle != null)
        {
            view.enabledToggle.SetIsOnWithoutNotify(enabled);
        }

        if (view.volumeSlider != null)
        {
            view.volumeSlider.SetValueWithoutNotify(volume);
        }

        if (view.volumeText != null)
        {
            view.volumeText.text = GamePreferencesLocalization.GetPercent(volume);
        }
    }

    private void RefreshShake()
    {
        service.GetShake(out bool enabled, out float intensity);

        if (shakeEnabledToggle != null)
        {
            shakeEnabledToggle.SetIsOnWithoutNotify(enabled);
        }

        if (shakeIntensitySlider != null)
        {
            shakeIntensitySlider.SetValueWithoutNotify(intensity);
        }

        if (shakeIntensityText != null)
        {
            shakeIntensityText.text = GamePreferencesLocalization.GetPercent(intensity);
        }
    }

    private void RefreshBindings()
    {
        if (bindingViews == null)
        {
            return;
        }

        foreach (GamePreferencesBindingView view in bindingViews)
        {
            if (view == null)
            {
                continue;
            }

            if (view.bindingText != null)
            {
                view.bindingText.text = bindings == null ? "—" :
                    bindings.GetBindingDisplay(view.actionName, view.bindingIndex);
            }

            if (view.changeButton != null)
            {
                view.changeButton.interactable = bindings != null && !listening;
            }
        }

        if (resetBindingsButton != null)
        {
            resetBindingsButton.interactable = bindings != null && !listening;
        }
    }

    private void OnListeningChanged(bool isListening)
    {
        listening = isListening;

        if (rebindingStatusText != null)
        {
            rebindingStatusText.text = isListening ?
                GamePreferencesText.Get(EPreferencesTextKey.Rebinding) : string.Empty;
        }

        RefreshBindings();
    }

    private void OnRebindRejected(string message)
    {
        if (rebindingStatusText != null)
        {
            rebindingStatusText.text = GamePreferencesText.Get(EPreferencesTextKey.RebindRejected);
        }

        Debug.LogWarning(message);
    }
}
