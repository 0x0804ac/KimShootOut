using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[SharedBetweenAnimators]
public class KickerAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.25f;
    public const float MOVEMENT_MULTIPLIER = 0.71f;
    public const float HIGH_POWER = 60f;
    public const float LOW_POWER = 20f;

    private GameObject kicker, goalkeeper, ball;
    private Animator goalkeeperAnimator;
    private Rigidbody body;
    private Vector3 movement;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (kicker == null) Init();
        if (animator.GetBool(Constants.ANIMATOR_TRIGGER_SHOOT))
        {
            if (stateInfo.IsTag(Constants.ANIMATOR_TRIGGER_IDLE))
            {
                float x = animator.GetFloat(Constants.ANIMATOR_VELOCITY_X);
                float y = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y);
                float z = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z);
                float length = new Vector3(x, y, z).magnitude;
                Debug.Log(length);
                if (length >= HIGH_POWER) animator.CrossFade(Constants.ANIMATOR_KICKER_SHOOT_STRONG, TRANSITION);
                else if (length >= LOW_POWER) animator.CrossFade(Constants.ANIMATOR_KICKER_SHOOT_NORMAL, TRANSITION);
                else if (StaticValues.attacker.IsLeftFooted) animator.CrossFade(Constants.ANIMATOR_KICKER_SHOOT_WEAK_LEFT, TRANSITION);
                else animator.CrossFade(Constants.ANIMATOR_KICKER_SHOOT_WEAK_RIGHT, TRANSITION);
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(Constants.ANIMATOR_TRIGGER_SHOOT) && stateInfo.IsTag(Constants.ANIMATOR_TRIGGER_SHOOT))
        {
            body.MovePosition(body.position - movement * (Time.deltaTime * MOVEMENT_MULTIPLIER));
            if (body.position.z > Constants.PENALTY_SPOT.z)
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

    private void Init()
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
}
