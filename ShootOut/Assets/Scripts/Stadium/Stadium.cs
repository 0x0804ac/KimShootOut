using Unity.Entities;
using UnityEngine;

public class Stadium : MonoBehaviour
{
    [SerializeField]
    GameObject ball;

    [SerializeField]
    GameObject goalPost;

    private static Stadium instance;

    private Stadium()
    {

    }

    public EResult Init()
    {
        EResult result = MakeInstance();

        return EResult.Success;
    }

    private EResult MakeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return EResult.Failed;
        }

        instance = this;

        return EResult.Success;
    }

    public static Stadium GetInstance()
    {
        if (instance == null)
        {
            return null; 
        }
        return instance;
    }

    public GameObject GetBall()
    {
        return ball;
    }

    public GameObject GetGoalPost()
    {
        return goalPost;
    }
}
