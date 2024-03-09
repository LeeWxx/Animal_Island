using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveDeathZone : MonoBehaviour
{

    PlayerHealth playerHealth;
    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Animal"))
        {
            //StartCoroutine(FindObjectOfType<PlayerHealth>().Die());
            playerHealth.ObstacleDamage(10);
        }
    }
}
