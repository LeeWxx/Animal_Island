using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : Item
{
    protected override void Use()
    {
        playerItemController.Heal( 10 * (PlayerMovement.movingStage));
    }
}
