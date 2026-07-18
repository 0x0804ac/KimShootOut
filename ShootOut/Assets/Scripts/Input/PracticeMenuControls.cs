using UnityEngine;
using UnityEngine.UIElements;

public class PracticeMenuControls : UIControls
{
    const string SIDE = "side-dropdown";
    const string CPU_BEHAVIOUR = "cpu-dropdown";
    const string START = "start-button";
    const string ATTACK = "공격";
    const string DEFENSE = "수비";
    const string OPPONENT = "상대";
    const string SUFFIX = " 방향";

    DropdownField sideDropdown, cpuDropdown;
    Button startButton;

    protected override void Init()
    {
        isHidden = true;
        root.EnableInClassList(RIGHT, true);
        sideDropdown = root.Q<DropdownField>(SIDE);
        cpuDropdown = root.Q<DropdownField>(CPU_BEHAVIOUR);
        startButton = root.Q<Button>(START);
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
                label = ATTACK + SUFFIX;
                break;
            case 2:
                label = DEFENSE + SUFFIX;
                break;
            default:
                label = OPPONENT + SUFFIX;
                break;
        }
        cpuDropdown.label = label;
    }

    private void OnStartButtonClick()
    {
        print("Load practice mode scene");
    }
}
