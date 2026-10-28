using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class ITypeController
{
    protected bool isAimConfirm = false;

    public virtual ERESULT Update(PlayerController player, UserInputs.PlayerActions playerInputActions)
    {
        return ERESULT.NOT_DEFINED;
    }

    public bool IsAimComfirmed()
    {
        return isAimConfirm;
    }
}
