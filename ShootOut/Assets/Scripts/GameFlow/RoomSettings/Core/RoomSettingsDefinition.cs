//using System;

//방 설정 (단일 항목과 값) 정의
public interface IRoomSettingsDefinition
{
    ERoomSettingsID GetID();
    LocalizationMetadata GetMetadata();
    IRoomSettingsSlot CreateSlot();
}

//간격이 있는 범위 구조체 (bool 값일 경우 사용하지 않음)
public readonly struct RoomSettingsRange<T> where T : struct
{
    public readonly T min;
    public readonly T max;
    public readonly T step;

    public RoomSettingsRange(T min, T max, T step)
    {
        double lower = RoomSettingsNumeric<T>.ToDouble(min);
        double upper = RoomSettingsNumeric<T>.ToDouble(max);
        double interval = RoomSettingsNumeric<T>.ToDouble(step);

        if (double.IsNaN(lower) || double.IsInfinity(lower) ||
            double.IsNaN(upper) || double.IsInfinity(upper) ||
            double.IsNaN(interval) || double.IsInfinity(interval) ||
            lower > upper || interval <= 0)
        {
            throw new System.ArgumentException("올바르지 않은 값입니다.");
        }

        this.min = min;
        this.max = max;
        this.step = step;
    }

    //범위 안에 있는 올바른 값인지 검사
    public bool Contains(T value)
    {
        double candidate = RoomSettingsNumeric<T>.ToDouble(value);
        double lower = RoomSettingsNumeric<T>.ToDouble(min);
        double upper = RoomSettingsNumeric<T>.ToDouble(max);
        double interval = RoomSettingsNumeric<T>.ToDouble(step);

        if (double.IsNaN(candidate) || double.IsInfinity(candidate) ||
            candidate < lower || candidate > upper)
        {
            return false;
        }

        double position = (candidate - lower) / interval;
        return System.Math.Abs(position - System.Math.Round(position)) < 0.00001d;
    }
}

public sealed class RoomSettingsDefinition<T> : IRoomSettingsDefinition where T : struct
{
    private readonly ERoomSettingsID id;
    private readonly LocalizationMetadata metadata;
    private readonly T defaultValue;
    private readonly RoomSettingsRange<T>? range;

    public RoomSettingsDefinition(
        ERoomSettingsID id,
        LocalizationMetadata metadata,
        T defaultValue,
        RoomSettingsRange<T>? range = null)
    {
        if (range.HasValue && !range.Value.Contains(defaultValue))
        {
            throw new System.ArgumentException("올바르지 않은 기본값", nameof(defaultValue));
        }

        this.id = id;
        this.metadata = metadata;
        this.defaultValue = defaultValue;
        this.range = range;
    }

    public ERoomSettingsID GetID()
    {
        return id;
    }

    public LocalizationMetadata GetMetadata()
    {
        return metadata;
    }

    public T GetDefaultValue()
    {
        return defaultValue;
    }

    public RoomSettingsRange<T>? GetRange()
    {
        return range;
    }

    public IRoomSettingsSlot CreateSlot()
    {
        return new RoomSettingsSlot<T>(this);
    }

    public bool IsValid(T value)
    {
        return !range.HasValue || range.Value.Contains(value);
    }
}
