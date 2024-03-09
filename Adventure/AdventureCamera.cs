using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureCamera : MonoBehaviour
{
   public void CameraSet(Vector3 pos, Quaternion rotation,Transform animalTransform)
   {
        this.transform.SetParent(null);
        this.transform.position = pos;
        this.transform.SetParent(animalTransform);
        this.transform.localRotation = rotation;
   }
}
