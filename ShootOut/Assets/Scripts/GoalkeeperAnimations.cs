using UnityEngine;

public class GoalkeeperAnimations : MonoBehaviour
{
    const float TRANSITION = 0.125f;

    [SerializeField] private GameObject goalkeeper;

    private Animator animator;
    private int idleHash, idle2Hash;
    private int sidestepLeftHash, sidestepRightHash;
    private int diveShortLeftHash, diveShortRightHash, diveLeftHash, diveLongLeftHash, diveLongRightHash;
    private int catchJumpHash, catchHighHash, catchHash, catchLowHash, catchMissHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        idleHash = Animator.StringToHash("Base Layer.Idle.Arms Side");
        idle2Hash = Animator.StringToHash("Base Layer.Idle.Arms Front");
        sidestepLeftHash = Animator.StringToHash("Base Layer.Sidestep.Left");
        sidestepRightHash = Animator.StringToHash("Base Layer.Sidestep.Right");
        diveShortLeftHash = Animator.StringToHash("Base Layer.Dive.Short Left");
        diveShortRightHash = Animator.StringToHash("Base Layer.Dive.Short Right");
        diveLeftHash = Animator.StringToHash("Base Layer.Dive.Normal Left");
        diveLongLeftHash = Animator.StringToHash("Base Layer.Dive.Long Left");
        diveLongRightHash = Animator.StringToHash("Base Layer.Dive.Long Right");
        catchJumpHash = Animator.StringToHash("Base Layer.Catch.Jump");
        catchHighHash = Animator.StringToHash("Base Layer.Catch.High");
        catchHash = Animator.StringToHash("Base Layer.Catch.Normal");
        catchLowHash = Animator.StringToHash("Base Layer.Catch.Low");
        catchMissHash = Animator.StringToHash("Base Layer.Catch.Jump Miss");
    }

    public void PlayIdleAnimation()
    {
        if (Random.Range(0f, 1f) > 0.25f) animator.Play(idleHash);
        else animator.Play(idle2Hash);
    }

    public void PlayAnimation(Goalkeeper goalkeeper, Vector3 movement)
    {
        this.goalkeeper.GetComponent<Rigidbody>().AddForce(movement);
        float x = movement.x / goalkeeper.Speed;
        print(x);
        if (x > 0.5f) animator.CrossFade(diveLongRightHash, TRANSITION);
        else if (x < 0.5f) animator.CrossFade(diveLongLeftHash, TRANSITION);
        else if (x > 0.25f) animator.CrossFade(diveShortRightHash, TRANSITION);
        else if (x < 0.25f) animator.CrossFade(diveShortLeftHash, TRANSITION);
        else
        {
            float y = movement.y / goalkeeper.Speed;
            if (y > 0.5f) animator.CrossFade(catchMissHash, TRANSITION);
            else if (y > 0.25f) animator.CrossFade(catchHighHash, TRANSITION);
            else if (y > -0.25f) animator.CrossFade(catchHash, TRANSITION);
            else animator.CrossFade(catchLowHash, TRANSITION);
        }
    }
}
