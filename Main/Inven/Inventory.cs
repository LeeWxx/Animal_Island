using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Inventory : MonoBehaviour
{
    private int slotCount = 12;

    public Slot[] slots;
    public Storage storage;
    public Image openBoxImage;
    public GameObject choiceMarkObj;

    public Button openButton;

    private delegate void SlotChoice(int index);
    private SlotChoice slotChoice;

    public GameObject getGoldObject;
    public TextMeshProUGUI getGoldText;

    public delegate void AnotherSlotClick(int index);
    public AnotherSlotClick anotherSlotClick;

    public Camera UICamera;

    private void Awake()
    {
        slotChoice += SlotClick;

        anotherSlotClick += SlotClick;
        anotherSlotClick += OpenBoxImageChange;
        anotherSlotClick += OpenButtonChange;

        SlotSet();
        OnClickSet();
        getGoldObject.SetActive(false);

        openButton.onClick.AddListener(BoxOpen);

        GoldBoxManager.Instance.inventory = this;
        GoldBoxManager.Instance.ReadyGoldBoxGet();
    }

    void SlotSet()
    {
        slots = new Slot[slotCount];

        for (int i =0; i< slotCount; i++)
        {
           slots[i] = transform.Find("Panel").transform.Find("Slot" + (i + 1)).GetComponent<Slot>();
        }
    }

    public void DataLoad()
    {
        for (int i = 0; i < slots.Length; i++)
        {
           int goldBoxPhase = PlayerPrefs.GetInt("Slot" + i + "Box");
           
            if(goldBoxPhase != 0)
            {
                slots[i].AddItem(GoldBoxManager.Instance.GoldBoxArray[goldBoxPhase - 1]);
            }
           
        }
    }
    
    public void DataSave()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].slotBut.enabled == true)
            {
                PlayerPrefs.SetInt("Slot" + i + "Box", slots[i].slotGoldBox.Phase);
            }
            else
            {
                PlayerPrefs.SetInt("Slot" + i + "Box",0);
            }
        }
    }

    private void OnEnable()
    {
        choiceMarkObj.SetActive(false);
        openBoxImage.gameObject.SetActive(false);
        openButton.gameObject.SetActive(false);
        
        if(getGoldObject.activeSelf == true)
        {
            getGoldObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        DataSave();
    }

    public void ItemAdd(GoldBox goldBox)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].slotBut.enabled == false)
            {
                slots[i].AddItem(goldBox);
                return;
            }
        }
        storage.ItemAdd(goldBox);
    }


    void OnClickSet()
    {
        for(int i =0; i<slots.Length; i++)
        {
            int _i = i;
            slots[i].slotBut.onClick.AddListener(delegate { anotherSlotClick(_i); });
            
        }
    }

    public void SlotClick(int index)
    {
        if(getGoldObject.activeSelf == true)
        {
            getGoldObject.SetActive(false);
        }

        choiceMarkObj.transform.SetParent(slots[index].transform);
        choiceMarkObj.transform.localPosition = Vector3.zero;

        if(choiceMarkObj.activeSelf == false)
        {
            choiceMarkObj.SetActive(true);
        }
    }

    void OpenBoxImageChange(int index)
    {
        openBoxImage.sprite = slots[index].itemImage.sprite;
        openBoxImage.transform.localScale = slots[index].slotGoldBox.inSlotSacle * 3.8f;
        if (openBoxImage.gameObject.activeSelf == false)
        {
            openBoxImage.gameObject.SetActive(true);
        }
    }

    void OpenButtonChange(int index)
    {
        //getGoldRangeText.text = slots[index].slotGoldBox.GetGoldMinValue + " ~ " + slots[index].slotGoldBox.GetGoldMaxValue;

        if(openButton.gameObject.activeSelf == false)
        {
            openButton.gameObject.SetActive(true);
        }
    }

    public void BoxCompare(Slot slot1, Slot slot2)
    {
        int slot2Index = Array.IndexOf(slots, slot2);
        if (slot1.slotGoldBox.Phase == slot2.slotGoldBox.Phase && slot1.slotGoldBox.Phase !=6)
        {
            BoxMerge(slot1,slot2, slot2Index);
        }
        else
        {
            SwitchSlotBox(slot1,slot2, slot2Index);
        }
    }

    void SwitchSlotBox(Slot slot1, Slot slot2,int slot2Index)
    {
        int slot1BoxPhase = slot1.slotGoldBox.Phase;
        int slot2BoxPhase = slot2.slotGoldBox.Phase;

        slot1.SlotItemSet(GoldBoxManager.Instance.GoldBoxArray[slot2BoxPhase - 1]);
        slot2.SlotItemSet(GoldBoxManager.Instance.GoldBoxArray[slot1BoxPhase - 1]);

        SoundManager.Instance.PlaySFXSound("RandomBoxDropSound");
        anotherSlotClick(slot2Index);
    }

    void BoxMerge(Slot slot1,Slot slot2,int slot2Index)
    {
        SoundManager.Instance.PlaySFXSound("RandomBoxMergeSound");

        GoldBox mergeGoldBox = GoldBoxManager.Instance.GoldBoxArray[slot1.slotGoldBox.Phase];
        slot2.SlotItemSet(mergeGoldBox);
        SlotClear(slot1);
        int Index = Array.IndexOf(slots, slot1);
        anotherSlotClick(slot2Index);

        RectTransform rectTransform = slot2.itemImage.GetComponent<RectTransform>();
        StartCoroutine(ParticleManager.Instance.MergeEffect(Utill.RectTransformToWorld(rectTransform,UICamera), 0.5f, mergeGoldBox.Phase));
    }

    void SlotClear(Slot slot)
    {
        slot.itemImage.gameObject.SetActive(false);

        if (slot.slotBut.enabled == true)
        {
            slot.slotBut.enabled = false;
        }
    }

    public void BoxOpen()
    {
        Slot nowSlot = choiceMarkObj.GetComponentInParent<Slot>();
        GoldBox goldBox = nowSlot.slotGoldBox;

        int Index = Array.IndexOf(slots, nowSlot);

        SlotClear(nowSlot);

        RectTransform rectTransform = openBoxImage.GetComponent<RectTransform>();
        int getGold = UnityEngine.Random.Range(goldBox.GetGoldMinValue, goldBox.GetGoldMaxValue);
        getGoldObject.SetActive(true);
        getGoldText.text = "+" + getGold;
        GameManager.Instance.GoldPlus(getGold);


        SoundManager.Instance.PlaySFXSound("RandomBoxOpenSound");
        StartCoroutine(ParticleManager.Instance.BoxOpenEffect(Utill.RectTransformToWorld(rectTransform, UICamera), 0.5f));

        openBoxImage.gameObject.SetActive(false);
        openButton.gameObject.SetActive(false);
        choiceMarkObj.SetActive(false);
    }

    public void BoxSlotMove(Slot beforeSlot,Slot afterSlot)
    {
        afterSlot.AddItem(beforeSlot.slotGoldBox);
        SlotClear(beforeSlot);

        int Index = Array.IndexOf(slots, afterSlot);
        SlotClick(Index);
    }
}
