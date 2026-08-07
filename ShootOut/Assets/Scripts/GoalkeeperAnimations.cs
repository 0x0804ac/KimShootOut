using UnityEngine;

[SharedBetweenAnimators]
public class GoalkeeperAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.125f;
    public const float MOVEMENT_MULTIPLIER = 55f;
    public const float JUMP_MULTIPLIER = 0.11f;

    private GameObject goalkeeper;
    private Rigidbody body;
    private Vector3 velocity;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (goalkeeper == null) Init();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(Constants.ANIMATOR_TRIGGER_GOALKEEP))
        {
            velocity.x = animator.GetFloat(Constants.ANIMATOR_VELOCITY_X) * MOVEMENT_MULTIPLIER;
            velocity.y = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y) * JUMP_MULTIPLIER;
            velocity.z = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z) * Constants.MULTIPLIER_Z;
            PlayAnimation(animator, velocity);
            animator.ResetTrigger(Constants.ANIMATOR_TRIGGER_GOALKEEP);
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
        if (goalkeeper == null) Init();
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //
    }

    private void Init()
    {
        goalkeeper = GameObject.FindWithTag(Constants.TAG_GOALKEEPER);
        body = goalkeeper.GetComponent<Rigidbody>();
        velocity = new Vector3();
    }

    private void PlayAnimation(Animator animator, Vector3 movement)
    {
        float x = movement.x / StaticValues.defender.Speed;
        Debug.Log(x);
        if (x > 0.5f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_RIGHT, TRANSITION);
        else if (x < 0.5f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_LEFT, TRANSITION);
        else if (x > 0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_SHORT_RIGHT, TRANSITION);
        else if (x < 0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_SHORT_LEFT, TRANSITION);
        else
        {
            float y = movement.y / StaticValues.defender.Speed;
            if (y > 0.125f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_JUMP_MISS, TRANSITION);
            else if (y > 0.0625f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_HIGH, TRANSITION);
            else if (y > -0.0625f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_NORMAL, TRANSITION);
            else animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_LOW, TRANSITION);
        }
        body.AddForce(movement, ForceMode.Impulse);
    }
}
