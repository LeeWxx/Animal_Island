using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MissionManager : MonoBehaviour
{
    private static MissionManager instance;

    public static MissionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MissionManager>();
            }

            return instance;
        }
    }

    public class Mission
    {
        public string name;

        private int missionValue;

        public int Value
        {
            get { return missionValue; }
            set { missionValue = value; }
        }

        public int hierarchyNum;

        public bool missionClear = false;
        public bool missionComplete = false;

        private int targetValue;

        public int TargetValue
        {
            get { return targetValue; }
            set { targetValue = value; }
        }

        private int rewardNum;
        
        public int RewardNum
        {
            get { return rewardNum; }
            set { rewardNum = value; }
        }
    }

    Dictionary<string, Mission> missionDic = new Dictionary<string, Mission>();

    List<Dictionary<string, object>> data_Mission;

    public List<Dictionary<string, object>> Data_Mission
    {
        get { return data_Mission; }
    }

    public Dictionary<string, MissionSlot> missionSlotDic = new Dictionary<string, MissionSlot>();

    public Sprite nonClearButSprite;
    public Sprite clearButSprite;

    public Sprite nonClearBgSprite;
    public Sprite clearBgSprite;

    private Color nonClearBgColor = new Color(1f,1f,1f,0.7f);
    private Color clearBgColor = new Color(1f, 1f, 1f, 1f);

    public delegate void CompleteButClick(string name);
    public CompleteButClick completeButClick;

    public Image missionClearImage;
    Color imageOriginalColor;
    public Text missionClearText;
    Color textOriginalColor;

    private GoldBox rewardGoldBox;

    DateTime resetTime;
    TimeSpan resetTimer;

    string resetDate;

    int notFirst;
    int goldBoxIndex;


    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }


        completeButClick += MissionCompleteClick;

        imageOriginalColor = missionClearImage.color;
        textOriginalColor = missionClearText.color;
    }

    private void Start()
    {
        MissionSet();

        if (PlayerPrefs.GetInt("missionNotFirst") == 0)
        {
            ResetTimeSet();
            MissionReset();
            PlayerPrefs.SetInt("missionNotFirst", 1);
        }
        else
        {
            DataLoad();
        }

        ResetTimeCheck();
        MissionSlotSet();
        CompleteOnClickSet();
    }

    private void ResetTimeSet()
    {
        resetTime = Convert.ToDateTime("10:10:00 AM");
        resetTime = resetTime.AddDays(1);

        resetDate = resetTime.ToString("yyyyMMddHHmmss");
        PlayerPrefs.SetString("resetDate", resetDate);
    }

    public bool ResetTimeCheck()
    {
        if (DateTime.Now > resetTime)
        {
            MissionReset();
            ResetTimeSet();
            return true;
        }

        return false;
    }

    public string GetResetTime()
    {
        TimeSpan resetTimeInterval = resetTime - DateTime.Now;
        string resetTimeIntervalString = resetTimeInterval.Hours + ":" + resetTimeInterval.Minutes + ":" + resetTimeInterval.Seconds;
        return resetTimeIntervalString;
    }

    private void OnApplicationQuit()
    {
        DataSave();
    }

    public void DataLoad()
    {
        resetDate = PlayerPrefs.GetString("resetDate");
        resetTime = DateTime.ParseExact(resetDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

        goldBoxIndex = PlayerPrefs.GetInt("goldBoxIndex");
        rewardGoldBox = GoldBoxManager.Instance.GoldBoxArray[goldBoxIndex];

        for (int i = 0; i < missionDic.Count; i++)
        {
            if (missionDic.TryGetValue((string)data_Mission[i]["Name"], out Mission mission))
            {
                mission.Value = PlayerPrefs.GetInt(mission.name + "Value", mission.Value);
            }

            if (PlayerPrefs.GetString(mission.name + "Clear") == "True")
            {
                mission.missionClear = true;

                if (PlayerPrefs.GetString(mission.name + "Complete") == "True")
                {
                    mission.missionComplete = true;
                }
                else
                {
                    mission.missionComplete = false;
                }
            }
            else
            {
                mission.missionClear = false;
                mission.missionComplete = false;
            }
        }
    }
    public void DataSave()
    {
        for (int i = 0; i < missionDic.Count; i++)
        {
            if (missionDic.TryGetValue((string)data_Mission[i]["Name"], out Mission mission))
            {
                PlayerPrefs.SetInt(mission.name + "Value", mission.Value);
                PlayerPrefs.SetString(mission.name + "Clear", mission.missionClear.ToString());
                PlayerPrefs.SetString(mission.name + "Complete", mission.missionComplete.ToString());
            }
        }
    }


    private void MissionSet()
    {
        data_Mission = CSVReader.Read("DailyMissionData");

        for (int i = 0; i < data_Mission.Count; i++)
        {
            missionDic.Add((string)data_Mission[i]["Name"], new Mission());
            missionDic[(string)data_Mission[i]["Name"]].name = (string)data_Mission[i]["Name"];
            missionDic[(string)data_Mission[i]["Name"]].Value = 0;
            missionDic[(string)data_Mission[i]["Name"]].hierarchyNum = i + 1;
            missionDic[(string)data_Mission[i]["Name"]].RewardNum = (int)data_Mission[i]["RewardNum"];
            missionDic[(string)data_Mission[i]["Name"]].TargetValue = (int)data_Mission[i]["Target"];
        }

        Transform contentPanel = GameObject.Find("Canvas").transform.Find("MissionPanel").
             transform.Find("Panel").transform.Find("ViewPort").transform.Find("ContentPanel");

        for (int i = 0; i < missionDic.Count; i++)
        {
            if (missionDic.TryGetValue((string)data_Mission[i]["Name"], out Mission mission))
            {
                missionSlotDic.Add(mission.name, contentPanel.transform.Find(mission.name + " Node").GetComponent<MissionSlot>());
            }
        }
    }

    public void MissionReset()
    {
        rewardGoldBox = RewardTypeDecision();

        for (int i = 0; i < data_Mission.Count; i++)
        {
            missionDic[(string)data_Mission[i]["Name"]].Value = 0;
            missionDic[(string)data_Mission[i]["Name"]].missionClear = false;
            missionDic[(string)data_Mission[i]["Name"]].missionComplete = false;
        }

        DataSave();
        MissionSlotSet();
        MissionSlotRange();
    }

    public void MissionSlotSet()
    {
        for (int i = 0; i < missionDic.Count; i++)
        {
            if (missionDic.TryGetValue((string)data_Mission[i]["Name"], out Mission mission))
            {
                if (missionSlotDic.TryGetValue(mission.name, out MissionSlot missionSlot))
                {
                    if (mission.missionComplete == false)
                    {

                        if (missionSlot.gameObject.activeSelf == false)
                        {
                            missionSlot.gameObject.SetActive(true);
                        }
                        missionSlot.rewardImage.sprite = rewardGoldBox.goldBoxImage;

                        missionSlot.rewardNumText.text = "X" + mission.RewardNum;
                        missionSlot.rewardProgressText.text = mission.Value + "/" + mission.TargetValue;
                        missionSlot.progressSlider.value = ((float)mission.Value / (float)mission.TargetValue);

                        if (mission.missionClear == false)
                        {
                            missionSlot.completeBut.enabled = false;
                            missionSlot.completeBut.GetComponent<Image>().sprite = nonClearButSprite;

                            missionSlot.backGroundImage.sprite = nonClearBgSprite;
                            missionSlot.backGroundImage.color = nonClearBgColor;
                        }
                        else
                        {
                            missionSlot.completeBut.enabled = true;
                            missionSlot.completeBut.GetComponent<Image>().sprite = clearButSprite;

                            missionSlot.backGroundImage.sprite = clearBgSprite; 
                            missionSlot.backGroundImage.color = clearBgColor;
                        }
                    }
                    else
                    {
                        missionSlot.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void CompleteOnClickSet()
    {
        for (int i = 0; i < missionDic.Count; i++)
        {
            if (missionDic.TryGetValue((string)data_Mission[i]["Name"], out Mission mission))
            {
                if (missionSlotDic.TryGetValue(mission.name, out MissionSlot missionSlot))
                {
                    missionSlot.completeBut.onClick.AddListener(delegate { completeButClick(mission.name); });
                }
            }
        }
    }

    public void RootMissionValueUp()
    {
        int missionIndex = UnityEngine.Random.Range(0, missionDic.Count);
        if (missionDic.TryGetValue((string)data_Mission[missionIndex]["Name"], out Mission mission))
        {
            MissionValueUp(mission.name, 5);
        }
    }

    public void MissionValueUp(string missionName, int plusValue)
    {
        ResetTimeCheck();
        if (missionDic.TryGetValue(missionName, out Mission mission))
        {
            if(mission.missionClear == false)
            {
                if (missionSlotDic.TryGetValue(missionName, out MissionSlot missionSlot))
                {
                    mission.Value += plusValue;

                    missionSlot.rewardProgressText.text = mission.Value + "/" + mission.TargetValue;
                    missionSlot.progressSlider.value = ((float)mission.Value / (float)mission.TargetValue);

                    MissionClearCheck(missionName);
                }
            }
        }
    }

    private void MissionClearCheck(string missionName)
    {
        if (missionDic.TryGetValue(missionName, out Mission mission))
        {
            if (mission.Value >= mission.TargetValue)
            {
                StartCoroutine(MissionClearEffect());
                mission.missionClear = true;
            }
        }
    }

    public void MissionCompleteClick(string missionName)
    {
        if (missionDic.TryGetValue(missionName, out Mission mission))
        {
            for(int i =0; i < mission.RewardNum; i++)
            {
                GoldBoxManager.Instance.inventory.ItemAdd(rewardGoldBox);
            }

            MissionComplete(missionName);
        }
    }

    public void MissionSlotRange()
    {
        for (int i = 0; i < missionDic.Count; i++)
        {
            if (missionSlotDic.TryGetValue((string)data_Mission[i]["Name"], out MissionSlot missionSlot))
            {
                if (missionSlot.completeBut.enabled == false)
                {
                    missionSlot.transform.SetAsLastSibling();
                }
                else
                {
                    missionSlot.transform.SetAsFirstSibling();
                }
            }
        }
    }

    IEnumerator MissionClearEffect()
    {
        missionClearImage.gameObject.SetActive(true);

        missionClearImage.color = imageOriginalColor;
        missionClearText.color = textOriginalColor;

        Color imageFadeColor = imageOriginalColor;
        Color textFadeColor = textOriginalColor;

        float c = imageOriginalColor.a;

        yield return new WaitForSeconds(2f);

        while (missionClearImage.color.a > 0.05f)
        {
            yield return null;
            c = Mathf.Lerp(c, 0, Time.deltaTime);

            imageFadeColor.a = c;
            textFadeColor.a = c;

            missionClearImage.color = imageFadeColor;
            missionClearText.color = textFadeColor;
        }

        missionClearImage.gameObject.SetActive(false);
    }

    void MissionComplete(string missionName)
    {
        if (missionDic.TryGetValue(missionName, out Mission mission))
        {
            mission.missionComplete = true;
            if (missionSlotDic.TryGetValue(missionName, out MissionSlot missionSlot))
            {
                missionSlot.gameObject.SetActive(false);
            }
        }
    }

    GoldBox RewardTypeDecision()
    {
        int goldBoxIndex = IslandManager.Instance.IslandPhase - 1;

        if(goldBoxIndex < 0)
        {
            goldBoxIndex = 1;
        }

        GoldBox returnGoldBox = GoldBoxManager.Instance.GoldBoxArray[goldBoxIndex];

        PlayerPrefs.SetInt("goldBoxIndex", goldBoxIndex);

        return returnGoldBox;
    }
}
        
   