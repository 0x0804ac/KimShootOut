using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuControls : UIControls
{
    [SerializeField] private UIControls singleplayerScript, multiplayerScript, customizeScript, settingsScript, creditsScript;

    protected override void Init()
    {
        isHidden = false;
    }

    protected override void RegisterClickEvents()
    {
        RegisterClickEvent("singleplayer-button", OnSingleplayerButtonClick);
        RegisterClickEvent("multiplayer-button", OnMultiplayerButtonClick);
        RegisterClickEvent("customize-button", OnCustomizeButtonClick);
        RegisterClickEvent("settings-button", OnSettingsButtonClick);
        RegisterClickEvent("credits-button", OnCreditsButtonClick);
    }

    protected override void UnregisterClickEvents()
    {
        UnregisterClickEvent("singleplayer-button", OnSingleplayerButtonClick);
        UnregisterClickEvent("multiplayer-button", OnMultiplayerButtonClick);
        UnregisterClickEvent("customize-button", OnCustomizeButtonClick);
        UnregisterClickEvent("settings-button", OnSettingsButtonClick);
        UnregisterClickEvent("credits-button", OnCreditsButtonClick);
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
}
