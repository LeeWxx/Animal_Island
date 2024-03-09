using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeEndZone : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerInput playerInput;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
           if(playerMovement.jumping == false && playerMovement.IsBoost == false)
           {
                playerInput.jumpPossible = false;
           }

           playerMovement.isOnBridge = false;

            AdventureManager.Instance.LastExitZoneUpdate(this);
        }
    }
}
