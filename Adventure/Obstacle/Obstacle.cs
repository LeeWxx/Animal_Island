using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Placement
{
    public int stage;

    public static PlayerHealth playerHealth;
    private int damage = 10;

    bool check;

    private void OnEnable()
    {
        check = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(check == false)
        {
            if (collision.collider.CompareTag("Animal"))
            {
                playerHealth.ObstacleDamage(damage);
            }
        }
    }
}
