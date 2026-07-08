using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuControls : MonoBehaviour
{
    private UIDocument document;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        RegisterClickEvent("singleplayer-button", OnSingleplayerButtonClick);
        RegisterClickEvent("multiplayer-button", OnMultiplayerButtonClick);
        RegisterClickEvent("customize-button", OnCustomizeButtonClick);
        RegisterClickEvent("settings-button", OnSettingsButtonClick);
        RegisterClickEvent("credits-button", OnCreditsButtonClick);
    }

    private void OnDisable()
    {
        UnregisterClickEvent("singleplayer-button", OnSingleplayerButtonClick);
        UnregisterClickEvent("multiplayer-button", OnMultiplayerButtonClick);
        UnregisterClickEvent("customize-button", OnCustomizeButtonClick);
        UnregisterClickEvent("settings-button", OnSettingsButtonClick);
        UnregisterClickEvent("credits-button", OnCreditsButtonClick);
    }

    private void RegisterClickEvent(string buttonName, System.Action action)
    {
        var button = document.rootVisualElement.Q<Button>(buttonName);
        if (button != null) button.clicked += action;
    }

    private void UnregisterClickEvent(string buttonName, System.Action action)
    {
        var button = document.rootVisualElement.Q<Button>(buttonName);
        if (button != null) button.clicked -= action;
    }

    private void OnSingleplayerButtonClick()
    {

    }

    private void OnMultiplayerButtonClick()
    {

    }

    private void OnCustomizeButtonClick()
    {

    }

    private void OnSettingsButtonClick()
    {

    }

    private void OnCreditsButtonClick()
    {

    }
}
