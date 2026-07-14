using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuControls : UIControls
{
    const string RENDER_QUALITY = "render-quality";
    const string FPS = "fps";
    const string MASTER = "master-volume";
    const string MUSIC = "music-volume";
    const string SOUND = "sound-volume";
    const string CONTROL_POSITION = "control-ui-position";
    const string NORMAL_CHAT = "toggle-chat-normal";
    const string CUSTOM_CHAT = "toggle-chat-custom";
    const string SIGN_IN = "signin-button";
    const string NICKNAME = "nickname-button";

    private DropdownField renderQuality, fps, controlUIPosition;
    private Slider masterVolume, musicVolume, soundVolume;
    private Toggle normalChat, customChat;
    private Button signInButton, nicknameButton;
    
    protected override void Init()
    {
        isHidden = true;
        root.EnableInClassList(RIGHT, true);
        renderQuality = root.Q<DropdownField>(RENDER_QUALITY);
        fps = root.Q<DropdownField>(FPS);
        masterVolume = root.Q<Slider>(MASTER);
        musicVolume = root.Q<Slider>(MUSIC);
        soundVolume = root.Q<Slider>(SOUND);
        controlUIPosition = root.Q<DropdownField>(CONTROL_POSITION);
        normalChat = root.Q<Toggle>(NORMAL_CHAT);
        customChat = root.Q<Toggle>(CUSTOM_CHAT);
        signInButton = root.Q<Button>(SIGN_IN);
        nicknameButton = root.Q<Button>(NICKNAME);
    }

    protected override void RegisterEvents()
    {
        renderQuality.RegisterValueChangedCallback(OnRenderQualityChange);
        fps.RegisterValueChangedCallback(OnFPSChange);
        masterVolume.RegisterValueChangedCallback(OnMasterVolumeChange);
        musicVolume.RegisterValueChangedCallback(OnMusicVolumeChange);
        soundVolume.RegisterValueChangedCallback(OnSoundVolumeChange);
        controlUIPosition.RegisterValueChangedCallback(OnControlUIPositionChange);
        normalChat.RegisterValueChangedCallback(OnNormalChatToggle);
        customChat.RegisterValueChangedCallback(OnCustomChatToggle);
        signInButton.clicked += OnSignInButtonClick;
        nicknameButton.clicked += OnNicknameButtonClick;
    }

    protected override void UnregisterEvents()
    {
        renderQuality.UnregisterValueChangedCallback(OnRenderQualityChange);
        fps.UnregisterValueChangedCallback(OnFPSChange);
        masterVolume.UnregisterValueChangedCallback(OnMasterVolumeChange);
        musicVolume.UnregisterValueChangedCallback(OnMusicVolumeChange);
        soundVolume.UnregisterValueChangedCallback(OnSoundVolumeChange);
        controlUIPosition.UnregisterValueChangedCallback(OnControlUIPositionChange);
        normalChat.UnregisterValueChangedCallback(OnNormalChatToggle);
        customChat.UnregisterValueChangedCallback(OnCustomChatToggle);
        signInButton.clicked -= OnSignInButtonClick;
        nicknameButton.clicked -= OnNicknameButtonClick;
    }

    private void OnRenderQualityChange(ChangeEvent<string> evt)
    {
        print($"Render quality: {evt.newValue}");
    }

    private void OnFPSChange(ChangeEvent<string> evt)
    {
        print($"FPS: {evt.newValue}");
    }

    private void OnMasterVolumeChange(ChangeEvent<float> evt)
    {
        print($"Master volume: {evt.newValue}");
    }

    private void OnMusicVolumeChange(ChangeEvent<float> evt)
    {
        print($"Music volume: {evt.newValue}");
    }

    private void OnSoundVolumeChange(ChangeEvent<float> evt)
    {
        print($"Sound volume: {evt.newValue}");
    }

    private void OnControlUIPositionChange(ChangeEvent<string> evt)
    {
        print($"Control UI position: {evt.newValue}");
    }

    private void OnNormalChatToggle(ChangeEvent<bool> evt)
    {
        print($"Show chat/emoji in normal matches: {evt.newValue}");
    }

    private void OnCustomChatToggle(ChangeEvent<bool> evt)
    {
        print($"Show chat/emoji in public custom matches: {evt.newValue}");
    }

    private void OnSignInButtonClick()
    {
        print("Sign in/out");
    }

    private void OnNicknameButtonClick()
    {
        print("Change nickname");
    }
}
