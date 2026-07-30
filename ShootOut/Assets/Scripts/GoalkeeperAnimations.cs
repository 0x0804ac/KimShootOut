using UnityEngine;

[SharedBetweenAnimators]
public class GoalkeeperAnimations : StateMachineBehaviour
{
    public const float TRANSITION = 0.125f;

    [SerializeField] private GameObject goalkeeper;

    void Awake()
    {
        //
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
        //
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        //
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //
    }

    public void PlayAnimation(Goalkeeper goalkeeper, Vector3 movement)
    {
        Animator animator = this.goalkeeper.GetComponent<Animator>();
        this.goalkeeper.GetComponent<Rigidbody>().AddForce(movement);
        float x = movement.x / goalkeeper.Speed;
        Debug.Log(x);
        if (x > 0.5f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_RIGHT, TRANSITION);
        else if (x < 0.5f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_LONG_LEFT, TRANSITION);
        else if (x > 0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_SHORT_RIGHT, TRANSITION);
        else if (x < 0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_DIVE_SHORT_LEFT, TRANSITION);
        else
        {
            float y = movement.y / goalkeeper.Speed;
            if (y > 0.5f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_JUMP_MISS, TRANSITION);
            else if (y > 0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_HIGH, TRANSITION);
            else if (y > -0.25f) animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_NORMAL, TRANSITION);
            else animator.CrossFade(Constants.ANIMATOR_GOALKEEPER_CATCH_LOW, TRANSITION);
        }
    }
}
