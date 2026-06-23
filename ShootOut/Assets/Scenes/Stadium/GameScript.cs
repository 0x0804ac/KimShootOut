using System.Collections;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject soccerBall;
    public GameObject goalNet;
    public ScriptManager manager;

    private readonly Vector3 penaltySpot = new(0f, 0.11f, 41f);
    private readonly float velocityMultiplier = 23.0f;
    private bool canShoot = true;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = soccerBall.GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void ResetBall()
    {
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        soccerBall.transform.localPosition = penaltySpot;
        manager.goalScript.flag = false;
        canShoot = true;
    }

    public void Shoot(Vector2 direction, float power)
    {
        if (!canShoot) return;
        canShoot = false;
        Vector3 v = direction.normalized;
        v.z = 1.0f;
        rigidBody.AddForce(v * power * velocityMultiplier, ForceMode.Impulse);
        StartCoroutine(ExampleCoroutine());
    }

    public bool CanShoot() { return canShoot; }

    public IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        ResetBall();
        manager.buttonScript.ResetControls();
    }
}
