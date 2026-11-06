using System;

//방 설정 전체 데이터
[Serializable]
public readonly struct RoomSettingsData : IEquatable<RoomSettingsData>
{
    private readonly object[] values;
    private readonly int cachedHashCode;

    internal RoomSettingsData(object[] ownedValues)
    {
        //Internal constructor is called only with a brand-new, non-shared array.
        values = ownedValues ?? throw new ArgumentNullException(nameof(ownedValues));
        cachedHashCode = CalculateHash(ownedValues);
    }

    public int GetCount()
    {
        return values == null ? 0 : values.Length;
    }

    public bool TryGet<T>(ERoomSettingsID id, out T value) where T : struct
    {
        int index = (int)id;

        if (values != null &&
            index >= 0 && index < values.Length &&
            values[index] is T typed)
        {
            value = typed;
            return true;
        }

        value = default;
        return false;
    }

    public T Get<T>(ERoomSettingsID id) where T : struct
    {
        if (TryGet(id, out T value))
        {
            return value;
        }

        throw new InvalidOperationException($"ID 또는 설정값 자료형이 올바르지 않습니다: {id}/{typeof(T).Name}");
    }

    //An object is returned only internally. The catalog allows value types only.
    internal object GetBoxed(int index)
    {
        return values[index];
    }

    public bool Equals(RoomSettingsData other)
    {
        if (ReferenceEquals(values, other.values))
        {
            return true;
        }

        if (values == null || other.values == null ||
            values.Length != other.values.Length ||
            cachedHashCode != other.cachedHashCode)
        {
            return false;
        }

        for (int i = 0; i < values.Length; i++)
        {
            if (!object.Equals(values[i], other.values[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object obj)
    {
        return obj is RoomSettingsData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return cachedHashCode;
    }

    private static int CalculateHash(object[] input)
    {
        unchecked
        {
            int hash = 17;

            for (int i = 0; i < input.Length; i++)
            {
                hash = hash * 31 + (input[i] == null ? 0 : input[i].GetHashCode());
            }

            return hash;
        }
    }
}
