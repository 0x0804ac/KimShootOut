public interface IPreferencesDefinition
{
    EPreferencesID GetID();
    EPreferencesCategory GetCategory();
    string GetLabel(EPreferencesLanguage language);
    IPreferencesSlot CreateSlot();
}

//Reuses one generic definition for bool, int, float, and enum values.
//Specialized side effects (video/input/mixer/shake) stay out of this type.
public sealed class PreferencesDefinition<T> : IPreferencesDefinition
    where T : struct
{
    private readonly EPreferencesID id;
    private readonly EPreferencesCategory category;
    private readonly T defaultValue;
    private readonly System.Func<T, bool> validator;
    private readonly string koreanLabel;
    private readonly string englishLabel;

    public PreferencesDefinition(
        EPreferencesID id,
        EPreferencesCategory category,
        T defaultValue,
        System.Func<T, bool> validator,
        string koreanLabel,
        string englishLabel)
    {
        this.id = id;
        this.category = category;
        this.defaultValue = defaultValue;
        this.validator = validator ?? throw new System.ArgumentNullException(nameof(validator));
        this.koreanLabel = koreanLabel;
        this.englishLabel = englishLabel;

        if (!validator(defaultValue))
        {
            throw new System.ArgumentException("올바르지 않은 기본값입니다: ", nameof(defaultValue));
        }
    }

    public EPreferencesID GetID()
    {
        return id;
    }

    public EPreferencesCategory GetCategory()
    {
        return category;
    }

    public string GetLabel(EPreferencesLanguage language)
    {
        return language == EPreferencesLanguage.Korean ? koreanLabel : englishLabel;
    }

    public IPreferencesSlot CreateSlot()
    {
        return new PreferencesSlot<T>(this, defaultValue);
    }

    public bool IsValid(T value)
    {
        return validator(value);
    }
}
