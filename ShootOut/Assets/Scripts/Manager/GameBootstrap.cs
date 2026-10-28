using UnityEngine;

public class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("Called GameBootstrap.Initialize()");

        GameManager.Initialize();
    }
}
