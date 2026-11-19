public enum EPreferencesTextKey
{
    Language,
    Video,
    WindowMode,
    Resolution,
    ApplyVideo,
    Music,
    Effects,
    Controls,
    Shake,
    TestShake,
    Enabled,
    Save,
    RestoreKeys,
    Rebinding,
    RebindRejected
}

public static class GamePreferencesText
{
    private static readonly string[,] sLabels =
    {
        { "언어", "Language" },
        { "비디오", "Video" },
        { "화면 모드", "Display mode" },
        { "해상도", "Resolution" },
        { "화면 설정 적용", "Apply display" },
        { "배경 음악", "Music" },
        { "효과음", "Sound effects" },
        { "조작", "Controls" },
        { "화면 흔들림", "Screen shake" },
        { "흔들림 테스트", "Test shake" },
        { "사용", "Enabled" },
        { "저장", "Save" },
        { "기본 키 복원", "Reset bindings" },
        { "키를 누르세요 (Esc: 취소)", "Press a key (Esc: cancel)" },
        { "이미 사용 중인 키입니다.", "Key already in use." }
    };

    public static string Get(EPreferencesTextKey key)
    {
        return sLabels[(int)key, (int)GamePreferencesLocalization.GetLanguage()];
    }
}
