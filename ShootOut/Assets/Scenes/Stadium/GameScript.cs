using System.Collections;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject soccerBall;
    public GameObject goalNet;
    public GameObject goalkeeper;
    public ScriptManager manager;

    private readonly Vector3 penaltySpot = new(0f, 0.11f, 41f);
    private readonly Vector3 goalLine = new(0f, 1f, 52f);
    private readonly Vector3 pos = new(0f, 0f, 0.5f);
    private readonly float ballMultiplier = 25.0f;
    private readonly float keeperMultiplier = 10.0f;
    private int turn = 0;
    private bool isReady = true;
    private Rigidbody ball, keeper;

    public int Turn { get { return turn; } }

    void Start()
    {
        ball = soccerBall.GetComponent<Rigidbody>();
        keeper = goalkeeper.GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void FinishTurn()
    {
        ball.linearVelocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        soccerBall.transform.localPosition = penaltySpot;
        keeper.linearVelocity = Vector3.zero;
        keeper.angularVelocity = Vector3.zero;
        goalkeeper.transform.SetLocalPositionAndRotation(goalLine, Quaternion.LookRotation(Vector3.forward));
        turn++;
        manager.goalScript.Ready = true;
        isReady = true;
    }

    public void Shoot(Vector2 direction, float power)
    {
        if (!isReady) return;
        isReady = false;
        Vector3 v = new(direction.x, direction.y, 1.0f);
        ball.AddForce(v * (power * ballMultiplier), ForceMode.Impulse);
        StartCoroutine(ExampleCoroutine());
    }

    public void Goalkeep(Vector2 direction)
    {
        if (!isReady) return;
        isReady = false;
        Vector3 v = new(direction.x, direction.y, -0.1f);
        keeper.AddForceAtPosition(v * keeperMultiplier, pos, ForceMode.Impulse);
        StartCoroutine(ExampleCoroutine());
    }

    public bool IsReady() { return isReady; }

    public IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        FinishTurn();
        manager.buttonScript.ResetControls();
    }
}
