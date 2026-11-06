public enum EDisplayLanguage
{
    Korean,
    English
}

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

    public string GetLabel(EDisplayLanguage language)
    {
        return language == EDisplayLanguage.Korean ? koreanLabel : englishLabel;
    }
}
