using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject soccerBall;
    public GameObject goalNet;
    public GoalScript goalScript;

    private readonly Vector3 penaltySpot = new Vector3(0f, 0.11f, 41f);

    private Rigidbody rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = soccerBall.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBall()
    {
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        soccerBall.transform.localPosition = penaltySpot;
        goalScript.flag = false;
    }

    public void Shoot(Vector2 direction, float power)
    {
        rigidBody.AddForce(new Vector3(direction.x, direction.y, 1.0f) * power, ForceMode.Impulse);
    }
}
