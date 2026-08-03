using UnityEngine;
using UnityEngine.SceneManagement;

[SharedBetweenAnimators]
public class KickerAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.25f;
    public const float MOVEMENT_MULTIPLIER = 0.77f;
    public const float HIGH_POWER = 120f;
    public const float LOW_POWER = 40f;

    private GameObject kicker, goalkeeper, ball;
    private Animator goalkeeperAnimator;
    private Rigidbody body;
    private Vector3 movement;
    private bool isMoving;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (kicker == null) Init();
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
        float x, y, z;
        if (!isMoving && animator.GetBool(Constants.ANIMATOR_TRIGGER_SHOOT))
        {
            isMoving = true;
            x = animator.GetFloat(Constants.ANIMATOR_VELOCITY_X);
            y = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y);
            z = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z);
            Vector3 vector = new(x, y, z);
            float length = vector.magnitude;
            if (length >= HIGH_POWER) animator.CrossFade(Constants.ANIMATOR_KICKER_SHOOT_STRONG, TRANSITION);
            else if (length >= LOW_POWER) animator.CrossFade(Constants.ANIMATOR_KICKER_SHOOT_NORMAL, TRANSITION);
            else if (StaticValues.attacker.IsLeftFooted) animator.CrossFade(Constants.ANIMATOR_KICKER_SHOOT_WEAK_LEFT, TRANSITION);
            else animator.CrossFade(Constants.ANIMATOR_KICKER_SHOOT_WEAK_RIGHT, TRANSITION);
        }
        else if (isMoving)
        {
            body.MovePosition(body.position - movement * (Time.deltaTime * MOVEMENT_MULTIPLIER));
            if (body.position.z > Constants.PENALTY_SPOT.z)
            {
                x = animator.GetFloat(Constants.ANIMATOR_VELOCITY_X);
                y = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y);
                z = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z);
                ball.GetComponent<Rigidbody>().AddForce(new Vector3(x, y, z), ForceMode.Impulse);
                goalkeeperAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_GOALKEEP);
                animator.ResetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
                isMoving = false;
            }
        }
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (kicker == null) Init();
        if (stateMachinePathHash == Constants.ANIMATOR_KICKER_SHOOT) isMoving = true;
        else if (stateMachinePathHash == Constants.ANIMATOR_KICKER_IDLE) animator.ResetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //
    }

    private void Init()
    {
        kicker = GameObject.FindWithTag(Constants.TAG_KICKER);
        goalkeeper = GameObject.FindWithTag(Constants.TAG_GOALKEEPER);
        ball = GameObject.FindWithTag(Constants.TAG_BALL);
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
}
