using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCoinPool : MonoBehaviour
{
    private static TouchCoinPool instance;

    public static TouchCoinPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TouchCoinPool>();
            }

            return instance;
        }
    }

    TouchCoin touchCoin;
    Queue<TouchCoin> touchCoinQueue = new Queue<TouchCoin>();

    public float touchCoinDuration = 1f;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }

        touchCoin = GameObject.Find("TouchCoin").GetComponent<TouchCoin>();
        StartCoroutine(CreateNewObject(10));
    }

    private IEnumerator CreateNewObject(int count = 1)
    {
        for(int i =0; i<count; i++)
        {
            TouchCoin newCoin = GameObject.Instantiate(touchCoin);
            newCoin.transform.parent = this.gameObject.transform;
            yield return null;
            newCoin.TextSet();
            newCoin.gameObject.SetActive(false);
            touchCoinQueue.Enqueue(newCoin);
        }
    } 

    private TouchCoin GetObject()
    {
        if(touchCoinQueue.Count == 0)
        {
            CreateNewObject(5);
        }

        TouchCoin touchCoin = touchCoinQueue.Dequeue();

        return touchCoin;
    }

    private void ReturnObject(TouchCoin touchCoin)
    {
        touchCoin.gameObject.SetActive(false);
        touchCoinQueue.Enqueue(touchCoin);
    }

    public IEnumerator TouchCoinGet(Vector3 pos)
    {
        int plusGold;

        if(EventManager.Instance.IsGoldEvent == true)
        {
            plusGold = 50;
        }
        else
        {
            plusGold = 10;
        }

        GameManager.Instance.GoldPlus(plusGold);

        TouchCoin touchCoin = GetObject();

        float lookYRotate = CameraManager.Instance.camFirstRotation.eulerAngles.y - Camera.main.transform.localRotation.eulerAngles.y;

        touchCoin.transform.position = pos;
        touchCoin.transform.localRotation = Quaternion.Euler(new Vector3(touchCoin.transform.localRotation.eulerAngles.x,
            -lookYRotate, touchCoin.transform.localRotation.eulerAngles.z));
        touchCoin.goldCoinText.CoinTextChange(plusGold);
        touchCoin.gameObject.SetActive(true);

        yield return new WaitForSeconds(touchCoinDuration);

        ReturnObject(touchCoin);
    }
}
