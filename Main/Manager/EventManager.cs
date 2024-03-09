using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventManager>();
            }

            return instance;
        }
    }

    public enum NowPressBtn  {GoldEvent,HeartEvent};
    public NowPressBtn nowPressBtn;

    private bool isGoldEvent;

    public bool IsGoldEvent
    {
        get { return isGoldEvent; }
    }

    private bool isGoldEventReady;

    public Button goldEventBut;
    public Image goldEventOn;
    public GameObject goldEventIcon;
    public GameObject goldEventReadyIcon;
    public Text goldReadyTimerText;

    private bool isHeartEvent;

    public bool IsHeartEvent
    {
        get { return isHeartEvent; }
    }

    private bool isHeartEventReady;

    public Button heartEventBut;
    public Image heartEventOn;
    public GameObject heartEventIcon;
    public GameObject heartEventReadyIcon;
    public Text heartReadyTimerText;

    float eventSeconds = 10f;

    TimeSpan eventReadyTimeSpan = new TimeSpan(0, 0, 30);
    DateTime nextGoldEventDateTime;
    DateTime nextHeartEventDateTime;

    string nextGoldEventDate;
    string nextHeartEventDate;



    private void Awake()
    {
        DataLoad();

        if (DateTime.Now < nextHeartEventDateTime)
        {
            HeartEventIsReady(true);
        }
        else
        {
            HeartEventIsReady(false);
        }

        isGoldEvent = false;
        isHeartEvent = false;

    }

    private void DataLoad()
    {
        if(PlayerPrefs.GetInt("goldEventNotFirst") != 0)
        {
            nextGoldEventDate = PlayerPrefs.GetString("nextGoldEventDate");
            nextGoldEventDateTime = DateTime.ParseExact(nextGoldEventDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

            if (DateTime.Now < nextGoldEventDateTime)
            {
                GoldEventIsReady(true);
            }
            else
            {
                GoldEventIsReady(false);
            }
        }

        if (PlayerPrefs.GetInt("heartEventNotFirst") != 0)
        {
            nextHeartEventDate = PlayerPrefs.GetString("nextHeartEventDate");
            nextHeartEventDateTime = DateTime.ParseExact(nextHeartEventDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

            if (DateTime.Now < nextHeartEventDateTime)
            {
                HeartEventIsReady(true);
            }
            else
            {
                HeartEventIsReady(false);
            }
        }
    }

    public void GoldEventButClick()
    {
        SoundManager.Instance.PlaySFXSound("PanelOpenSound");
        nowPressBtn = NowPressBtn.GoldEvent;
        if (Advertisement.IsReady("Rewarded_Android"))
        {
            AdsManager.Instance.ShowRewardedAd();
        }
        else
        {
            GoldEvent();
        }
    }

    public void HeartEventButClick()
    {
        SoundManager.Instance.PlaySFXSound("PanelOpenSound");
        nowPressBtn = NowPressBtn.HeartEvent;
        if (Advertisement.IsReady("Rewarded_Android"))
        {
            AdsManager.Instance.ShowRewardedAd();
        }
        else
        {
            HeartEvent();
        }
    }

    public IEnumerator GoldEvent()
    {
        if (PlayerPrefs.GetInt("goldEventNotFirst") == 0)
        {
            PlayerPrefs.SetInt("goldEventNotFirst", 1);
        }

        isGoldEvent = true;
        goldEventBut.enabled = false;


        nextGoldEventDateTime = DateTime.Now + eventReadyTimeSpan;
        nextGoldEventDate = nextGoldEventDateTime.ToString("yyyyMMddHHmmss");
        PlayerPrefs.SetString("nextGoldEventDate", nextGoldEventDate);

        Debug.Log(nextGoldEventDate);

        float current = eventSeconds;

        while (current >= 0)
        {
            goldEventOn.fillAmount = current / eventSeconds;
            yield return new WaitForSeconds(0.1f);
            current -= 0.1f;
        }

        GoldEventIsReady(true);
        isGoldEvent = false;
    }

    private void GoldEventIsReady(bool boolParameter)
    {
        isGoldEventReady = boolParameter;
        goldEventIcon.SetActive(!boolParameter);
        goldEventReadyIcon.SetActive(boolParameter);

        if (boolParameter == true)
        {
            goldEventOn.fillAmount = 0;
        }
        else
        {
            goldEventOn.fillAmount = 1;
        }
    }

    public IEnumerator HeartEvent()
    {
        if (PlayerPrefs.GetInt("heartEventNotFirst") == 0)
        {
            PlayerPrefs.SetInt("heartEventNotFirst", 1);
        }

        isHeartEvent = true;
        heartEventBut.enabled = false;


        float current = eventSeconds;

        nextHeartEventDateTime = DateTime.Now + eventReadyTimeSpan;
        nextHeartEventDate = nextHeartEventDateTime.ToString("yyyyMMddHHmmss");
        PlayerPrefs.SetString("nextHeartEventDate", nextHeartEventDate);

        while (current >= 0)
        {
            heartEventOn.fillAmount = current / eventSeconds;
            yield return new WaitForSeconds(0.1f);
            current -= 0.1f;
        }

        HeartEventIsReady(true);
        isHeartEvent = false;
    }

    private void HeartEventIsReady(bool boolParameter)
    {
        isHeartEventReady = boolParameter;
        heartEventIcon.SetActive(!boolParameter);
        heartEventReadyIcon.SetActive(boolParameter);

        if(boolParameter == true)
        {
            heartEventOn.fillAmount = 0;
        }
        else
        {
            heartEventOn.fillAmount = 1;
        }
    }

    private void Update()
    {
        if (isHeartEventReady == true)
        {
            if (DateTime.Now < nextHeartEventDateTime)
            {
                TimeSpan ts = nextHeartEventDateTime - DateTime.Now;
                heartReadyTimerText.text = ts.Minutes + ":" + ts.Seconds;
            }
            else
            {
                isHeartEventReady = false;
                heartEventIcon.SetActive(true);
                heartEventReadyIcon.SetActive(false);
                heartEventOn.fillAmount = 1;
                heartEventBut.enabled = true;
            }
        }

        if (isGoldEventReady == true)
        {
            if (DateTime.Now < nextGoldEventDateTime)
            {
                TimeSpan ts = nextGoldEventDateTime - DateTime.Now;
                goldReadyTimerText.text = ts.Minutes + ":" + ts.Seconds;
            }
            else
            {
                isGoldEventReady = false;
                goldEventIcon.SetActive(true);
                goldEventReadyIcon.SetActive(false);
                goldEventOn.fillAmount = 1;
                goldEventBut.enabled = true;
            }
        }
    }
}
