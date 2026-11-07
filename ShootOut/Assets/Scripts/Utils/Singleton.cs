using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    private static T instance;

    public abstract EResult Init();

    protected virtual EResult MakeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return EResult.Failed;
        }

        instance = (T)this;

        return EResult.Success;
    }

    public static T GetIntance()
    {
        return instance;
    }
}
