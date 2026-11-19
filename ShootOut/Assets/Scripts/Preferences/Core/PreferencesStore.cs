public interface IPreferencesSlot
{

}

public sealed class PreferencesSlot<T> : IPreferencesSlot where T : struct
{
    private readonly PreferencesDefinition<T> definition;
    private T value;

    public PreferencesSlot(PreferencesDefinition<T> definition, T defaultValue)
    {
        this.definition = definition;
        value = defaultValue;
    }

    public T Get()
    {
        return value;
    }

    public bool CanSet(T candidate)
    {
        return definition.IsValid(candidate);
    }

    public bool TrySet(T candidate, out bool changed)
    {
        changed = false;

        if (!CanSet(candidate))
        {
            return false;
        }

        if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(value, candidate))
        {
            return true;
        }

        value = candidate;
        changed = true;
        return true;
    }
}

//One slot per setting allocated once; edits do not clone an object[] or hash values.
public sealed class PreferencesStore
{
    private readonly IPreferencesSlot[] slots;

    public PreferencesStore()
    {
        System.Collections.Generic.IReadOnlyList<IPreferencesDefinition> definitions = PreferencesCatalog.GetAll();
        slots = new IPreferencesSlot[definitions.Count];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = definitions[i].CreateSlot();
        }
    }

    public T Get<T>(EPreferencesID id) where T : struct
    {
        if (TryGet(id, out T value))
        {
            return value;
        }

        throw new System.InvalidOperationException($"개인 설정 ID 또는 자료형이 올바르지 않습니다: {id}/{typeof(T).Name}");
    }

    public bool TryGet<T>(EPreferencesID id, out T value) where T : struct
    {
        if (TryFindSlot(id, out PreferencesSlot<T> slot))
        {
            value = slot.Get();
            return true;
        }

        value = default;
        return false;
    }

    public bool CanSet<T>(EPreferencesID id, T candidate) where T : struct
    {
        return TryFindSlot(id, out PreferencesSlot<T> slot) && slot.CanSet(candidate);
    }

    public bool TrySet<T>(EPreferencesID id, T candidate, out bool changed)
        where T : struct
    {
        if (TryFindSlot(id, out PreferencesSlot<T> slot))
        {
            return slot.TrySet(candidate, out changed);
        }

        changed = false;
        return false;
    }

    private bool TryFindSlot<T>(EPreferencesID id, out PreferencesSlot<T> slot)
        where T : struct
    {
        int index = (int)id;
        if (index >= 0 && index < slots.Length &&
            slots[index] is PreferencesSlot<T> typed)
        {
            slot = typed;
            return true;
        }

        slot = null;
        return false;
    }
}
