using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHorizontalBridge : Bridge
{
    public override Vector3 BridgeSet(Vector3 lastBridgePos, float bridgeInterval)
    {

        bridgeScale = Random.Range(minBridgeScale, maxBridgeScale);

        bridgePos.y = lastBridgePos.y;
        bridgePos.z = lastBridgePos.z;
        bridgePos.x = lastBridgePos.x - bridgeInterval;
        gameObject.transform.localScale = new Vector3(1, 1, bridgeScale);

        boostBridge.transform.localScale = new Vector3(1, 1, (bridgeInterval / 10) / bridgeScale);

        gameObject.transform.position = bridgePos;

        for (int i = 0; i < bridgeParts.Length; i++)
        {
            bridgeParts[i].BridgeCompositionSet(bridgeLength,bridgeScale);
        }

        thisTotalBridgeLength = (bridgeLength * bridgeScale);

        bridgeEndPos.x = bridgePos.x - thisTotalBridgeLength;
        bridgeEndPos.y = bridgePos.y;
        bridgeEndPos.z = bridgePos.z;

        return bridgeEndPos;
    }
}
