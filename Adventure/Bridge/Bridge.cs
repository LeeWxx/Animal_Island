using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bridge : MonoBehaviour
{
    public enum BridgeType { Vertical, RightCurve, LeftCurve, RightToVertical, LeftToVertical, RightHorizontal, LeftHorizontal };
    public BridgeType bridgeType;

    public int thisBridgeStage;

    protected Vector3 bridgePos = new Vector3();
    public Vector3 bridgeEndPos = new Vector3();

    public List<Obstacle> obstacleList = new List<Obstacle>();
    public List<AdventureCoinBundle> coinBundleList = new List<AdventureCoinBundle>();
    public List<Placement> placementList = new List<Placement>();

    public static BridgeLoop bridgeLoop;

    protected int bridgeScale = 1;
    protected int minBridgeScale = 1;
    protected int maxBridgeScale = 4;

    protected float bridgeLength = 50;

    public float curveSpace = 2.5f;

    public SpinZone[] spinZones;
    public BridgePart[] bridgeParts;

    public BoostBridge boostBridge;

    public static ObstacleManager obstacleManager;
    public static AdventureCoinManager adventureCoinManager;
    public static ItemSpawner itemSpawner;
    public static ArcherSpawner archerSpawner;
    public static GhostSpawner ghostSpawner;
    public static ParticleObstacleSpawner particleObstacleSpawner;

    public float thisTotalBridgeLength;

    public GameObject[] Terrains;

    private List<MeshRenderer> terrainMesh = new List<MeshRenderer>();
    public List<MeshRenderer> bridgeMesh = new List<MeshRenderer>();


    public void MaterialSet()
    {
        MeshTerrain[] meshTerrains = GetComponentsInChildren<MeshTerrain>();
        MeshBridge[] meshBridges = GetComponentsInChildren<MeshBridge>();

        for(int i =0; i<meshTerrains.Length; i++)
        {
           terrainMesh.AddRange(meshTerrains[i].GetComponentsInChildren<MeshRenderer>());
        }

        for (int i = 0; i < meshBridges.Length; i++)
        {
            bridgeMesh.AddRange(meshBridges[i].GetComponentsInChildren<MeshRenderer>());
        }
    }

    public void BoostBridgeSet()
    {
        boostBridge = GetComponentInChildren<BoostBridge>();
    }

    public void ReturnPool()
    {
        for (int i = 0; i < obstacleList.Count; i++)
        {
            obstacleManager.ReturnObject(obstacleList[i]);
        }
        obstacleList.RemoveAll(x => true);

        for (int i = 0; i < coinBundleList.Count; i++)
        {
            adventureCoinManager.ReturnObject(coinBundleList[i]);
        }
        coinBundleList.RemoveAll(x => true);

        for (int i = 0; i < placementList.Count; i++)
        {
            if (placementList[i].CompareTag("Item"))
            {
                itemSpawner.ReturnObject(placementList[i]);
            }
            else if (placementList[i].CompareTag("Archer"))
            {
                if (placementList[i].gameObject.activeSelf == true)
                {
                    archerSpawner.ReturnObject(placementList[i].GetComponent<Archer>());
                }
            }
            else if (placementList[i].CompareTag("Ghost"))
            {
                ghostSpawner.ReturnObject(placementList[i]);
            }
            else if (placementList[i].CompareTag("ParticleObstacle"))
            {
                particleObstacleSpawner.ReturnObject(placementList[i]);
            }
        }
        placementList.RemoveAll(x => true);

        bridgeLoop.ReturnObject(this);
    }

    public abstract Vector3 BridgeSet(Vector3 lastBridgePos, float bridgeInterval); 

    protected void SpinZoneFind()
    {
        spinZones = GetComponentsInChildren<SpinZone>();
    }

    protected void SpinZoneUpdate()
    {
        for(int i =0; i<spinZones.Length; i++)
        {
            spinZones[i].SpinZoneUpdate();
        }
    }

    public void MaterialChange(Material bridgeMaterial, Material terrainMaterial)
    {
        for (int i = 0; i < bridgeMesh.Count; i++)
        {
            bridgeMesh[i].material = bridgeMaterial;
        }
        for (int i = 0; i < terrainMesh.Count; i++)
        {
            terrainMesh[i].material = terrainMaterial;
        }
    }
}
