using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureCoinBundle : MonoBehaviour
{
    public int length;

    private AdventureCoin[] adventureCoins;

    public Transform rotateTransform;

    private void Awake()
    {
        adventureCoins = GetComponentsInChildren<AdventureCoin>();
    }

    private void OnEnable()
    {
        for(int i =0; i<adventureCoins.Length; i++)
        {
           if(adventureCoins[i].gameObject.activeSelf == false)
           {
                adventureCoins[i].gameObject.SetActive(true);
           }
        }
    }
}
