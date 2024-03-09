using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandInven : MonoBehaviour
{
    public IslandInvenNode[] islandInvenNodes = new IslandInvenNode[4];

    private void Awake()
    {
        for(int i = 0; i< islandInvenNodes.Length; i++)
        {
            islandInvenNodes[i].NodeSet();
            int _i = i;
            islandInvenNodes[i].purchaseBut.onClick.AddListener(delegate { IslandPurchase(islandInvenNodes[_i].island); });
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < islandInvenNodes.Length; i++)
        {
            if (islandInvenNodes[i].island.IsSpawn == true)
            {
                islandInvenNodes[i].purchaseBut.gameObject.SetActive(false);
                islandInvenNodes[i].lockImage.gameObject.SetActive(false);
                islandInvenNodes[i].islandImage.gameObject.SetActive(true);
                islandInvenNodes[i].phaseImage.gameObject.SetActive(false);
            }

            else if (islandInvenNodes[i].island.NeedPhase <= GameManager.Instance.PlayerPhase + 1 && islandInvenNodes[i].island.IsSpawn == false)
            {
                islandInvenNodes[i].purchaseBut.gameObject.SetActive(true);
                islandInvenNodes[i].lockImage.gameObject.SetActive(false);
                islandInvenNodes[i].islandImage.gameObject.SetActive(true);
                islandInvenNodes[i].phaseImage.gameObject.SetActive(false);
                islandInvenNodes[i].purchaseBut.enabled = true;
            }
            else
            {
                islandInvenNodes[i].purchaseBut.gameObject.SetActive(false);
                islandInvenNodes[i].lockImage.gameObject.SetActive(true);
                islandInvenNodes[i].islandImage.gameObject.SetActive(false);
                islandInvenNodes[i].phaseImage.gameObject.SetActive(true);
            }
        }
    }

    public void IslandPurchase(Island island)
    {
        if (GameManager.Instance.Gold >= island.Price)
        {
            GameManager.Instance.GoldMinus(island.Price);
            IslandManager.Instance.SpawnReady(island);
        }
        else
        {
            WarningManager.Instance.ShortageGoldWarning();
        }
    }

}
