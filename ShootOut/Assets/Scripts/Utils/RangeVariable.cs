using UnityEngine;
using FloatRangeVariable = RangeVariable<float>;
using IntRangeVariable = RangeVariable<int>;

public class RangeVariable<T>
{
    public T min;
    public T max;

    public RangeVariable(T min, T max)
    {
        this.min = min;
        this.max = max; 
    }
}