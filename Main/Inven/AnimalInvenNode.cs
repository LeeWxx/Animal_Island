using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalInvenNode : AnimalNode
{
    public AnimalInven animalInven;

    public Text priceText;

    public Button purchaseBut;

    private void Awake()
    {
        animalInven = FindObjectOfType<AnimalInven>();
    }

    public override void NodeSet()
    {
        base.NodeSet();
        priceText = transform.Find("Purchase Button").Find("PurchaseGoldLabel").GetComponent<Text>();
        GameObject purchaseButObj = transform.Find("Purchase Button").gameObject;
        purchaseBut = purchaseButObj.GetComponent<Button>();
    }

    public void NodeCheck()
    {
        if (nodeAnimalState.IsSpawn == true)
        {
            lockImage.gameObject.SetActive(false);
            animalProfileImage.gameObject.SetActive(true);
            purchaseBut.gameObject.SetActive(false);
        }
        else if (nodeAnimalState.Phase <= GameManager.Instance.PlayerPhase+1)
        {
            lockImage.gameObject.SetActive(false);
            animalProfileImage.gameObject.SetActive(true);
            purchaseBut.gameObject.SetActive(true);
            purchaseBut.enabled = true;
        }
        else
        {
            lockImage.gameObject.SetActive(true);
            animalProfileImage.gameObject.SetActive(false);
            purchaseBut.gameObject.SetActive(false);
        }
    }
      
}
