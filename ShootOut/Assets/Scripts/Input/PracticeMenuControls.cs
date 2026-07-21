using UnityEngine;
using UnityEngine.UIElements;

public class PracticeMenuControls : UIControls
{
    public const string SIDE = "side-dropdown";
    public const string CPU_BEHAVIOUR = "cpu-dropdown";
    const string START = "start-button";
    const string ATTACK = "공격";
    const string DEFENSE = "수비";
    const string OPPONENT = "상대";
    const string SUFFIX = " 방향";

    DropdownField sideDropdown, cpuDropdown;
    Button startButton;

    void Start()
    {
        UnregisterEvents();
    }

    protected override void Init()
    {
        isHidden = true;
        root.EnableInClassList(RIGHT, true);
        sideDropdown = root.Q<DropdownField>(SIDE);
        cpuDropdown = root.Q<DropdownField>(CPU_BEHAVIOUR);
        startButton = root.Q<Button>(START);
        startButton.SetEnabled(false);
    }

    protected override void RegisterEvents()
    {
        sideDropdown.RegisterValueChangedCallback(OnSideChange);
        startButton.clicked += OnStartButtonClick;
    }

    protected override void UnregisterEvents()
    {
        sideDropdown.UnregisterValueChangedCallback(OnSideChange);
        startButton.clicked -= OnStartButtonClick;
    }

    private void OnSideChange(ChangeEvent<string> evt)
    {
        string label;
        switch (sideDropdown.index)
        {
            case 1:
                label = DEFENSE + SUFFIX;
                startButton.SetEnabled(true);
                break;
            case 2:
                label = ATTACK + SUFFIX;
                startButton.SetEnabled(true);
                break;
            default:
                label = OPPONENT + SUFFIX;
                startButton.SetEnabled(false);
                break;
        }
        cpuDropdown.label = label;
    }

    private void OnStartButtonClick()
    {
        switch (sideDropdown.index)
        {
            case 1:
                print("Load attack practice");
                break;
            case 2:
                print("Load defense practice");
                break;
            default:
                print("Please choose side before starting practice");
                break;
        }
    }
}
