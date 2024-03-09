using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyDome : MonoBehaviour
{
    public float rotateAngle;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotateAngle);
    }

    public void XMove(float moveXPos)
    {
        transform.position = new Vector3(moveXPos,transform.position.y,transform.position.z);
    }
}
