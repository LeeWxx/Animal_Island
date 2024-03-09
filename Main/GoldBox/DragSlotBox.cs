using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlotBox : DragParent, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Slot thisSlot;
    Vector3 objPos;

    protected override void Awake()
    {
        base.Awake();
        objPos = transform.position;
    }

    protected override void Start()
    {
        base.Start();
        thisSlot = this.GetComponentInParent<Slot>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetParent(this.transform.parent.parent);
        this.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Draging();
    }

    protected override void Draging()
    {
        base.Draging();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(thisSlot.transform);
        this.transform.position = objPos;

        Slot raySlot = DragEnd();
        if(raySlot != null)
        {
            if (raySlot != thisSlot)
            {
                if (raySlot.GetComponent<Button>().enabled == true)
                {
                    inventory.BoxCompare(thisSlot, raySlot);
                }
                else
                {
                    inventory.BoxSlotMove(thisSlot, raySlot);
                }

            }
        }
    }

    protected override Slot DragEnd()
    {
        return base.DragEnd();
    }
}
