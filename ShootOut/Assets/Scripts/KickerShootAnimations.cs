using UnityEngine;

[SharedBetweenAnimators]
public class KickerShootAnimations : StateMachineBehaviour
{
    public const float MOVEMENT_MULTIPLIER = 0.71f;

    private GameObject kicker, goalkeeper, ball;
    private Animator goalkeeperAnimator;
    private Rigidbody body;
    private Vector3 movement;
    private bool isMoving = false;

    private void Init(Animator animator)
    {
        kicker = GameObject.FindWithTag(Constants.TAG_KICKER);
        goalkeeper = GameObject.FindWithTag(Constants.TAG_GOALKEEPER);
        ball = GameObject.FindWithTag(Constants.TAG_BALL);
        goalkeeperAnimator = goalkeeper.GetComponent<Animator>();
        body = kicker.GetComponent<Rigidbody>();
        movement = new Vector3();
        if (StaticValues.attacker != null && StaticValues.attacker.IsLeftFooted)
        {
            kicker.GetComponent<Animator>().SetBool(Constants.ANIMATOR_MIRRORED, true);
            movement.x = Constants.KICKER_OFFSET_RIGHT.x;
            movement.z = Constants.KICKER_OFFSET_RIGHT.z;
        }
        else
        {
            movement.x = Constants.KICKER_OFFSET_LEFT.x;
            movement.z = Constants.KICKER_OFFSET_LEFT.z;
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (kicker == null) Init(animator);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int hash = stateInfo.fullPathHash;
        float time = stateInfo.normalizedTime;
        if (hash == Constants.ANIMATOR_KICKER_SHOOT_STRONG)
        {
            isMoving = time - (int)time < 0.7f;
        }
        else if (hash == Constants.ANIMATOR_KICKER_SHOOT_NORMAL)
        {
            isMoving = time - (int)time < 0.8f;
        }
        else if (stateInfo.IsTag(Constants.ANIMATOR_TRIGGER_SHOOT))
        {
            isMoving = true;
        }
        if (isMoving)
        {
            if (body.position.z < Constants.PENALTY_SPOT.z)
            {
                body.MovePosition(body.position - movement * (Time.deltaTime * MOVEMENT_MULTIPLIER));
            }
            else if (animator.GetBool(Constants.ANIMATOR_TRIGGER_SHOOT))
            {
                animator.SetBool(Constants.ANIMATOR_TRIGGER_SHOOT, false);
                float x = animator.GetFloat(Constants.ANIMATOR_VELOCITY_X);
                float y = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y);
                float z = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z);
                ball.GetComponent<Rigidbody>().AddForce(new Vector3(x, y, z), ForceMode.Impulse);
                goalkeeperAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_GOALKEEP);
            }
        }
    }
}
