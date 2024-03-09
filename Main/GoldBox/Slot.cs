using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Button slotBut;
    public Image itemImage;
    public GoldBox slotGoldBox;

    public void AddItem(GoldBox goldBox)
    {
        SlotItemSet(goldBox);
        itemImage.gameObject.SetActive(true);
        slotBut.enabled = true;
    }

    public void SlotItemSet(GoldBox goldBox)
    {
        slotGoldBox = goldBox;
        itemImage.sprite = goldBox.goldBoxImage;
        itemImage.transform.localPosition = goldBox.inSlotPos;
        itemImage.transform.localScale = goldBox.inSlotSacle;
    }
}
