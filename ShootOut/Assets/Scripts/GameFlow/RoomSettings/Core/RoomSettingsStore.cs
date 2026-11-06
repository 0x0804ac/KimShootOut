//One slot is allocated per setting at initialization, not per setting edit.
public interface IRoomSettingsSlot
{
    ERoomSettingsID GetID();
    object CaptureBoxed();
    bool AcceptBoxed(object candidate);
    void AssignBoxed(object candidate);
}

public sealed class RoomSettingsSlot<T> : IRoomSettingsSlot where T : struct
{
    private readonly RoomSettingsDefinition<T> definition;
    private T value;

    public RoomSettingsSlot(RoomSettingsDefinition<T> definition)
    {
        this.definition = definition;
        value = definition.GetDefaultValue();
    }

    public ERoomSettingsID GetID()
    {
        return definition.GetID();
    }

    public T Get()
    {
        return value;
    }

    public bool TrySet(T newValue, out bool changed)
    {
        changed = false;

        if (!definition.IsValid(newValue))
        {
            return false;
        }
        if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(value, newValue))
        {
            return true;
        }

        value = newValue;
        changed = true;
        return true;
    }

    public object CaptureBoxed()
    {
        return value;
    }

    public bool AcceptBoxed(object candidate)
    {
        return candidate is T typed && definition.IsValid(typed);
    }

    //모든 값이 검증된 이후에 호출할 것
    public void AssignBoxed(object candidate)
    {
        value = (T)candidate;
    }
}

public sealed class RoomSettingsStore
{
    private readonly IRoomSettingsSlot[] slots;

    public RoomSettingsStore()
    {
        var definitions = RoomSettingsCatalog.GetAll();

        slots = new IRoomSettingsSlot[definitions.Count];
        for (int i = 0; i < definitions.Count; i++)
        {
            slots[i] = definitions[i].CreateSlot();
        }
    }

    public T Get<T>(ERoomSettingsID id) where T : struct
    {
        if (TryGet(id, out T value))
        {
            return value;
        }
        throw new System.InvalidOperationException($"ID 또는 설정값 자료형이 올바르지 않습니다: {id}/{typeof(T).Name}");
    }

    public bool TryGet<T>(ERoomSettingsID id, out T value) where T : struct
    {
        if (TryFindSlot(id, out RoomSettingsSlot<T> slot))
        {
            value = slot.Get();
            return true;
        }

        value = default;
        return false;
    }

    public bool TrySet<T>(RoomSettingsChange<T> change, out bool changed) where T : struct
    {
        if (TryFindSlot(change.id, out RoomSettingsSlot<T> slot))
        {
            return slot.TrySet(change.value, out changed);
        }

        changed = false;
        return false;
    }

    public RoomSettingsData CreateData()
    {
        object[] values = new object[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            values[i] = slots[i].CaptureBoxed();
        }

        return new RoomSettingsData(values);
    }

    public bool TryLoadData(RoomSettingsData data)
    {
        if (data.GetCount() != slots.Length)
        {
            return false;
        }

        //Validate everything first; never partially change the store.
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].AcceptBoxed(data.GetBoxed(i)))
            {
                return false;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].AssignBoxed(data.GetBoxed(i));
        }

        return true;
    }

    private bool TryFindSlot<T>(ERoomSettingsID id, out RoomSettingsSlot<T> slot)
        where T : struct
    {
        int index = (int)id;

        if (index >= 0 && index < slots.Length &&
            slots[index] is RoomSettingsSlot<T> typed)
        {
            slot = typed;
            return true;
        }

        slot = null;
        return false;
    }
}
