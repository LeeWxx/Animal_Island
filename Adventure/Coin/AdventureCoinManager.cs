using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureCoinManager : MonoBehaviour
{
    private static AdventureCoinManager instance;

    public static AdventureCoinManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdventureCoinManager>();
            }

            return instance;
        }
    }
    public AdventureCoinBundle[] adventureCoinBundles;
    private int coinBundlesLowestLength = 4;
    private int coinBundlesHighestLength = 8;
    private Dictionary<int, Queue<AdventureCoinBundle>> adventureCoinBundleDic = new Dictionary<int, Queue<AdventureCoinBundle>>();

    public int poolCount;

    private Transform poolTransform;
    private Quaternion zeroQuaternion;

    public GameObject coinPrefab;
    private float rotateAngle = 300f;

    private void Awake()
    {
        poolTransform = this.transform;

        AdventureCoin.coinTransform = coinPrefab.transform;

        zeroQuaternion = Quaternion.Euler(Vector3.zero);

        for (int i =0; i<adventureCoinBundles.Length; i++)
        {
            adventureCoinBundleDic.Add(adventureCoinBundles[i].length, new Queue<AdventureCoinBundle>());
            Initialize(poolCount, adventureCoinBundles[i]);
        }
    }

    private void Start()
    {
        MagnetCoin.playerTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        coinPrefab.transform.Rotate(Vector3.forward, Time.deltaTime * rotateAngle);
    }

    private void Initialize(int initCount,AdventureCoinBundle adventureCoinBundle)
    {
        if(adventureCoinBundleDic.TryGetValue(adventureCoinBundle.length,out Queue<AdventureCoinBundle> queue))
        {
            for (int i = 0; i < initCount; i++)
            {
                queue.Enqueue(CreateNewObject(adventureCoinBundle));
            }
        }
    }

    private AdventureCoinBundle CreateNewObject(AdventureCoinBundle adventureCoinBundle)
    {
        AdventureCoinBundle newCoinBundle = Instantiate(adventureCoinBundle);
        newCoinBundle.transform.SetParent(poolTransform);
        return newCoinBundle;
    }

    private AdventureCoinBundle GetObject(int length)
    {
        AdventureCoinBundle getCoinBundle;

        if (adventureCoinBundleDic.TryGetValue(length, out Queue<AdventureCoinBundle> queue))
        {
            if (queue.Count > 0)
            {
                getCoinBundle = queue.Dequeue();
            }

            else
            {
                getCoinBundle = CreateNewObject(adventureCoinBundles[length - coinBundlesLowestLength]);
            }

            getCoinBundle.gameObject.SetActive(true);

            return getCoinBundle;
        }

        return null;
    }

    public void ReturnObject(AdventureCoinBundle retureCoinBundle)
    {
        if (adventureCoinBundleDic.TryGetValue(retureCoinBundle.length, out Queue<AdventureCoinBundle> queue))
        {
            retureCoinBundle.gameObject.SetActive(false);

            queue.Enqueue(retureCoinBundle);
            retureCoinBundle.transform.SetParent(poolTransform);
        }
    }

    public void CoinPlace(float ZposStart,float ZPosStartToEnd, BridgePart bridgePart,float bridgeScale)
    {
        AdventureCoinBundle adventureCoinBundle;
        float centerValue = PosValueSet();

        for (int i = coinBundlesHighestLength; i >= coinBundlesLowestLength;i--)
        {
            if(ZPosStartToEnd >= i)
            {
                adventureCoinBundle = GetObject(i);
                adventureCoinBundle.transform.SetParent(bridgePart.transform);
                adventureCoinBundle.transform.localPosition = new Vector3(centerValue, adventureCoinBundle.transform.localPosition.y
                    , ZposStart + (ZPosStartToEnd - i) / bridgeScale * 0.5f);
                adventureCoinBundle.transform.localRotation = zeroQuaternion;

                adventureCoinBundle.transform.SetParent(null);
                adventureCoinBundle.transform.localScale = Vector3.one;

                adventureCoinBundle.transform.SetParent(bridgePart.transform);

                bridgePart.parentBridge.coinBundleList.Add(adventureCoinBundle);

                return;
            }
        }
    }

    private float PosValueSet()
    {
        float randomNumber = Random.Range(1, 99);

        if (randomNumber <= 33)
        {
            return PlayerMoving.sideMove;
        }
        else if (randomNumber <= 66)
        {
            return -PlayerMoving.sideMove;
        }
        else if (randomNumber <= 99)
        {
            return 0;
        }
        return 0;
    }
}
