using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    PlayerMeshController playerMeshController;

    Coroutine boost;
    Coroutine coinTwice;
    Coroutine margnet;

    CoinDetector coinDetector;

    BoxCollider boxCollider;

    private Buff boostBuff;
    private Buff magnetBuff;
    private Buff coinTwiceBuff;

    WaitForSeconds seconds = new WaitForSeconds(0.1f);

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMeshController = GetComponent<PlayerMeshController>();

        boxCollider = GetComponent<BoxCollider>();

        GameObject buffState = GameObject.Find("BuffState");

        boostBuff = buffState.transform.Find("BoostBuff").GetComponent<Buff>();
        magnetBuff = buffState.transform.Find("MagnetBuff").GetComponent<Buff>();
        coinTwiceBuff = buffState.transform.Find("CoinTwiceBuff").GetComponent<Buff>();
    }

    private void Start()
    {
        coinDetector = FindObjectOfType<CoinDetector>();
        coinDetector.transform.SetParent(playerMovement.transform);
        coinDetector.transform.localPosition = new Vector3(0, 0, 2f);
        coinDetector.gameObject.SetActive(false);
    }
    public void Heal(int plusHp)
    {
        if (playerHealth.Hp + plusHp >= playerHealth.MaxHp)
        {
            playerHealth.Hp = playerHealth.MaxHp;
        }
        else
        {
            playerHealth.Hp += plusHp;
        }

        AdventureUIManager.Instance.HpPlusUpdate(plusHp);
    }

    public void Booster()
    {
        if (boost != null)
        {
            StopCoroutine(boost);
        }

        boost = StartCoroutine(Boost());
    }

    public IEnumerator Boost()
    {
        playerMovement.IsBoost = true;
        playerMovement.SpeedSet();
        playerMeshController.BoostModeOn();
        //playerMovement.jumpPower = 20f;

        //playerMovement.rigidBody.constraints = RigidbodyConstraints.None;
        //playerMovement.rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //boxCollider.isTrigger = true;
        BridgeLoop.Instance.BoostBridgeOn();
        Utill.SetLayerRecursively(playerMovement.transform, "BoostAnimal");

        float currentTime = 0f;


        boostBuff.gameObject.SetActive(true);
        boostBuff.transform.SetAsLastSibling();
        boostBuff.buffTimeStateImage.fillAmount = 0f;

        while (currentTime < boostBuff.duration)
        {
            currentTime += 0.1f;
            boostBuff.buffTimeStateImage.fillAmount = currentTime / boostBuff.duration;
            yield return seconds;
        }

        boostBuff.buffTimeStateImage.fillAmount = 1f;
        boostBuff.gameObject.SetActive(false);

        StartCoroutine(BoostOff());
    }

    private IEnumerator BoostOff()
    {
        while (playerMovement.isOnBridge == false)
        {
            yield return null;
        }

        playerMovement.IsBoost = false;
        playerMovement.SpeedSet();
        playerMeshController.OriginalMaterialOn();
        //playerMovement.rigidBody.constraints = RigidbodyConstraints.None;
        //playerMovement.rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //boxCollider.isTrigger = false;
        Utill.SetLayerRecursively(playerMovement.transform, "Animal");
        BridgeLoop.Instance.BoostBridgeOff();
    }

    public void CoinTwiceModer()
    {
        if (coinTwice != null)
        {
            StopCoroutine(coinTwice);
        }

        coinTwice = StartCoroutine(CoinTwiceMode());
    }

    public IEnumerator CoinTwiceMode()
    {
        AdventureCoin.GetCoin = 2;

        float currentTime = 0f;

        coinTwiceBuff.gameObject.SetActive(true);
        coinTwiceBuff.transform.SetAsLastSibling();
        coinTwiceBuff.buffTimeStateImage.fillAmount = 0f;

        while (currentTime < coinTwiceBuff.duration)
        {
            currentTime += 0.1f;
            coinTwiceBuff.buffTimeStateImage.fillAmount = currentTime / coinTwiceBuff.duration;
            yield return seconds;
        }

        coinTwiceBuff.buffTimeStateImage.fillAmount = 1f;
        coinTwiceBuff.gameObject.SetActive(false);

        AdventureCoin.GetCoin = 1;
    }

    public void Magneter()
    {
        if (margnet != null)
        {
            StopCoroutine(margnet);
        }
        margnet = StartCoroutine(Magnet());
    }

    public IEnumerator Magnet()
    {
        coinDetector.gameObject.SetActive(true);

        float currentTime = 0f;

        magnetBuff.gameObject.SetActive(true);
        magnetBuff.transform.SetAsLastSibling();
        magnetBuff.buffTimeStateImage.fillAmount = 0f;

        while (currentTime < magnetBuff.duration)
        {
            currentTime += 0.1f;
            magnetBuff.buffTimeStateImage.fillAmount = currentTime / magnetBuff.duration;
            yield return seconds;
        }


        magnetBuff.buffTimeStateImage.fillAmount = 1f;
        magnetBuff.gameObject.SetActive(false);

        coinDetector.gameObject.SetActive(false);
    }

    public void BuffAllOff()
    {
        boostBuff.gameObject.SetActive(false);
        magnetBuff.gameObject.SetActive(false);
        coinTwiceBuff.gameObject.SetActive(false);

        if (boost != null)
        {
            StopCoroutine(boost);
        }
        if (margnet != null)
        {
            StopCoroutine(margnet);
        }
        if(coinTwice != null)
        {
            StopCoroutine(coinTwice);
        }
    }
}
