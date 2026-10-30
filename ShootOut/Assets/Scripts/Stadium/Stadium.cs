using Unity.Entities;
using UnityEngine;

public class Stadium : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject goalPostPrefab;

    static GameObject ball = null;
    static GameObject goalPost = null;

    private void Awake()
    {
        if (ballPrefab != null && ball == null)
        {
            ball = Instantiate(ballPrefab, new Vector3(0, 100, 0), transform.rotation);
        }

        if (goalPostPrefab != null && goalPost == null)
        {
            goalPost = Instantiate(goalPostPrefab, new Vector3(0, 200, 0), transform.rotation);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject GetGoalPost()
    {
        return goalPost;
    }
}
