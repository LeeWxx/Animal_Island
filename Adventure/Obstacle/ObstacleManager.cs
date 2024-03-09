using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private static ObstacleManager instance;

    public static ObstacleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObstacleManager>();
            }

            return instance;
        }
    }

    public Queue<Obstacle>[] stageObstacleQueue; //카운트의 수 만큼 넣어주는 곳
    public List<Obstacle>[] stageObstacleList; //하나씩만 프리팹 원본 넣어준다.

    private int finalStageIndex;
    int count = 200;

    public Transform poolTransform;

    private void Awake()
    {
        finalStageIndex = AdventureManager.Instance.finalStageIndex;

        poolTransform = this.transform;

        ObstacleDataSet();
    }

    private void Start()
    {
        Obstacle.playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void ObstacleDataSet()
    {
        stageObstacleQueue = new Queue<Obstacle>[finalStageIndex];
        stageObstacleList = new List<Obstacle>[finalStageIndex];

        for (int i = 0; i < finalStageIndex; i++)
        {
            stageObstacleList[i] = new List<Obstacle>();
            stageObstacleQueue[i] = new Queue<Obstacle>();
        }

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        for (int i = 0; i < obstacles.Length; i++)
        {
            if (obstacles[i].stage != 0)
            {
                stageObstacleList[obstacles[i].stage - 1].Add(obstacles[i]);
                obstacles[i].PlacementSet();
                obstacles[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < finalStageIndex; i++)
        {
            Initialize(count, i);
        }
    }

    private void Initialize(int initCount, int stage)
    {
        for (int i = 0; i < initCount; i++)
        {
            int randomNumber = Random.Range(0, stageObstacleList[stage].Count);
            Obstacle newObj = CreateNewObject(stageObstacleList[stage][randomNumber]);
            stageObstacleQueue[stage].Enqueue(newObj);
        }
    }

    private Obstacle CreateNewObject(Obstacle obstaclePrefab)
    {
        Obstacle newObstacle = Instantiate(obstaclePrefab);
        Utill.SetLayerRecursively(newObstacle.transform, "Obstacle");
        newObstacle.transform.SetParent(poolTransform);
        newObstacle.gameObject.SetActive(false);

        return newObstacle;
    }

    public Obstacle GetObject(int stage)
    {
        if (stageObstacleQueue[stage-1].Count > 0)
        {
            Obstacle obj = stageObstacleQueue[stage-1].Dequeue();

            obj.gameObject.SetActive(true);

            return obj;
        }

        else
        {
            int randomNumber = Random.Range(0, stageObstacleList[stage-1].Count);
            Obstacle newObj = CreateNewObject(stageObstacleList[stage-1][randomNumber]);

            newObj.gameObject.SetActive(true);

            return newObj;
        }
    }

    public void ReturnObject(Obstacle returnObstacle)
    {
        stageObstacleQueue[returnObstacle.stage - 1].Enqueue(returnObstacle);
        returnObstacle.gameObject.transform.SetParent(this.transform);
        returnObstacle.gameObject.SetActive(false);
    }
}
