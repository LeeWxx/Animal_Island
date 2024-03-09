using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpinZone : MonoBehaviour
{
    public float centerValue;

    public static PlayerInput playerInput;
    public static PlayerMovement playerMovement;

    public PlayerMoving.PosState posState;

    protected Transform myParentTransform;

    protected void SpinZoneSet()
    {
        myParentTransform = transform.parent.transform;
    }

    public abstract void SpinZoneUpdate();

    protected virtual void SpinZoneEnter()
    { 
        playerInput.spinPosState = posState;
        playerInput.spinCenterValue = centerValue;
        playerMovement.SpinSpeedDecline();
    }

    protected virtual void SpinZoneExit()
    {
        playerMovement.SpeedSet();
    }

    protected virtual bool BoostCheck()
    {
        if(playerMovement.IsBoost == true)
        {
           return true;
        }
        else
        {
            return false;
        }
    }
}
