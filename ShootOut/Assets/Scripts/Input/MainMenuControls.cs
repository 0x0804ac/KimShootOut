using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuControls : MonoBehaviour
{
    private UIDocument document;
    private VisualElement root;
    private bool isHidden = false;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
    }

    private void OnEnable()
    {
        RegisterClickEvents();
    }

    private void OnDisable()
    {
        UnregisterClickEvents();
    }

    private void RegisterClickEvent(string buttonName, System.Action action)
    {
        Button button = root.Q<Button>(buttonName);
        if (button != null) button.clicked += action;
    }

    private void UnregisterClickEvent(string buttonName, System.Action action)
    {
        Button button = root.Q<Button>(buttonName);
        if (button != null) button.clicked -= action;
    }

    private void RegisterClickEvents()
    {
        RegisterClickEvent("singleplayer-button", OnSingleplayerButtonClick);
        RegisterClickEvent("multiplayer-button", OnMultiplayerButtonClick);
        RegisterClickEvent("customize-button", OnCustomizeButtonClick);
        RegisterClickEvent("settings-button", OnSettingsButtonClick);
        RegisterClickEvent("credits-button", OnCreditsButtonClick);
    }

    private void UnregisterClickEvents()
    {
        UnregisterClickEvent("singleplayer-button", OnSingleplayerButtonClick);
        UnregisterClickEvent("multiplayer-button", OnMultiplayerButtonClick);
        UnregisterClickEvent("customize-button", OnCustomizeButtonClick);
        UnregisterClickEvent("settings-button", OnSettingsButtonClick);
        UnregisterClickEvent("credits-button", OnCreditsButtonClick);
    }

    public void ToggleVisible()
    {
        if (!isHidden) UnregisterClickEvents();
        isHidden = !isHidden;
        root.EnableInClassList("offScreen", isHidden);
        if (!isHidden) Invoke(nameof(RegisterClickEvents), 0.5f);
    }

    private void OnSingleplayerButtonClick()
    {
        ToggleVisible();
    }

    private void OnMultiplayerButtonClick()
    {
        ToggleVisible();
    }

    private void OnCustomizeButtonClick()
    {
        ToggleVisible();
    }

    private void OnSettingsButtonClick()
    {
        ToggleVisible();
    }

    private void OnCreditsButtonClick()
    {
        ToggleVisible();
    }

    //public IEnumerator ShowUI(string name) { yield return new WaitForSeconds(0.5f); }
}
