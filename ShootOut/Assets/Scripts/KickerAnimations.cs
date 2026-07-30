using UnityEngine;

[SharedBetweenAnimators]
public class KickerAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.25f;

    [SerializeField] private GameObject kicker;

    private Rigidbody body;
    private Vector3 movement;
    private bool isMoving;

    void Awake()
    {
        body = kicker.GetComponent<Rigidbody>();
        movement = new Vector3(Constants.KICKER_OFFSET_LEFT.x, 0f, Constants.KICKER_OFFSET_LEFT.z);
        isMoving = false;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isMoving)
        {
            body.MovePosition(body.position + movement * Time.deltaTime);
            if (body.position.z > Constants.PENALTY_SPOT.z)
            {
                isMoving = false;
            }
        }
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (stateMachinePathHash == Constants.ANIMATOR_KICKER_SHOOT) isMoving = true;
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //
    }
}
