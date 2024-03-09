using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

abstract public class DragParent : MonoBehaviour
{
    public static Inventory inventory;

    public Canvas mycanvas; // raycast가 될 캔버스

    GraphicRaycaster gr;
    PointerEventData ped;

    [SerializeField]
    Vector3 endPoint;

    public Camera UiCamera;


    protected virtual void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        UiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        mycanvas = GameObject.Find("PanelCanvas").GetComponent<Canvas>();
    }

    protected virtual void Start()
    {
        gr = mycanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
    }

    protected virtual void Draging()
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 5f;
        endPoint = UiCamera.ScreenToWorldPoint(screenPoint);

        transform.position = endPoint;
    }

    protected virtual Slot DragEnd()
    { 
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
        gr.Raycast(ped, results);

        if (results.Count != 0)
        {
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.CompareTag("Slot"))
                {
                    return results[i].gameObject.GetComponent<Slot>();
                }
            }
        }
        return null;
    }
}
