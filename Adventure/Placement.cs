using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public Vector3 thisPos;
    public Vector3 thisScale;
    public Quaternion thisRotation;

    public float length;

    public void PlacementSet()
    {
        thisPos = transform.position;
        thisScale = transform.lossyScale;
        thisRotation = transform.rotation;
    }
}
