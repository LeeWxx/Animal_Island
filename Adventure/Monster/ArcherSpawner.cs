using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpawner : MonoBehaviour
{
    private static ArcherSpawner instance;

    public static ArcherSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ArcherSpawner>();
            }

            return instance;
        }
    }

    [SerializeField]
    Archer[] archerArray;

    List<List<Archer>> archerPrefabList = new List<List<Archer>>();
    List<Queue<Archer>> archerQueueList = new List<Queue<Archer>>();

    int initializeCount = 10;

    Transform poolTransform;

    int archerKindCount;

    private void Awake()
    {
        archerKindCount = AdventureManager.Instance.finalStageIndex;
        poolTransform = this.transform;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for(int i =0;i<archerKindCount; i++)
        {
            archerQueueList.Add(new Queue<Archer>());
            archerPrefabList.Add(new List<Archer>());
        }

        for(int i =0; i < archerArray.Length; i++)
        {
            for(int k = 0; k < initializeCount; k++)
            {
                Archer archer = CreateNewObject(archerArray[i]);
                archerQueueList[archerArray[i].stage-1].Enqueue(archer);
            }

            archerPrefabList[archerArray[i].stage - 1].Add(archerArray[i]);
            archerArray[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < archerKindCount; i++)
        {
            Utill.SuffleQueue<Archer>(archerQueueList[i]);
        }
    }

    private Archer CreateNewObject(Archer archerPrefab)
    {
        Archer archer  = Instantiate(archerPrefab);
        Utill.SetLayerRecursively(archer.transform, "Obstacle");
        archer.PlacementSet();
        archer.transform.parent= poolTransform;
        archer.gameObject.SetActive(false);
        return archer;
    }

    public Archer GetObject(int stage)
    {
        if (archerQueueList[stage-1].Count > 0)
        {
            Archer Archer = archerQueueList[stage - 1].Dequeue();
            Archer.gameObject.SetActive(true);
            return Archer;
        }
        else
        {
            int num = Random.Range(0, archerPrefabList[stage - 1].Count);
            Archer Archer = CreateNewObject(archerPrefabList[stage - 1][num]);
            Archer.gameObject.SetActive(true);
            return Archer;
        }
    }

    public void ReturnObject(Archer returnArcher)
    {
        returnArcher.transform.SetParent(poolTransform);
        returnArcher.gameObject.SetActive(false);
        archerQueueList[returnArcher.stage-1].Enqueue(returnArcher);
    }
}
