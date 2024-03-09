using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public float islandPosX;

    public int islandNum;

    private bool isSpawn = false;

    public bool IsSpawn
    {
        get { return isSpawn; }
        set { isSpawn = value; }
    }

    private int needPhase;
    
    public int NeedPhase
    {
        get { return needPhase; }
        set { needPhase = value; }
    }
    private int price;

    public int Price
    {
        get { return price; }
        set { price = value; }
    }

    public Vector3 GetFirstDestination()
    {
        return  NavmeshUtil.GetRandomPoint(transform.position, 10f);
    }

    public Vector3 GetDestination(Vector3 animalPos)
    {
        return NavmeshUtil.GetRandomPoint(animalPos, 10f);
    }


}
