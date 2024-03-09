using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            if (AdventureManager.Instance.IsRevival == false && AdventureManager.Instance.LastExitZone != null && Advertisement.IsReady("Rewarded_Android"))
            {
                AdventureManager.Instance.GameOver();
                //AdventureManager.Instance.ReviveReady();
                //Camera.main.transform.SetParent(null);
                //StartCoroutine(CameraPosUp(new Vector3(Camera.main.transform.position.x, 2f, Camera.main.transform.position.z), 1f));
            }
            else
            {
                AdventureManager.Instance.GameOver();
            }
        }
    }

    //IEnumerator CameraPosUp(Vector3 targetPos, float targetTIme)
    //{
    //    float time = 0.0f;
    //    Vector3 firstPos = Camera.main.transform.position;
    //    while (time < targetTIme)
    //    {
    //        Vector3 new_pos = Vector3.Lerp(firstPos, targetPos, time / targetTIme);
    //        Camera.main.transform.position = new_pos;
    //        time += Time.deltaTime;
    //        yield return new WaitForSeconds(Time.deltaTime);
    //    }
    //}

}
