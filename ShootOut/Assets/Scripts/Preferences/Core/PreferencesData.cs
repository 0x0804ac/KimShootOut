using System;

//JsonUtility-friendly persistence DTO; only the service may mutate its working instance.
[Serializable]
public sealed class PreferencesAudioData
{
    public bool enabled = true;
    public float volume = 1f;
}

[Serializable]
public sealed class PreferencesData
{
    public int version = 1;
    public EPreferencesLanguage language = EPreferencesLanguage.Korean;
    public EPreferencesWindowMode windowMode = EPreferencesWindowMode.Borderless;
    public int screenWidth = 1280;
    public int screenHeight = 720;
    public PreferencesAudioData music = new PreferencesAudioData();
    public PreferencesAudioData effects = new PreferencesAudioData();
    public bool screenShakeEnabled = true;
    public float screenShakeIntensity = 1f;
    public string bindingOverridesJson = "";
}
