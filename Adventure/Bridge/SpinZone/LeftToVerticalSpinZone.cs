using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftToVerticalSpinZone : SpinZone
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
            playerInput.spinPossibe = PlayerInput.SpinPossible.LeftToVertical;
        }
        else
        {
            playerMovement.LeftToVerticalCurve(centerValue, posState);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SpinZoneExit();
    }
}
