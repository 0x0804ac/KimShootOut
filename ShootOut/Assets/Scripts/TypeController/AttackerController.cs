using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class AttackerController: ITypeController
{
    float power = 0f;
    RangeVariable<float> powerRangeVar = new RangeVariable<float>(0f, 100f);
    
    [SerializeField]
    int chargePowerPerSecond = 100;
    sbyte sign = 1;

    bool isCharging = false;

    [SerializeField]
    Image chargeGuageImage;

    [SerializeField]
    Rigidbody ballRigidbody;

    [SerializeField]
    Camera gameCamera;

    [SerializeField]
    BoxCollider goalCollider;

    public override ERESULT Update(PlayerController player, UserInputs.PlayerActions playerInputActions)
    {
        CheckAimComfirmed(playerInputActions);

        if (IsAimComfirmed())
        {
            ShootAction(player, playerInputActions.Action);
        }

        return ERESULT.Success;
    }

    private void ShootAction(PlayerController player, InputAction action)
    {
        if (action.WasPerformedThisFrame())
        {
            isCharging = true;
        }

        if (action.IsPressed() && isCharging)
        {
            power += chargePowerPerSecond * Time.deltaTime * sign;

            if (power > powerRangeVar.max)
            {
                power = powerRangeVar.max;
                sign *= -1;
            }

            else if (power < powerRangeVar.min)
            {
                power = powerRangeVar.min;
                sign *= -1;
            }
        }

        if (action.WasReleasedThisFrame())
        {
            Vector2 lastAimPosition = player.GetLastAimPosition();
            Ray ray = gameCamera.ScreenPointToRay(lastAimPosition);

            goalCollider.Raycast(ray, out RaycastHit hit, 1000f);

            Debug.Log("hit point: " + hit.point);

            Vector3 direction = (hit.point - ballRigidbody.position).normalized;

            ballRigidbody.AddForce(
                direction * power,
                ForceMode.Impulse
                );

            power = 0f;
            isCharging = false;
            Debug.Log("Shoot");
        }

        chargeGuageImage.fillAmount = Mathf.Clamp01(power / powerRangeVar.max);
    }

    private void CheckAimComfirmed(UserInputs.PlayerActions playerInputActions)
    {
        InputAction aimComfirm = playerInputActions.AimConfirm;

        if (aimComfirm.WasPerformedThisFrame())
        {
            isAimConfirm = !isAimConfirm;
            Debug.Log("aimConfirm: " + isAimConfirm);
        }
    }
}
