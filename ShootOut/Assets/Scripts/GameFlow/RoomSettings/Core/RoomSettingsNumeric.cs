// Created once per closed generic type. No boxing in numeric conversions during edits.
public static class RoomSettingsNumeric<T> where T : struct
{
    private static readonly System.Func<T, double> sToDouble = CreateToDouble();
    private static readonly System.Func<double, T> sFromDouble = CreateFromDouble();

    public static double ToDouble(T value)
    {
        return sToDouble(value);
    }

    public static T FromDouble(double value)
    {
        return sFromDouble(value);
    }

    private static System.Func<T, double> CreateToDouble()
    {
        if (typeof(T) == typeof(int))
        {
            return (System.Func<T, double>)(object)(System.Func<int, double>)(value => value);
        }
        if (typeof(T) == typeof(float))
        {
            return (System.Func<T, double>)(object)(System.Func<float, double>)(value => value);
        }
        throw new System.NotSupportedException($"int 또는 float 형식만 지원됩니다: {typeof(T)}");
    }

    private static System.Func<double, T> CreateFromDouble()
    {
        if (typeof(T) == typeof(int))
        {
            return (System.Func<double, T>)(object)(System.Func<double, int>)(value => checked((int)System.Math.Round(value)));
        }
        if (typeof(T) == typeof(float))
        {
            return (System.Func<double, T>)(object)(System.Func<double, float>)(value => checked((float)value));
        }
        throw new System.NotSupportedException($"int 또는 float 형식만 지원됩니다: {typeof(T)}");
    }
}
