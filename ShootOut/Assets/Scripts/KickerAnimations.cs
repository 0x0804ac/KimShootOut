using UnityEngine;

[SharedBetweenAnimators]
public class KickerAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.25f;

    [SerializeField] private GameObject kicker, goalkeeper, ball;

    private Animator goalkeeperAnimator;
    private Rigidbody body;
    private Vector3 movement;
    private bool isMoving;

    void Awake()
    {
        goalkeeperAnimator = goalkeeper.GetComponent<Animator>();
        body = kicker.GetComponent<Rigidbody>();
        movement = new Vector3();
        if (StaticValues.attacker.IsLeftFooted)
        {
            movement.x = Constants.KICKER_OFFSET_RIGHT.x;
            movement.z = Constants.KICKER_OFFSET_RIGHT.z;
        }
        else
        {
            movement.x = Constants.KICKER_OFFSET_LEFT.x;
            movement.z = Constants.KICKER_OFFSET_LEFT.z;
        }
        isMoving = false;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isMoving && animator.GetBool(Constants.ANIMATOR_TRIGGER_SHOOT))
        {
            isMoving = true;
        }
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
                ball.GetComponent<Rigidbody>().AddForce(new Vector3(animator.GetFloat(Constants.ANIMATOR_VELOCITY_X), animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y), animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z)), ForceMode.Impulse);
                goalkeeperAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_GOALKEEP);
                animator.ResetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
                isMoving = false;
            }
        }
        else if (animator.GetBool(Constants.ANIMATOR_TRIGGER_SHOOT))
        {
            isMoving = true;
            //check direction and magnitude of velocity => add force and play animation
        }
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (stateMachinePathHash == Constants.ANIMATOR_KICKER_SHOOT) isMoving = true;
        else if (stateMachinePathHash == Constants.ANIMATOR_KICKER_IDLE) animator.ResetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //
    }
}
