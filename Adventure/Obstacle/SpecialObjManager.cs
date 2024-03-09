using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObjManager : MonoBehaviour
{
    private static SpecialObjManager instance;

    public static SpecialObjManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpecialObjManager>();
            }

            return instance;
        }
    }

    public ArcherSpawner archerSpawner;
    public GhostSpawner ghostSpawner;
    public ParticleObstacleSpawner particleObstacleSpawner;

    public Placement SpecialObjGet()
    {
        switch (BridgeLoop.Instance.stage)
        {
            case 1:
                return archerSpawner.GetObject(1);
            case 2:
                return archerSpawner.GetObject(2);
            case 3:
                int num1 = Random.Range(1, 101);
                if(num1 < 50)
                {
                    return archerSpawner.GetObject(3);
                }
                else
                {
                    return ghostSpawner.GetObject();
                }
            case 4:
                int num2 = Random.Range(1, 101);
                if (num2 < 50)
                {
                    return archerSpawner.GetObject(4);
                }
                else
                {
                    return particleObstacleSpawner.GetObject();
                }
            default:
                return null;
        }
    }
}
