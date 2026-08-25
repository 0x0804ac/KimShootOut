using UnityEngine;

public class SaveScript : MonoBehaviour
{
    [SerializeField] private ScriptManager manager;
    [SerializeField] private GameObject ball;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.thisGameObject || !collision.gameObject) return;
        if (collision.thisGameObject == gameObject && collision.gameObject == ball)
        {
            manager.Game.IsSave = true;
        }
    }
}
