using UnityEngine;
using UnityEngine.UIElements;

public class SingleplayerMenuControls : UIControls
{
    [SerializeField] private UIControls tutorialMenu, practiceMenu, vsBotMenu;

    protected override void Init()
    {
        isHidden = true;
        root.EnableInClassList(RIGHT, true);
    }

    protected override void RegisterClickEvents()
    {
        RegisterClickEvent("tutorial-button", OnTutorialButtonClick);
        RegisterClickEvent("practice-button", OnPracticeButtonClick);
        RegisterClickEvent("vsbot-button", OnVersusBotButtonClick);
    }

    protected override void UnregisterClickEvents()
    {
        UnregisterClickEvent("tutorial-button", OnTutorialButtonClick);
        UnregisterClickEvent("practice-button", OnPracticeButtonClick);
        UnregisterClickEvent("vsbot-button", OnVersusBotButtonClick);
    }

    private void OnTutorialButtonClick()
    {
        print("Load Tutorial Scene");
    }

    private void OnPracticeButtonClick()
    {
        MoveToLeft();
    }

    private void OnVersusBotButtonClick()
    {
        MoveToLeft();
    }
}
