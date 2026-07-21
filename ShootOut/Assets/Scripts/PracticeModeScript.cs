using UnityEngine;
using UnityEngine.UIElements;

public class PracticeModeScript : MonoBehaviour
{
    const string CPU_PANEL = "cpu-control-panel";
    const string BUTTON = "toggle-visibility";
    const string CPU_SLIDER = "cpu-power-slider";
    const string PLAYER_SLIDER = "player-power-slider";

    [SerializeField] private UIDocument document;

    private Practice game;
    private VisualElement root;

    void Start()
    {
        root = document.rootVisualElement;
        PracticeType type = (PracticeType)root.Q<DropdownField>(PracticeMenuControls.SIDE).index;
        bool controllable = root.Q<DropdownField>(PracticeMenuControls.CPU_BEHAVIOUR).index == 1;
        game = new Practice(type, controllable, this);
        if (type == PracticeType.ATTACK) root.Q<TemplateContainer>(CPU_SLIDER).visible = false;
        else if (type == PracticeType.DEFENSE) root.Q<TemplateContainer>(PLAYER_SLIDER).visible = false;
        if (!controllable) root.Q<Button>(BUTTON).visible = false;
        game.Load();
    }

    void OnEnable()
    {
        root.Q<Button>(BUTTON).clicked += OnToggleButtonClick;
    }

    void OnDisable()
    {
        root.Q<Button>(BUTTON).clicked -= OnToggleButtonClick;
    }

    private void OnToggleButtonClick()
    {
        root.Q<DropdownField>(CPU_PANEL).visible = !root.Q<DropdownField>(CPU_PANEL).visible;
    }
}
