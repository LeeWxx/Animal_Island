using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCurveBridge : Bridge
{
    private void Awake()
    {
        SpinZoneFind();
        thisTotalBridgeLength = bridgeLength * 2;
    }

    public override Vector3 BridgeSet(Vector3 lastBridgePos, float bridgeInterval)
    {
        bridgePos.x = lastBridgePos.x;
        bridgePos.y = lastBridgePos.y;
        bridgePos.z = lastBridgePos.z + bridgeInterval;

        boostBridge.transform.localScale = new Vector3(1, 1, (bridgeInterval / 10) / bridgeScale);

        gameObject.transform.position = bridgePos;
        SpinZoneUpdate();

        bridgeEndPos.x = bridgePos.x - bridgeLength + curveSpace ;
        bridgeEndPos.y = bridgePos.y;
        bridgeEndPos.z = bridgePos.z + bridgeLength + curveSpace;

        for(int i =0; i<bridgeParts.Length; i++)
        {
            bridgeParts[i].BridgeCompositionSet(bridgeLength);
        }

        return bridgeEndPos;
    }
}
