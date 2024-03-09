using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchCoin : MonoBehaviour
{
    public GoldCoinText goldCoinText;
    
    public void TextSet()
    {
        transform.Find("CoinText").GetComponent<GoldCoinText>();
    }
}
