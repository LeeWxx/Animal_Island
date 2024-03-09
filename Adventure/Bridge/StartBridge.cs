using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBridge : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Animal"))
        {
            StartCoroutine(PosYCheck(collision.collider.GetComponent<PlayerMovement>()));
        }
    }

    private IEnumerator PosYCheck(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(0.01f);
        playerMovement.animalPosY = playerMovement.gameObject.transform.position.y;
    }
}
