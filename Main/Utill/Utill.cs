using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public static class Utill 
{
     public static Vector3 RectTransformToWorld(RectTransform rectTransform,Camera camera)
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(camera, rectTransform.position);
        Vector3 result = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPos, camera, out result);

        return result;
    }

    public static T[] SuffleArray<T>(T[] gameObjects)
    {
        System.Random random = new System.Random();
        gameObjects = gameObjects.OrderBy(x => random.Next()).ToArray();

        return gameObjects;
    }

    public static Queue<T> SuffleQueue<T>(Queue<T> queue)
    {

        List<T> shuffleList = new List<T>();

        for (int i = 0; i < queue.Count; i++)
        {
            shuffleList.Add(queue.Dequeue());
        }

        shuffleList = ShuffleList(shuffleList);

        for (int i = 0; i < shuffleList.Count; i++)
        {
            queue.Enqueue(shuffleList[i]);
        }

        return queue;
    }

    public static List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }

    public static void SetLayerRecursively(Transform trans,string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach(Transform child in trans)
        {
            SetLayerRecursively(child, name);
        }
    }
}
