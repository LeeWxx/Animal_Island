using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObstacleSpawner : MonoBehaviour
{
    private static ParticleObstacleSpawner instance;

    public static ParticleObstacleSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ParticleObstacleSpawner>();
            }

            return instance;
        }
    }


    [SerializeField]
    Placement[] volcanoParticleObstacle;

    Queue<Placement> volcanoParticleObstacleQueue = new Queue<Placement>();

    int initializeCount = 10;

    Transform poolTransform;

    private void Awake()
    {
        poolTransform = this.transform;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < volcanoParticleObstacle.Length; i++)
        {
            for (int k = 0; k < initializeCount; k++)
            {
                volcanoParticleObstacleQueue.Enqueue(CreateNewObject(volcanoParticleObstacle[i]));
            }

            volcanoParticleObstacle[i].gameObject.SetActive(false);
        }

        Utill.SuffleQueue<Placement>(volcanoParticleObstacleQueue);
    }

    private Placement CreateNewObject(Placement particleObstaclePrefab)
    {
        Placement particleObstacle = Instantiate(particleObstaclePrefab);
        particleObstacle.PlacementSet();
        particleObstacle.transform.parent = poolTransform;
        particleObstacle.gameObject.SetActive(false);
        return particleObstacle;
    }

    public Placement GetObject()
    {
        if (volcanoParticleObstacleQueue.Count > 0)
        {
            Placement Placement = volcanoParticleObstacleQueue.Dequeue();
            Placement.gameObject.SetActive(true);
            return Placement;
        }
        else
        {
            int num = Random.Range(0, volcanoParticleObstacle.Length);
            Placement Placement = CreateNewObject(volcanoParticleObstacle[num]);
            Placement.gameObject.SetActive(true);
            return Placement;
        }
    }

    public void ReturnObject(Placement returnParticleObstacle)
    {
        returnParticleObstacle.transform.SetParent(poolTransform);
        returnParticleObstacle.gameObject.SetActive(false);
        volcanoParticleObstacleQueue.Enqueue(returnParticleObstacle);
    }
}
