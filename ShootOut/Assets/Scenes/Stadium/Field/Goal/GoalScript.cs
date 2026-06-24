using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public Collider col;
    public ScriptManager manager;

    private bool isReady = true;

    public bool Ready
    {
        get { return isReady; }
        set { isReady = value; }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (!isReady) return;
        if (other.gameObject.CompareTag("Ball"))
        {
            if (col.bounds.Contains(other.bounds.min) && col.bounds.Contains(other.bounds.max))
            {
                Debug.Log("GOAL!");
                isReady = false;
            }
        }
    }
}
