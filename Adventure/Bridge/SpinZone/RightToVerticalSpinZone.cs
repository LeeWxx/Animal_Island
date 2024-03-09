using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightToVerticalSpinZone : SpinZone
{
    private void Awake()
    {
        SpinZoneSet();
    }

    public override void SpinZoneUpdate()
    {
        centerValue = myParentTransform.position.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            SpinZoneEnter();
        }
    }

    protected override void SpinZoneEnter()
    {
        if (!BoostCheck())
        {
            base.SpinZoneEnter();
            playerInput.spinPossibe = PlayerInput.SpinPossible.RightToVertical;
        }
        else
        {
            playerMovement.RightToVerticalCurve(centerValue,posState);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SpinZoneExit();
    }
}
