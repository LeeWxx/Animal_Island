using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLoop : MonoBehaviour
{
    private static BridgeLoop instance;

    public static BridgeLoop Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BridgeLoop>();
            }

            return instance;
        }
    }

    public Bridge[] bridgePrefabArray;

    public Dictionary<Bridge.BridgeType, Bridge> bridgePrefabDic = new Dictionary<Bridge.BridgeType, Bridge>();

    private Dictionary<Bridge.BridgeType, Queue<Bridge>> bridgeDic = new Dictionary<Bridge.BridgeType, Queue<Bridge>>();
    private List<Bridge> bridgeFieldList;

    public int poolCount = 10;
    public int fieldBridgeCount = 2;

    public Vector3 lastBridgePos = new Vector3(0, 0, 50);
    private Bridge.BridgeType lastBridgeType;

    public int[] maxBridgeIntervalArray;
    public int[] minBridgeIntervalArray;
    private int bridgeInterval;

    public PortalSpawner portalSpawner;
    public static float totalBridgeLength;

    public int stage;
    private int[] stageNeedLength;

    public Material[] bridgeMaterials;
    public Material[] terrainMaterials;

    public int[] specialObjPer;

    PlayerMovement playerMovement;

    private void Awake()
    {
        maxBridgeIntervalArray = new int[AdventureManager.Instance.finalStageIndex];
        minBridgeIntervalArray = new int[AdventureManager.Instance.finalStageIndex];

        for(int i =0; i< AdventureManager.Instance.finalStageIndex; i++)
        {
            minBridgeIntervalArray[i] = 8 + (i);
            maxBridgeIntervalArray[i] = 10 + (i);
        }

        stage = 1;

        totalBridgeLength = 0;
        stageNeedLength = new int[AdventureManager.Instance.finalStageIndex];

        for (int i = 0; i < stageNeedLength.Length; i++)
        {
            stageNeedLength[i] = i * 1000;
        }


        if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        bridgeFieldList = new List<Bridge>();
        lastBridgeType = Bridge.BridgeType.Vertical;

        Bridge.obstacleManager = ObstacleManager.Instance;
        Bridge.adventureCoinManager = AdventureCoinManager.Instance;
        Bridge.itemSpawner = ItemSpawner.Instance;
        Bridge.archerSpawner = ArcherSpawner.Instance;
        Bridge.ghostSpawner = GhostSpawner.Instance;
        Bridge.particleObstacleSpawner = ParticleObstacleSpawner.Instance;

        Bridge.bridgeLoop = this;

        BridgeDictionarySet();
    }

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        for (int i = 0; i < fieldBridgeCount; i++)
        {
            BridgePlace();
        }
    }

    void BridgeDictionarySet()
    {
        for (int i = 0; i < bridgePrefabArray.Length; i++)
        {
            bridgePrefabDic.Add(bridgePrefabArray[i].bridgeType, bridgePrefabArray[i]);
            bridgeDic.Add(bridgePrefabArray[i].bridgeType, new Queue<Bridge>());
            bridgePrefabArray[i].gameObject.SetActive(false);

            for (int k = 0; k < poolCount; k++)
            {
                Initialize(bridgePrefabArray[i].bridgeType);
            }
        }
    }

    private void Initialize(Bridge.BridgeType bridgeType)
    {
        if (bridgeDic.TryGetValue(bridgeType, out Queue<Bridge> bridgeQueue))
        {
            bridgeQueue.Enqueue(CreateNewObject(bridgeType));
        }
    }

    private Bridge CreateNewObject(Bridge.BridgeType bridgeType)
    {
        if (bridgePrefabDic.TryGetValue(bridgeType, out Bridge bridgePrefab))
        {
            Bridge newBridge = GameObject.Instantiate(bridgePrefab);
            newBridge.MaterialSet();
            newBridge.BoostBridgeSet();
            newBridge.transform.SetParent(this.transform);
            return newBridge;
        }

        return null;
    }

    private Bridge GetObject(Bridge.BridgeType bridgeType)
    {
        Bridge newBridge;

        if (bridgeDic.TryGetValue(bridgeType, out Queue<Bridge> bridgeQueue))
        {
            if (bridgeQueue.Count > 0)
            {
                newBridge = bridgeQueue.Dequeue();
                newBridge.transform.SetParent(null);
                return newBridge;
            }
            else
            {
                newBridge = CreateNewObject(bridgeType);
                newBridge.transform.SetParent(null);
                return newBridge;
            }
        }

        
        return null;
    }

    public void ReturnObject(Bridge returnBridge)
    {
        if (bridgeDic.TryGetValue(returnBridge.bridgeType, out Queue<Bridge> bridgeQueue))
        {
            bridgeQueue.Enqueue(returnBridge);
            returnBridge.transform.SetParent(this.transform);
            returnBridge.gameObject.SetActive(false);
        }
    }

    private Bridge.BridgeType BridgeTypeSelect()
    {
        if (lastBridgeType == Bridge.BridgeType.Vertical)
        {
            int randomNum = Random.Range(1, 100);

            if (randomNum <= 60)
            {
                 return Bridge.BridgeType.Vertical;
            }
            else if (randomNum <= 80)
            {
                return Bridge.BridgeType.LeftCurve;
            }
            else
            {
                return Bridge.BridgeType.RightCurve;
            }
        }
        else if (lastBridgeType == Bridge.BridgeType.RightCurve)
        {
            return Bridge.BridgeType.RightHorizontal;
        }
        else if (lastBridgeType == Bridge.BridgeType.LeftCurve)
        {
            return Bridge.BridgeType.LeftHorizontal;
        }
        else if (lastBridgeType == Bridge.BridgeType.RightHorizontal)
        {
            int randomNum = Random.Range(1, 100);

            if (randomNum <= 50)
            {
                return Bridge.BridgeType.RightToVertical;
            }
            else
            {
                return Bridge.BridgeType.RightHorizontal;
            }
        }
        else if (lastBridgeType == Bridge.BridgeType.LeftHorizontal)
        {
            int randomNum = Random.Range(1, 100);

            if (randomNum <= 50)
            {
                return Bridge.BridgeType.LeftToVertical;
            }
            else
            {
                return Bridge.BridgeType.LeftHorizontal;
            }
        }
        else
        {
            return Bridge.BridgeType.Vertical;
        }
    }

    public void BridgePlace()
    {
        bridgeInterval = Random.Range(minBridgeIntervalArray[stage -1], maxBridgeIntervalArray[stage - 1]);

        lastBridgeType = BridgeTypeSelect();
        Bridge placeBridge = GetObject(lastBridgeType);

        placeBridge.gameObject.SetActive(true);
        lastBridgePos = placeBridge.BridgeSet(lastBridgePos, bridgeInterval);

        bridgeFieldList.Add(placeBridge);

        if (playerMovement.IsBoost == true)
        {
            placeBridge.boostBridge.gameObject.SetActive(true);
        }
        else
        {
            placeBridge.boostBridge.gameObject.SetActive(false);
        }

        BridgePlaceCheck();

        placeBridge.thisBridgeStage = stage;
        placeBridge.MaterialChange(bridgeMaterials[stage - 1], terrainMaterials[stage - 1]);
        if (placeBridge.thisBridgeStage > PlayerMovement.movingStage)
        {
            placeBridge.gameObject.SetActive(false);
        }

        if (stage != stageNeedLength.Length)
        {
            StageCheck(placeBridge);
        }
    }

    public void StageCheck(Bridge bridge)
    {
        totalBridgeLength += bridge.thisTotalBridgeLength;

        if (stageNeedLength[stage] <= totalBridgeLength)
        {
            portalSpawner.PortalPlace(bridge);
            stage += 1;
        }
    }


    public void BridgePlaceCheck()
    {
        if (bridgeFieldList.Count >= fieldBridgeCount + 4)
        {
            if(bridgeFieldList[0].boostBridge.gameObject.activeSelf == true)
            {
                bridgeFieldList[0].boostBridge.gameObject.SetActive(false);
            }
            bridgeFieldList[0].ReturnPool();
            bridgeFieldList.RemoveAt(0);
        }
    }

    public void StageUp()
    {
        for(int i=0; i< bridgeFieldList.Count; i++)
        {
           if(bridgeFieldList[i].thisBridgeStage == PlayerMovement.movingStage)
           {
                bridgeFieldList[i].gameObject.SetActive(true);
           }
        }
    }

    public void BoostBridgeOn()
    {
        for(int i =0; i<bridgeFieldList.Count; i++)
        {
            bridgeFieldList[i].boostBridge.gameObject.SetActive(true);
        }
    }

    public void BoostBridgeOff()
    {
        for (int i = 0; i < bridgeFieldList.Count; i++)
        {
            bridgeFieldList[i].boostBridge.gameObject.SetActive(false);
        }
    }
}
