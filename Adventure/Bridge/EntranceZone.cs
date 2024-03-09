using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceZone : MonoBehaviour
{
    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            BridgeLoop.Instance.BridgePlace();
            playerMovement.isOnBridge = true;
        }
    }
}
