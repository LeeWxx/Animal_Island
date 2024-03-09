using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Placement
{
    public static PlayerItemController playerItemController;

    protected abstract void Use();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            Use();
            ItemSpawner.Instance.ReturnObject(this);
        }
    }
}
