using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Placement
{
    public PlayerHealth playerHealth;
    private PlayerMovement playerMovement;

    Quaternion rightForwardRotation;
    Quaternion leftForwardRotation;

    int damage = 10;

    private float turnTime;
    private float time;

    private void Awake()
    {
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();

        rightForwardRotation = Quaternion.Euler(0, 90f, 0);
        leftForwardRotation = Quaternion.Euler(0, 270f, 0);
    }

    private void OnEnable()
    {
        time = Time.time;
        turnTime = Random.Range(3f, 5f);
        float startXPos = Random.Range(-1.5f, 1.5f);
        transform.localPosition = new Vector3(startXPos, transform.localPosition.y, transform.localPosition.z);
    }

    private void Update()
    {
        Move();
        RotateCheck();
    }


    private void Move()
    {
        if (transform.localRotation == rightForwardRotation)
        {
            transform.localPosition += Vector3.right * Time.deltaTime;
        }
        else if (transform.localRotation == leftForwardRotation)
        {
            transform.localPosition += Vector3.left * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            if(playerMovement.IsBoost == false)
            {
                playerHealth.OneTimeDamage(damage);
            }
        }
    }

    private void RotateCheck()
    {
        if (transform.localPosition.x > 1.5)
        {
            transform.localRotation = leftForwardRotation;
        }
        else if (transform.localPosition.x < -1.5)
        {
            transform.localRotation = rightForwardRotation;
        }

        if (time + turnTime < Time.time)
        {
            if (transform.localRotation == rightForwardRotation)
            {
                transform.localRotation = leftForwardRotation;
            }
            else if (transform.localRotation == leftForwardRotation)
            {
                transform.localRotation = rightForwardRotation;
            }

            time = Time.time;
            turnTime = Random.Range(3f, 5f);
        }
    }
}
