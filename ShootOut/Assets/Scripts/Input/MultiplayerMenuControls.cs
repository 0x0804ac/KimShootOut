using UnityEngine;

public class MultiplayerMenuControls : UIControls
{
    [SerializeField] private UIControls eventMenu, normalMenu, rankedMenu, customMenu;

    const string EVENT = "event-button";
    const string NORMAL = "normal-button";
    const string RANKED = "ranked-button";
    const string CUSTOM = "custom-button";

    protected override void Init()
    {
        isHidden = true;
        root.EnableInClassList(RIGHT, true);
    }

    protected override void RegisterEvents()
    {
        RegisterClickEvent(EVENT, OnEventButtonClick);
        RegisterClickEvent(NORMAL, OnNormalButtonClick);
        RegisterClickEvent(RANKED, OnRankedButtonClick);
        RegisterClickEvent(CUSTOM, OnCustomButtonClick);
    }

    protected override void UnregisterEvents()
    {
        UnregisterClickEvent(EVENT, OnEventButtonClick);
        UnregisterClickEvent(NORMAL, OnNormalButtonClick);
        UnregisterClickEvent(RANKED, OnRankedButtonClick);
        UnregisterClickEvent(CUSTOM, OnCustomButtonClick);
    }

    private void OnEventButtonClick()
    {
        print("Event match");
    }

    private void OnNormalButtonClick()
    {
        print("Normal match");
    }

    private void OnRankedButtonClick()
    {
        print("Ranked match");
    }

    private void OnCustomButtonClick()
    {
        print("Custom match");
    }
}
