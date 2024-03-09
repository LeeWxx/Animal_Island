using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IslandInvenNode : MonoBehaviour
{
    public Island island;

    [SerializeField]
    private Text islandNameText;

    [SerializeField]
    private Text priceText;

    public Button purchaseBut;
    public Image purchaseImage;

    public Image islandImage;
    public Image lockImage;

    public Image phaseImage;

    public void NodeSet()
    {
        islandNameText.text = island.name;
        priceText.text = "" + island.Price;
    }
}
