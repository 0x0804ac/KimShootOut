using UnityEngine;

public enum EResult
{
    Failed,
    Success,
    NotDefined,
}

public static class Result
{
    public static void Check(EResult result)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (result != EResult.Success)
        {
            Debug.LogError($"Operation failed: {result}");
        }
#endif
    }
}