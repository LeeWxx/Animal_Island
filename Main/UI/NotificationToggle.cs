using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationToggle : MonoBehaviour
{
    private Toggle thisToggle;
    public GameObject toggle;
    public GameObject backToggle;

    private void Awake()
    {
        thisToggle = GetComponent<Toggle>();

        OnValueCheck();
    }
    public void OnValueCheck()
    {
        if(thisToggle.isOn == true)
        {
            toggle.transform.localPosition = new Vector3(30, 0, 0);
            backToggle.transform.gameObject.SetActive(true);
            NotifyManager.isNotifyAllow = true;
        }
        else
        {
            toggle.transform.localPosition = new Vector3(-30, 0, 0);
            backToggle.transform.gameObject.SetActive(false);
            NotifyManager.isNotifyAllow = false;
        }
    }
}
