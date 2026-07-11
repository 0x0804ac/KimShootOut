using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuControls : MonoBehaviour
{
    [SerializeField] private UIDocument singleplayerDocument, multiplayerDocument, customizeDocument, settingsDocument, creditsDocument;

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
        root.RegisterCallback<TransitionEndEvent>(OnTransitionEnd);
    }

    private void OnDisable()
    {
        UnregisterClickEvents();
        root.UnregisterCallback<TransitionEndEvent>(OnTransitionEnd);
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
        root.EnableInClassList("offScreenLeft", isHidden);
    }

    private void OnSingleplayerButtonClick()
    {
        ToggleVisible();
        singleplayerDocument.rootVisualElement.EnableInClassList("offScreenRight", false);
    }

    private void OnMultiplayerButtonClick()
    {
        ToggleVisible();
        multiplayerDocument.rootVisualElement.EnableInClassList("offScreenRight", false);
    }

    private void OnCustomizeButtonClick()
    {
        ToggleVisible();
        customizeDocument.rootVisualElement.EnableInClassList("offScreenRight", false);
    }

    private void OnSettingsButtonClick()
    {
        ToggleVisible();
        settingsDocument.rootVisualElement.EnableInClassList("offScreenRight", false);
    }

    private void OnCreditsButtonClick()
    {
        ToggleVisible();
        creditsDocument.rootVisualElement.EnableInClassList("offScreenRight", false);
    }

    private void OnTransitionEnd(TransitionEndEvent evt)
    {
        if (!isHidden) RegisterClickEvents();
    }
}
