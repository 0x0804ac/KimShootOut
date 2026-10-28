using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class AttackerController: ITypeController
{
    float power = 0f;
    RangeVariable<float> minMaxPower = new RangeVariable<float>(0f, 100f);
    
    [SerializeField]
    int chargePowerPerSecond = 10;
    sbyte sign = 1;

    bool isCharging = false;

    public override ERESULT Update(PlayerController player, UserInputs.PlayerActions playerInputActions)
    {
        InputAction shootAction = playerInputActions.Action;

        if (shootAction.WasPerformedThisFrame())
        {
            isCharging = true;
            Debug.Log("ChargeStart");
        }

        if (shootAction.IsPressed() && isCharging)
        {
            power += chargePowerPerSecond * Time.deltaTime * sign;

            if (power > minMaxPower.max)
            {
                power = minMaxPower.max;
                sign *= -1;
            }

            else if (power < minMaxPower.min)
            {
                power = minMaxPower.min;
                sign *= -1;
            }

            Debug.Log("power: " + power);
        }

        if (shootAction.WasReleasedThisFrame())
        {
            power = 0f;
            isCharging = false;
            Debug.Log("Shoot");
        }

        CheckAimComfirmed(playerInputActions);

        return ERESULT.SUCCESS;
    }

    private void CheckAimComfirmed(UserInputs.PlayerActions playerInputActions)
    {
        InputAction aimComfirm = playerInputActions.AimConfirm;

        if (aimComfirm.WasPerformedThisFrame())
        {
            isAimConfirm = !isAimConfirm;
        }
    }
}
