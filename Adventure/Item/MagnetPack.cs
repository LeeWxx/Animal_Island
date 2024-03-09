using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPack : Item
{
    protected override void Use()
    {
        playerItemController.Magneter();
    }
}
