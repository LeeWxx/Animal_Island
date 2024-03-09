using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBridge : Bridge
{
    public override Vector3 BridgeSet(Vector3 lastBridgePos, float bridgeInterval)
    {
        bridgeScale = Random.Range(minBridgeScale, maxBridgeScale);

        bridgePos.x = lastBridgePos.x;
        bridgePos.y = lastBridgePos.y;
        bridgePos.z = lastBridgePos.z + bridgeInterval;
       
        gameObject.transform.localScale = new Vector3(1, 1, bridgeScale);
        gameObject.transform.position = bridgePos;

        boostBridge.transform.localScale = new Vector3(1, 1, (bridgeInterval / 10) / bridgeScale);

        for (int i = 0; i < bridgeParts.Length; i++)
        {
            bridgeParts[i].BridgeCompositionSet(bridgeLength,bridgeScale);
        }

        thisTotalBridgeLength = (bridgeLength * bridgeScale);

        bridgeEndPos.x = bridgePos.x;
        bridgeEndPos.y = bridgePos.y;
        bridgeEndPos.z = bridgePos.z + thisTotalBridgeLength;

        return bridgeEndPos;
    }
}
