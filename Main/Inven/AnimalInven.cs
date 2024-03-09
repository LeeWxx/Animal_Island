using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInven : MonoBehaviour
{
    public AnimalInvenNode[] animalNodes = new AnimalInvenNode[40];

    public Sprite nonPurchaseButtonSprite;
    public Sprite PurchaseButtonSprite;

    private void Awake() 
    {
        for (int i = 0; i < animalNodes.Length; i++)
        {
            animalNodes[i].NodeSet();
        }

        AnimalNodesSet();
        OnClickSet();
    }

    private void OnClickSet()
    {
        for (int i = 0; i < animalNodes.Length; i++)
        {
            int temp = i;
            animalNodes[i].purchaseBut.onClick.AddListener(() => AnimalPurchase(animalNodes[temp].nodeAnimalState));
        }
    }

    private void OnEnable()
    {
        AnimalNodesCheck();
    }

    void AnimalNodesSet()
    {
        GameObject[] animalArray = AnimalManager.Instance.animalArray;

        for (int i = 0; i < animalNodes.Length; i++)
        {
            animalNodes[i].nodeAnimalState = animalArray[i].GetComponent<AnimalState>();
            animalNodes[i].nodeAnimalNameText.text = animalNodes[i].nodeAnimalState.name;
            animalNodes[i].priceText.text = "" + animalNodes[i].nodeAnimalState.Price;
            animalNodes[i].nodeAnimalIsland = animalArray[i].GetComponent<AnimalMovement>().myIsland;
            animalNodes[i].animalInven = this;

            LocalizeSrcript local = animalNodes[i].nodeAnimalNameText.GetComponent<LocalizeSrcript>();
            local.textKey = animalNodes[i].nodeAnimalState.name;
            local.LocalizeChanged();
        }
    }

    void AnimalNodesCheck()
    {
        for(int i = 0; i<animalNodes.Length; i++)
        {
            animalNodes[i].NodeCheck();
        }
    }


    public void AnimalPurchase(AnimalState animal)
    {
        AnimalMovement animalMovement = animal.GetComponent<AnimalMovement>();

        if (animalMovement.myIsland.IsSpawn == false)
        {
            WarningManager.Instance.NeedIslandWarning(animalMovement.myIsland.islandNum + 1);
        }
        else
        {
            if (GameManager.Instance.Gold >= animal.Price)
            {
                GameManager.Instance.GoldMinus(animal.Price);
                AnimalManager.Instance.SpawnReady(animal);
            }
            else
            {
                WarningManager.Instance.ShortageGoldWarning();
            }
        }
    }
}
