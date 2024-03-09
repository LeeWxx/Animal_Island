using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public Portal portalPrefab;
    Portal[] portalArray;

    private void Awake()
    {
        portalPrefab.PortalSet();
        PortalArraySet();  
    }

    private void Start()
    {
        Portal.playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void PortalArraySet()
    {
        portalArray = new Portal[AdventureManager.Instance.finalStageIndex];

        for (int i = 0; i < portalArray.Length; i++)
        {
            Portal portal = Instantiate(portalPrefab);
            portalArray[i] = portal;
            portal.gameObject.SetActive(false);
        }
    }

    public void PortalPlace(Bridge bridge)
    {
        Portal portal = portalArray[BridgeLoop.Instance.stage-1];
        portal.transform.position = bridge.bridgeEndPos;
        portal.transform.parent = bridge.bridgeParts[bridge.bridgeParts.Length - 1].transform;
        portal.gameObject.SetActive(true);
        portal.transform.localRotation = Portal.thisRotation;
        portal.transform.localScale = Portal.thisScale;
        portal.transform.localPosition = new Vector3(portal.transform.localPosition.x, portal.transform.localPosition.y-5f,
             portal.transform.localPosition.z + (BridgeLoop.Instance.minBridgeIntervalArray[BridgeLoop.Instance.stage] / bridge.transform.localScale.z));
    }
}
