using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Storage : MonoBehaviour
{
    public Inventory inventory;
    public Camera UICamera;

    public class StorageBox
    {
        public int count;
        public TextMeshProUGUI text;

        public StorageBox(int _count, TextMeshProUGUI _text)
        {
            count = _count;
            text = _text;
        }
    }

    Dictionary<GoldBox, StorageBox> storageBoxDic = new Dictionary<GoldBox, StorageBox>();
    public TextMeshProUGUI[] storageBoxCountText;
    public DragStorageBox[] storageBoxArray;

    private void Awake()
    {
        GoldBoxManager.Instance.storage = this;
    }
    public void StroageSet()
    {
        GoldBox[] goldBoxArray = GoldBoxManager.Instance.GoldBoxArray;
        for (int i = 0; i < goldBoxArray.Length; i++)
        {
            storageBoxDic.Add(goldBoxArray[i], new StorageBox(0, storageBoxCountText[i]));
            storageBoxArray[i].thisGoldBox = goldBoxArray[i];
        }
    }

    public void DataSave()
    {
        GoldBox[] goldBoxArray = GoldBoxManager.Instance.GoldBoxArray;
        for (int i = 0; i < goldBoxArray.Length; i++)
        {
            if (storageBoxDic.TryGetValue(goldBoxArray[i], out StorageBox storageBox))
            {
                PlayerPrefs.SetInt("Storage" + i, storageBox.count);
            }
        }
    }

    public void DataLoad()
    {
        GoldBox[] goldBoxArray = GoldBoxManager.Instance.GoldBoxArray;
        for (int i = 0; i < goldBoxArray.Length; i++)
        {
            if (storageBoxDic.TryGetValue(goldBoxArray[i], out StorageBox storageBox))
            {
                storageBox.count =  PlayerPrefs.GetInt("Storage" + i);
                storageBox.text.text = "" + storageBox.count;
            }
        }
    }

    public void ItemAdd(GoldBox goldBox)
    {
        if (storageBoxDic.TryGetValue(goldBox, out StorageBox storageBox))
        {
            storageBox.count += 1;
            storageBox.text.text = "" + storageBox.count;
        }
    }

     public bool ItemDragCheck(GoldBox goldBox)
    {
        if (storageBoxDic.TryGetValue(goldBox, out StorageBox storageBox))
        {
            if (storageBox.count > 0)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void MoveBoxSlot(GoldBox goldBox, Slot dropSlot)
    {
        SoundManager.Instance.PlaySFXSound("RandomBoxDropSound");
        dropSlot.AddItem(goldBox);
        ItemMinus(goldBox);
    }

    public void BoxCompare(GoldBox goldBox, Slot dropSlot)
    {
        if(goldBox.Phase == dropSlot.slotGoldBox.Phase)
        {
            if(goldBox.Phase != GoldBoxManager.Instance.GoldBoxArray.Length)
            {
                BoxMerge(goldBox, dropSlot);
                ItemMinus(goldBox);
            }
            else
            {
                inventory.anotherSlotClick(Array.IndexOf(inventory.slots, dropSlot));
            }
        }
        else
        {
            BoxTrade(goldBox, dropSlot);
        }
    }

    void BoxMerge(GoldBox goldBox, Slot dropSlot)
    {
        dropSlot.SlotItemSet(GoldBoxManager.Instance.GoldBoxArray[goldBox.Phase]);
        inventory.anotherSlotClick(Array.IndexOf(inventory.slots, dropSlot));
        RectTransform rectTransform = dropSlot.itemImage.GetComponent<RectTransform>();
        StartCoroutine(ParticleManager.Instance.MergeEffect(Utill.RectTransformToWorld(rectTransform, UICamera), 0.5f, goldBox.Phase));
    }

    void BoxTrade(GoldBox goldBox, Slot dropSlot)
    {
        ItemAdd(dropSlot.slotGoldBox);
        ItemMinus(goldBox);
        SoundManager.Instance.PlaySFXSound("RandomBoxDropSound");
        dropSlot.SlotItemSet(goldBox);
        inventory.SlotClick(Array.IndexOf(inventory.slots, dropSlot));
    }

    void ItemMinus(GoldBox goldBox)
    {
        if (storageBoxDic.TryGetValue(goldBox, out StorageBox storageBox))
        {
            if (storageBox.count > 0)
            {
                storageBox.count -= 1;
                storageBox.text.text = "" + storageBox.count;
            }
        }
    }

    private void OnDisable()
    {
        DataSave();
    }
}
