using UnityEngine;

public class KickerAnimations : MonoBehaviour
{
    const float TRANSITION = 0.25f;

    private Animator animator;
    private int shootWeakRightHash, shootHash, shootStrongHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        shootWeakRightHash = Animator.StringToHash("Base Layer.Shoot.Weak Right");
        shootHash = Animator.StringToHash("Base Layer.Shoot.Normal");
        shootStrongHash = Animator.StringToHash("Base Layer.Shoot.Strong");
    }

    public void PlayAnimation(Vector3 shootVector)
    {
        float length = shootVector.magnitude;
        if (length > 50f) animator.CrossFade(shootStrongHash, TRANSITION);
        else if (length > 10f) animator.CrossFade(shootHash, TRANSITION);
        else animator.CrossFade(shootWeakRightHash, TRANSITION);
    }
}
