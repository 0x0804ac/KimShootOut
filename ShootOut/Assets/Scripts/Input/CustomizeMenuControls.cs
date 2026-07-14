using UnityEngine;

public class CustomizeMenuControls : UIControls
{
    [SerializeField] private UIControls playerMenu, effectMenu, profileMenu;

    const string PLAYER = "player-button";
    const string EFFECT = "effect-button";
    const string PROFILE = "profile-button";

    protected override void Init()
    {
        isHidden = true;
        root.EnableInClassList(RIGHT, true);
    }

    protected override void RegisterEvents()
    {
        RegisterClickEvent(PLAYER, OnPlayerButtonClick);
        RegisterClickEvent(EFFECT, OnEffectButtonClick);
        RegisterClickEvent(PROFILE, OnProfileButtonClick);
    }

    protected override void UnregisterEvents()
    {
        UnregisterClickEvent(PLAYER, OnPlayerButtonClick);
        UnregisterClickEvent(EFFECT, OnEffectButtonClick);
        UnregisterClickEvent(PROFILE, OnProfileButtonClick);
    }

    private void OnPlayerButtonClick()
    {
        print("Customize player");
    }

    private void OnEffectButtonClick()
    {
        print("Customize effect");
    }

    private void OnProfileButtonClick()
    {
        print("View/Edit profile");
    }
}
