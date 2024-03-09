using UnityEngine;

public class BridgePart : MonoBehaviour
{
    public Bridge parentBridge;

    private float bridgeStartSpace = 5f;
    private float bridgeEndSpace = 5f;

    private float placementInterval = 8f;

    public void BridgeCompositionSet(float bridgeLength,float bridgeScale = 1)
    {
        bridgeLength -= bridgeEndSpace / bridgeScale;

        float previousPlacementEndPosZ;
        float placePosZ = bridgeStartSpace / bridgeScale;
        float placementStartPosZ;
        float placementEndPosZ;
        float nextPlacePosZ;

        float safetyLength = 1 / bridgeScale;

        placementInterval = placementInterval / bridgeScale;

        previousPlacementEndPosZ = 2 / bridgeScale;

        int count = 0;

        while (bridgeLength >= placePosZ)
        {
            Placement placement;

            int index = Random.Range(1, 100);

            if(index <= 10 && count != 0)
            {
                Placement item = ItemSpawner.Instance.GetObject();
                this.parentBridge.placementList.Add(item);
                placement = item;
            }
            else if(index <= Bridge.bridgeLoop.specialObjPer[Bridge.bridgeLoop.stage - 1] && count != 0)
            {
                Placement specialObj = SpecialObjManager.Instance.SpecialObjGet();
                this.parentBridge.placementList.Add(specialObj);
                placement = specialObj;
            }
            else
            {
                Obstacle obstacle = ObstacleManager.Instance.GetObject(BridgeLoop.Instance.stage);
                this.parentBridge.obstacleList.Add(obstacle);
                placement = obstacle;
            }

            count += 1;

            placement.transform.SetParent(this.transform);
            placePosZ += placement.length * 0.5f;
            placement.transform.localPosition = new Vector3(placement.thisPos.x, placement.thisPos.y, placePosZ);
            placement.transform.localRotation = placement.thisRotation;

            placement.transform.SetParent(null);
            placement.transform.localScale = placement.thisScale;

            placement.transform.SetParent(this.transform);


            placementStartPosZ = placePosZ - (placement.length * 0.5f / bridgeScale);
            placementEndPosZ = placePosZ + (placement.length * 0.5f / bridgeScale);

            nextPlacePosZ = placementEndPosZ + placementInterval;

            if (placementStartPosZ < bridgeLength)
            {
                AdventureCoinManager.Instance.CoinPlace(previousPlacementEndPosZ + safetyLength, (placementStartPosZ - previousPlacementEndPosZ - safetyLength) * bridgeScale, this,bridgeScale);
                placePosZ = nextPlacePosZ;
                previousPlacementEndPosZ = placementEndPosZ;
            }
            else
            {
                AdventureCoinManager.Instance.CoinPlace(previousPlacementEndPosZ + safetyLength, (placementStartPosZ - previousPlacementEndPosZ - safetyLength) * bridgeScale, this, bridgeScale);
                return;
            }
        }
    }
}