using UnityEngine;
using UnityEngine.InputSystem;

public class DefenderController: ITypeController
{
    public override ERESULT Update(PlayerController player, UserInputs.PlayerActions playerInputActions)
    {
        InputAction defendAction = playerInputActions.Action;

        if (defendAction.WasPressedThisFrame())
        {
            Debug.Log("Space pressed.");
        }

        if (defendAction.WasPerformedThisFrame())
        {
            Debug.Log("ChargeStart");
        }

        if (defendAction.WasReleasedThisFrame())
        {
            Debug.Log("Defense");
        }

        return ERESULT.Success;
    }
}
