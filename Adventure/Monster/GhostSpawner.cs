using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{

    private static GhostSpawner instance;

    public static GhostSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GhostSpawner>();
            }

            return instance;
        }
    }


    [SerializeField]
    Placement ghostPrefab;

    Queue<Placement> ghostQueue = new Queue<Placement>();
    int initializeCount = 10;

    Transform poolTransform;

    private void Awake()
    {
        poolTransform = this.transform;

        Initialize();
    }

    private void Initialize()
    {
        for (int k = 0; k < initializeCount; k++)
        {
            ghostQueue.Enqueue(CreateNewObject(ghostPrefab));
        }

        ghostPrefab.gameObject.SetActive(false);
    }

    private Placement CreateNewObject(Placement ghostPrefab)
    {
        Placement ghost = Instantiate(ghostPrefab);
        ghost.PlacementSet();
        ghost.transform.parent = poolTransform;
        ghost.gameObject.SetActive(false);
        return ghost;
    }

    public Placement GetObject()
    {
        if (ghostQueue.Count > 0)
        {
            Placement Placement = ghostQueue.Dequeue();
            Placement.gameObject.SetActive(true);
            return Placement;
        }
        else
        {
            Placement Placement = CreateNewObject(ghostPrefab);
            Placement.gameObject.SetActive(true);
            return Placement;
        }
    }

    public void ReturnObject(Placement returnGhost)
    {
        returnGhost.transform.SetParent(poolTransform);
        returnGhost.gameObject.SetActive(false);
        ghostQueue.Enqueue(returnGhost);
    }
}
