using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[SharedBetweenAnimators]
public class KickerAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.25f;
    public const float HIGH_POWER = 60f;
    public const float LOW_POWER = 20f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(Constants.ANIMATOR_TRIGGER_SHOOT))
        {
            if (stateInfo.IsTag(Constants.ANIMATOR_TRIGGER_IDLE))
            {
                float x = animator.GetFloat(Constants.ANIMATOR_VELOCITY_X);
                float y = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Y);
                float z = animator.GetFloat(Constants.ANIMATOR_VELOCITY_Z);
                float length = new Vector3(x, y, z).magnitude;
                Debug.Log(length);
                if (length >= HIGH_POWER) animator.CrossFadeInFixedTime(Constants.ANIMATOR_KICKER_SHOOT_STRONG, TRANSITION);
                else if (length >= LOW_POWER) animator.CrossFadeInFixedTime(Constants.ANIMATOR_KICKER_SHOOT_NORMAL, TRANSITION);
                else if (StaticValues.attacker.IsLeftFooted) animator.CrossFadeInFixedTime(Constants.ANIMATOR_KICKER_SHOOT_WEAK_LEFT, TRANSITION);
                else animator.CrossFadeInFixedTime(Constants.ANIMATOR_KICKER_SHOOT_WEAK_RIGHT, TRANSITION);
            }
        }
    }
}
