using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureNode : AnimalNode
{
    public Button adventureBut;

    public override void NodeSet()
    {
        base.NodeSet();
        adventureBut = transform.Find("Adventure Button").GetComponent<Button>();
    }

    public void NodeCheck()
    {
        if (nodeAnimalState.IsSpawn == true)
        {
            lockImage.gameObject.SetActive(false);
            animalProfileImage.gameObject.SetActive(true);
            adventureBut.gameObject.SetActive(true);
        }
        else
        {
            lockImage.gameObject.SetActive(true);
            animalProfileImage.gameObject.SetActive(false);
            adventureBut.gameObject.SetActive(false);
        }
    }
}
