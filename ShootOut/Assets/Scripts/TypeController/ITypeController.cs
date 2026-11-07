using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class ITypeController
{
    protected bool isAimConfirm = false;


    public virtual EResult Update(PlayerController player, UserInputs.PlayerActions playerInputActions)
    {
        return EResult.NotDefined;
    }

    public bool IsAimComfirmed()
    {
        return isAimConfirm;
    }
}
