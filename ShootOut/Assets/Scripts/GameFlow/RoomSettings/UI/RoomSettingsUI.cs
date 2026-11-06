using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class RoomSettingsViewBase
{
    public ERoomSettingsID id;
    public TMP_Text label;
    public TMP_Text valueText;
}

[Serializable]
public sealed class RoomSettingsSliderView : RoomSettingsViewBase
{
    public Slider control;
}

[Serializable]
public sealed class RoomSettingsToggleView : RoomSettingsViewBase
{
    public Toggle control;
}

//테스트용 임시 UI
public sealed class RoomSettingsUI : MonoBehaviour
{
    [SerializeField] private RoomSettingsSliderView[] sliders;
    [SerializeField] private RoomSettingsToggleView[] toggles;
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private Button localStartGameButton;

    private RoomSettingsService service;
    private readonly List<Action> redrawAll = new List<Action>();
    private readonly Dictionary<ERoomSettingsID, Action> redrawById =
        new Dictionary<ERoomSettingsID, Action>();
    private readonly List<Action> detachListeners = new List<Action>();

    private void Awake()
    {
        service = RoomSettingsBootstrap.GetService();
        BindControls();
        BindLanguageDropdown();
        BindLocalStartButton();
    }

    private void OnEnable()
    {
        if (service == null)
        {
            return;
        }

        service.SettingsChanged += RefreshSettings;
        service.SettingsReplaced += RefreshAll;
        service.RoomStateChanged += RefreshAccess;
        GameLocalization.LanguageChanged += RefreshAll;
        RefreshAll();
    }

    private void OnDisable()
    {
        if (service != null)
        {
            service.SettingsChanged -= RefreshSettings;
            service.SettingsReplaced -= RefreshAll;
            service.RoomStateChanged -= RefreshAccess;
        }

        GameLocalization.LanguageChanged -= RefreshAll;
    }

    private void OnDestroy()
    {
        foreach (Action detach in detachListeners)
        {
            detach();
        }

        detachListeners.Clear();
        redrawAll.Clear();
        redrawById.Clear();
    }

    private void BindControls()
    {
        if (sliders != null)
        {
            foreach (RoomSettingsSliderView view in sliders)
            {
                if (view == null || view.control == null)
                {
                    continue;
                }

                if (RoomSettingsCatalog.TryGet(view.id, out RoomSettingsDefinition<int> intDefinition))
                {
                    BindSlider(view, intDefinition);
                }
                else if (RoomSettingsCatalog.TryGet(view.id, out RoomSettingsDefinition<float> floatDefinition))
                {
                    BindSlider(view, floatDefinition);
                }
                else
                {
                    Debug.LogError($"슬라이더에는 int/float 값을 사용해야 합니다: {view.id}");
                }
            }
        }

        if (toggles != null)
        {
            foreach (RoomSettingsToggleView view in toggles)
            {
                if (view == null || view.control == null)
                {
                    continue;
                }

                if (RoomSettingsCatalog.TryGet(view.id, out RoomSettingsDefinition<bool> boolDefinition))
                {
                    BindToggle(view, boolDefinition);
                }
                else
                {
                    Debug.LogError($"토글에는 bool 값을 사용해야 합니다: {view.id}");
                }
            }
        }
    }

    private void BindSlider<T>(
        RoomSettingsSliderView view,
        RoomSettingsDefinition<T> definition)
        where T : struct
    {
        if (!definition.GetRange().HasValue)
        {
            Debug.LogError($"슬라이더를 사용하려면 유효한 범위가 필요합니다: {definition.GetID()}");
            return;
        }

        RoomSettingsRange<T> range = definition.GetRange().Value;
        double minimum = RoomSettingsNumeric<T>.ToDouble(range.min);
        double maximum = RoomSettingsNumeric<T>.ToDouble(range.max);
        double step = RoomSettingsNumeric<T>.ToDouble(range.step);

        view.control.minValue = (float)minimum;
        view.control.maxValue = (float)maximum;
        view.control.wholeNumbers = typeof(T) == typeof(int);

        UnityAction<float> handler = rawValue =>
        {
            double count = Math.Round((rawValue - minimum) / step);
            double snapped = Math.Max(minimum, Math.Min(maximum, minimum + count * step));
            T typed = RoomSettingsNumeric<T>.FromDouble(snapped);

            if (!service.RequestChange(definition.GetID(), typed))
            {
                //A refused local request restores the last confirmed value.
                view.control.SetValueWithoutNotify((float)RoomSettingsNumeric<T>.ToDouble(
                    service.Get<T>(definition.GetID())));
            }
        };

        view.control.onValueChanged.AddListener(handler);
        detachListeners.Add(() => view.control.onValueChanged.RemoveListener(handler));

        RegisterRedraw(definition.GetID(), () =>
        {
            T value = service.Get<T>(definition.GetID());
            double number = RoomSettingsNumeric<T>.ToDouble(value);
            view.control.SetValueWithoutNotify((float)number);
            view.control.interactable = service.CanLocalEdit();

            if (view.label != null)
            {
                view.label.text = definition.GetMetadata().GetLabel(GameLocalization.GetLanguage());
            }

            if (view.valueText != null)
            {
                view.valueText.text = view.control.wholeNumbers ?
                number.ToString("F0") : number.ToString("F2");
            }
        });
    }

