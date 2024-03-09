using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AdventureCoin : MonoBehaviour
{
    Vector3 CoinRotate = new Vector3(90f, 360f, 0f);
    public float Duration = 1;

    public static Transform coinTransform;
    private static int getCoin = 1;

    public static int GetCoin
    {
        get { return getCoin; }
        set { getCoin = value; }
    }

    MagnetCoin magnetCoinScript;

    private void Awake()
    {
        magnetCoinScript = GetComponent<MagnetCoin>();
    }

    void Update()
    {
        this.transform.rotation = coinTransform.rotation;
    }

    private void OnEnable()
    {
        magnetCoinScript.enabled = false;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            AdventureManager.Instance.CoinUp(getCoin);
            SoundManager.Instance.PlaySFXSound("AdventrueCoinSound");
            this.gameObject.SetActive(false);
        }
        else if (other.CompareTag("CoinDetector") == true)
        {
            magnetCoinScript.enabled = true;
        }
    }
}
