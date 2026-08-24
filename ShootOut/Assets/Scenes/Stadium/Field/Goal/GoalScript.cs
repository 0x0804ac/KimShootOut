using UnityEngine;

public class GoalScript : MonoBehaviour
{
    [SerializeField] private Collider col;
    [SerializeField] private ScriptManager manager;

    void OnTriggerStay(Collider other)
    {
        if (manager.Game.IsGoal) return;
        if (other.gameObject.CompareTag("Ball"))
        {
            if (col.bounds.Contains(other.bounds.min) && col.bounds.Contains(other.bounds.max))
            {
                manager.Game.IsGoal = true;
            }
        }
    }
}
