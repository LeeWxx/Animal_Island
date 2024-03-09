using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private static ItemSpawner instance;

    public static ItemSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemSpawner>();
            }

            return instance;
        }
    }

    public Placement[] itemPrefabArray;

    Queue<Placement> itemQueue = new Queue<Placement>();

    int poolCount = 100;

    Transform poolTransform;

    private void Awake()
    {
        poolTransform = this.transform;
        Initialize();
    }

    private void Start()
    {
        Item.playerItemController = FindObjectOfType<PlayerItemController>();
    }

    private void Initialize()
    {
        for (int i = 0; i < poolCount; i++)
        {
            itemQueue.Enqueue(CreateNewObject());
        }
    }

    private Placement CreateNewObject()
    {
        int index = Random.Range(0, itemPrefabArray.Length);
        Placement Placement = Instantiate(itemPrefabArray[index]);
        Placement.PlacementSet();
        Placement.gameObject.SetActive(false);
        Placement.transform.SetParent(poolTransform);
        return Placement;
    }

    public Placement GetObject()
    {
        Placement getItem;

        if (itemQueue.Count != 0)
        {
            getItem = itemQueue.Dequeue();
        }
        else
        {
            getItem = CreateNewObject();
        }

        getItem.gameObject.SetActive(true);

        return getItem;
    }

    public void ReturnObject(Placement returnItem)
    {
        itemQueue.Enqueue(returnItem);
        returnItem.gameObject.SetActive(false);
        returnItem.transform.SetParent(poolTransform);
    }
}
