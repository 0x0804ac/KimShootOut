using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuControls : UIControls
{
    [SerializeField] private UIControls singleplayerScript, multiplayerScript, customizeScript, settingsScript, creditsScript;

    const string SINGLEPLAYER = "singleplayer-button";
    const string MULTIPLAYER = "multiplayer-button";
    const string CUSTOMIZE = "customize-button";
    const string SETTINGS = "settings-button";
    const string CREDITS = "credits-button";
    const string QUIT = "quit-button";

    protected override void Init()
    {
        isHidden = false;
    }

    protected override void RegisterEvents()
    {
        RegisterClickEvent(SINGLEPLAYER, OnSingleplayerButtonClick);
        RegisterClickEvent(MULTIPLAYER, OnMultiplayerButtonClick);
        RegisterClickEvent(CUSTOMIZE, OnCustomizeButtonClick);
        RegisterClickEvent(SETTINGS, OnSettingsButtonClick);
        RegisterClickEvent(CREDITS, OnCreditsButtonClick);
        RegisterClickEvent(QUIT, OnQuitButtonClick);
    }

    protected override void UnregisterEvents()
    {
        UnregisterClickEvent(SINGLEPLAYER, OnSingleplayerButtonClick);
        UnregisterClickEvent(MULTIPLAYER, OnMultiplayerButtonClick);
        UnregisterClickEvent(CUSTOMIZE, OnCustomizeButtonClick);
        UnregisterClickEvent(SETTINGS, OnSettingsButtonClick);
        UnregisterClickEvent(CREDITS, OnCreditsButtonClick);
        UnregisterClickEvent(QUIT, OnQuitButtonClick);
    }

    private void OnSingleplayerButtonClick()
    {
        MoveToLeft();
        singleplayerScript.MoveToCenter();
    }

    private void OnMultiplayerButtonClick()
    {
        MoveToLeft();
        multiplayerScript.MoveToCenter();
    }

    private void OnCustomizeButtonClick()
    {
        MoveToLeft();
        customizeScript.MoveToCenter();
    }

    private void OnSettingsButtonClick()
    {
        MoveToLeft();
        settingsScript.MoveToCenter();
    }

    private void OnCreditsButtonClick()
    {
        MoveToLeft();
        creditsScript.MoveToCenter();
    }

    private void OnQuitButtonClick()
    {
        print("Show quit confirm dialog");
    }
}
