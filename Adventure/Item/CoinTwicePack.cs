using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTwicePack : Item
{
    protected override void Use()
    {
        playerItemController.CoinTwiceModer();
    }
}
