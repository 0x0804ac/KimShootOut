using UnityEngine;
using UnityEngine.UIElements;

public class SingleplayerMenuControls : UIControls
{
    [SerializeField] private UIControls tutorialMenu, practiceMenu, vsBotMenu;

    const string TUTORIAL = "tutorial-button";
    const string PRACTICE = "practice-button";
    const string BOTMATCH = "vsbot-button";

    protected override void Init()
    {
        isHidden = true;
        root.EnableInClassList(RIGHT, true);
    }

    protected override void RegisterEvents()
    {
        RegisterClickEvent(TUTORIAL, OnTutorialButtonClick);
        RegisterClickEvent(PRACTICE, OnPracticeButtonClick);
        RegisterClickEvent(BOTMATCH, OnVersusBotButtonClick);
    }

    protected override void UnregisterEvents()
    {
        UnregisterClickEvent(TUTORIAL, OnTutorialButtonClick);
        UnregisterClickEvent(PRACTICE, OnPracticeButtonClick);
        UnregisterClickEvent(BOTMATCH, OnVersusBotButtonClick);
    }

    private void OnTutorialButtonClick()
    {
        print("Load Tutorial Scene");
    }

    private void OnPracticeButtonClick()
    {
        MoveToLeft();
        practiceMenu.MoveToCenter();
        MainMenuCamera.MoveCameraRight();
    }

    private void OnVersusBotButtonClick()
    {
        MoveToLeft();
        //vsBotMenu.MoveToCenter();
        MainMenuCamera.MoveCameraRight();
    }
}
