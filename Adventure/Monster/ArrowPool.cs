using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    public Arrow arrowPrefab;
    Queue<Arrow> arrowQueue = new Queue<Arrow>();

    private int arrowCount = 50;

    Vector3 arrowLocalPos = new Vector3(0, 120f, 0);
    Quaternion arrowLocalRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
    Vector3 arrowLocalScale = new Vector3(3f, 0.6f, 3f);

    public Material[] materialArray;

    private void Start()
    {
        for (int i = 0; i < arrowCount; i++)
        {
            Arrow arrow = CreateNewObject();
            arrow.transform.parent = this.transform;
        }
    }

    private Arrow CreateNewObject()
    {
        Arrow arrow = Instantiate(arrowPrefab);
        arrow.MeshRendererSet();
        arrowQueue.Enqueue(arrow);
        arrow.gameObject.SetActive(false);
        return arrow;
    }

    private Arrow GetObject()
    {
        Arrow arrow;

        if (arrowQueue.Count == 0)
        {
            arrow = CreateNewObject();
        }
        else
        {
            arrow = arrowQueue.Dequeue();
        }

        return arrow;
    }

    public void ReturnObject(Arrow arrow)
    {
        arrowQueue.Enqueue(arrow);
        arrow.transform.parent = null;
        arrow.gameObject.SetActive(false);
    }

    public void Shot(Archer archer)
    {
        Arrow arrow = GetObject();

        arrow.MaterialChange(materialArray[archer.stage-1]);
        arrow.transform.parent = archer.transform;
        arrow.transform.localPosition = arrowLocalPos;
        arrow.transform.localRotation = arrowLocalRotation;
        arrow.transform.localScale = arrowLocalScale;
        arrow.gameObject.SetActive(true);
    }
}
