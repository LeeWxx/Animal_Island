using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPack : Item
{
    protected override void Use()
    {
        playerItemController.Booster();
    }
}
