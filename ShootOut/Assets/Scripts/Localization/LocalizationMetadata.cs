public readonly struct LocalizationMetadata
{
    private readonly string koreanLabel;
    private readonly string englishLabel;
    
    public LocalizationMetadata(
        string koreanLabel,
        string englishLabel)
    {
        this.koreanLabel = koreanLabel;
        this.englishLabel = englishLabel;
    }

    public string GetLabel(EPreferencesLanguage language)
    {
        return language == EPreferencesLanguage.Korean ? koreanLabel : englishLabel;
    }
}
