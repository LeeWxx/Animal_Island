using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObstacle : Placement
{
    private BoxCollider playerCollider;
    private PlayerHealth playerHealth;
    private bool check;

    int damage = 10;
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerCollider = playerHealth.GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        check = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            playerHealth.OneTimeDamage(damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Animal"))
        {
            playerHealth.ObstacleDamage(damage);
        }
    }
}
