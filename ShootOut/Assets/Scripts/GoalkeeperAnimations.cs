using UnityEngine;

[SharedBetweenAnimators]
public class GoalkeeperAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.125f;

    private GameObject goalkeeper;
    private Rigidbody body;
    private Vector3 velocity;
    private bool isMoving;

    void Awake()
    {
        goalkeeper = GameObject.FindWithTag(Constants.TAG_GOALKEEPER);
        body = goalkeeper.GetComponent<Rigidbody>();
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
        if (!isMoving && animator.GetBool(Constants.ANIMATOR_TRIGGER_GOALKEEP))
        {
            isMoving = true;
            velocity = new Vector3(animator.GetFloat(Constants.ANIMATOR_VELOCITY_X), animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y), animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z));
            PlayAnimation(animator, velocity);
        }
        else if (animator.GetBool(Constants.ANIMATOR_TRIGGER_IDLE))
        {
            if (Random.Range(0, 8) > 0) animator.Play(Constants.ANIMATOR_GOALKEEPER_IDLE_ARMS_SIDE);
            else animator.Play(Constants.ANIMATOR_GOALKEEPER_IDLE_ARMS_FRONT);
            animator.ResetTrigger(Constants.ANIMATOR_TRIGGER_IDLE);
        }
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (stateMachinePathHash == Constants.ANIMATOR_GOALKEEPER_IDLE) isMoving = false;
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //
    }

    private void PlayAnimation(Animator animator, Vector3 movement)
    {
        body.AddForce(movement);
        float x = movement.x / StaticValues.defender.Speed;
        Debug.Log(x);
        if (x > 0.5f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_RIGHT, TRANSITION);
        else if (x < 0.5f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_LEFT, TRANSITION);
        else if (x > 0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_SHORT_RIGHT, TRANSITION);
        else if (x < 0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_SHORT_LEFT, TRANSITION);
        else
        {
            float y = movement.y / StaticValues.defender.Speed;
            if (y > 0.5f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_JUMP_MISS, TRANSITION);
            else if (y > 0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_HIGH, TRANSITION);
            else if (y > -0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_NORMAL, TRANSITION);
            else animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_LOW, TRANSITION);
        }
    }
}