    private void BindToggle(RoomSettingsToggleView view, RoomSettingsDefinition<bool> definition)
    {
        UnityAction<bool> handler = value =>
        {
            if (!service.RequestChange(definition.GetID(), value))
            {
                view.control.SetIsOnWithoutNotify(service.Get<bool>(definition.GetID()));
            }
        };

        view.control.onValueChanged.AddListener(handler);
        detachListeners.Add(() => view.control.onValueChanged.RemoveListener(handler));

        RegisterRedraw(definition.GetID(), () =>
        {
            bool value = service.Get<bool>(definition.GetID());
            view.control.SetIsOnWithoutNotify(value);
            view.control.interactable = service.CanLocalEdit();

            if (view.label != null)
            {
                view.label.text = definition.GetMetadata().GetLabel(GameLocalization.GetLanguage());
            }

            if (view.valueText != null)
            {
                view.valueText.text = GameLocalization.FormatToggle(value);
            }
        });
    }

    private void RegisterRedraw(ERoomSettingsID id, Action redraw)
    {
        if (redrawById.ContainsKey(id))
        {
            redrawById[id] += redraw;
        }
        else
        {
            redrawById.Add(id, redraw);
        }

        redrawAll.Add(redraw);
    }

    private void BindLanguageDropdown()
    {
        if (languageDropdown == null)
        {
            return;
        }

        UnityAction<int> handler = index =>
            GameLocalization.SetLanguage(index == 0 ? EDisplayLanguage.Korean : EDisplayLanguage.English);

        languageDropdown.onValueChanged.AddListener(handler);
        detachListeners.Add(() => languageDropdown.onValueChanged.RemoveListener(handler));
        languageDropdown.SetValueWithoutNotify((int)GameLocalization.GetLanguage());
    }

    private void BindLocalStartButton()
    {
        if (localStartGameButton == null)
        {
            return;
        }

        UnityAction handler = StartLocalGame;
        localStartGameButton.onClick.AddListener(handler);
        detachListeners.Add(() => localStartGameButton.onClick.RemoveListener(handler));
    }

    private void RefreshSettings(ERoomSettingsID id)
    {
        if (redrawById.TryGetValue(id, out Action redraw))
        {
            redraw();
        }
    }

    private void RefreshAll()
    {
        foreach (Action redraw in redrawAll)
        {
            redraw();
        }

        RefreshAccess();
    }

    private void RefreshAccess()
    {
        //A room-state change can alter editing rights without changing a value.
        bool canEdit = service.CanLocalEdit();

        if (sliders != null)
        {
            foreach (RoomSettingsSliderView view in sliders)
            {
                if (view != null && view.control != null)
                {
                    view.control.interactable = canEdit;
                }
            }
        }

        if (toggles != null)
        {
            foreach (RoomSettingsToggleView view in toggles)
            {
                if (view != null && view.control != null)
                {
                    view.control.interactable = canEdit;
                }
            }
        }

        if (localStartGameButton != null)
        {
            localStartGameButton.interactable = canEdit;
        }

        if (languageDropdown != null)
        {
            languageDropdown.interactable = canEdit;
        }
    }

    private void StartLocalGame()
    {
        ERoomSettingsResult result = RoomSettingsBootstrap.TryStartLocalGame();

        if (!result.IsSuccess())
        {
            Debug.LogWarning($"게임 시작에 실패했습니다: {result}");
        }
    }
}
