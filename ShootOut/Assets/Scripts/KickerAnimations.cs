using UnityEngine;

public class KickerAnimations : MonoBehaviour
{
    const float TRANSITION = 0.25f;

    [SerializeField] private GameObject kicker;

    private Animator animator;
    private Rigidbody body;
    private Vector3 movement;
    private int idleHash, shootWeakRightHash, shootHash, shootStrongHash;
    private bool isMoving;

    void Start()
    {
        animator = GetComponent<Animator>();
        body = kicker.GetComponent<Rigidbody>();
        movement = new Vector3(Constants.KICKER_OFFSET_LEFT.x, 0f, Constants.KICKER_OFFSET_LEFT.z);
        idleHash = Animator.StringToHash("Base Layer.Idle");
        shootWeakRightHash = Animator.StringToHash("Base Layer.Shoot.Weak Right");
        shootHash = Animator.StringToHash("Base Layer.Shoot.Normal");
        shootStrongHash = Animator.StringToHash("Base Layer.Shoot.Strong");
        isMoving = false;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            body.MovePosition(body.position + movement * Time.fixedDeltaTime);
            if (body.position.z > Constants.PENALTY_SPOT.z)
            {
                isMoving = false;
            }
        }
    }

    public void PlayIdleAnimation()
    {
        animator.Play(idleHash);
    }

    public void PlayKickAnimation(Vector3 shootVector)
    {
        isMoving = true;
        float length = shootVector.magnitude;
        if (length > 50f) animator.CrossFade(shootStrongHash, TRANSITION);
        else if (length > 10f) animator.CrossFade(shootHash, TRANSITION);
        else animator.CrossFade(shootWeakRightHash, TRANSITION);
    }
}
