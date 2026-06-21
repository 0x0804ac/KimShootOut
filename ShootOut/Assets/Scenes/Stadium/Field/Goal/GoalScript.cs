using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public bool flag = false;
    public Collider col;

    void Start()
    {
        if (col == null) col = GetComponent<Collider>();
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (flag) return;
        if (other.gameObject.tag == "Ball")
        {
            Vector3 center = other.transform.position;
            Vector3 pos = col.ClosestPointOnBounds(center);
            if (Vector3.Distance(center, pos) > other.GetComponent<SphereCollider>().radius)
            {
                Debug.Log("GOAL!");
                flag = true;
            }
        }
    }
}
