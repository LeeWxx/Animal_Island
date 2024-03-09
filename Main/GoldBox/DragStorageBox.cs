using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragStorageBox : DragParent, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Storage storage;
    GameObject fixBox;
    public GoldBox thisGoldBox;

    private RectTransform rectTransform;
    private Vector3 rectPos;
    private Transform parentTransform;

    protected override void Awake()
    {
        base.Awake();
        storage = FindObjectOfType<Storage>();
        rectTransform = GetComponent<RectTransform>();
        rectPos = rectTransform.localPosition;
        parentTransform = transform.parent;
    }

    protected override void Start()
    {
        base.Start();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (storage.ItemDragCheck(thisGoldBox))
        {
            fixBox = Instantiate(gameObject);
            fixBox.transform.SetParent(this.transform.parent);
            fixBox.transform.position = transform.position;
            fixBox.transform.localScale = transform.localScale;
            fixBox.transform.rotation = transform.rotation;
            fixBox.transform.SetAsFirstSibling();

            this.transform.SetParent(UIManager.Instance.goldBoxPanel.transform);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (storage.ItemDragCheck(thisGoldBox))
        {
            Draging();
        }
    }

    protected override void Draging()
    {
        base.Draging();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentTransform);
        this.transform.SetAsFirstSibling();
        rectTransform.localPosition = rectPos;

        if (storage.ItemDragCheck(thisGoldBox))
        {
            GameObject.Destroy(fixBox);
            Slot raySlot = DragEnd();

            if(raySlot != null)
            {
                if (raySlot.GetComponent<Button>().enabled == true)
                {
                    storage.BoxCompare(thisGoldBox, raySlot);
                }
                else
                {
                    storage.MoveBoxSlot(thisGoldBox, raySlot);
                }
            }
        }
    }

    protected override Slot DragEnd()
    {
        return base.DragEnd();
    }
}
