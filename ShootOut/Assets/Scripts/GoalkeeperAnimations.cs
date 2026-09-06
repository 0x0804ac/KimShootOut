using UnityEngine;

[SharedBetweenAnimators]
public class GoalkeeperAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.125f;
    public const float MOVEMENT_MULTIPLIER = 0.55f;
    public const float JUMP_MULTIPLIER = 0.5f;
    public readonly int IDLE_HASH = Animator.StringToHash("A");

    private GameObject goalkeeper;
    private Rigidbody body;
    private Vector3 velocity;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (goalkeeper == null) Init();
        if (animator.GetBool(Constants.ANIMATOR_TRIGGER_GOALKEEP) && stateInfo.IsTag(Constants.ANIMATOR_TRIGGER_GOALKEEP))
        {
            animator.SetBool(Constants.ANIMATOR_TRIGGER_GOALKEEP, false);
            velocity.x = animator.GetFloat(Constants.ANIMATOR_VELOCITY_X) * (MOVEMENT_MULTIPLIER * StaticValues.defender.Speed);
            velocity.y = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y) * JUMP_MULTIPLIER;
            velocity.z = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z) * Constants.MULTIPLIER_Z;
            body.AddForce(velocity, ForceMode.Impulse); //PlayAnimation(animator, velocity);
        }
        else if (stateInfo.IsTag(Constants.ANIMATOR_TRIGGER_IDLE))
        {
            animator.ResetTrigger(Constants.ANIMATOR_TRIGGER_IDLE);
            animator.SetInteger(IDLE_HASH, Random.Range(0, 8) == 0 ? 1 : 0);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(Constants.ANIMATOR_MIRRORED)) animator.SetBool(Constants.ANIMATOR_MIRRORED, false);
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
        if (x > 5) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_LEFT, TRANSITION);
        else if (x < -5) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_RIGHT, TRANSITION);
        else if (x > 3) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_NORMAL_LEFT, TRANSITION);
        else if (x < -3)
        {
            animator.SetBool(Constants.ANIMATOR_MIRRORED, true);
            animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_LEFT, TRANSITION);
        }
        else if (x > 1) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_SHORT_LEFT, TRANSITION);
        else if (x < -1) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_SHORT_RIGHT, TRANSITION);
        else
        {
            float y = movement.y / StaticValues.defender.Speed;
            Debug.Log(y);
            if (y > 0.125f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_JUMP_MISS, TRANSITION);
            else if (y > 0.0625f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_HIGH, TRANSITION);
            else if (y > -0.0625f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_NORMAL, TRANSITION);
            else animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_LOW, TRANSITION);
        }
        body.AddForce(movement, ForceMode.Impulse);
    }
}
