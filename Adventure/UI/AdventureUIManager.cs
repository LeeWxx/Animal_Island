using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Advertisements;

public class AdventureUIManager : MonoBehaviour
{
    private static AdventureUIManager instance;

    public static AdventureUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdventureUIManager>();
            }

            return instance;
        }
    }

    public Button settingBut;
    public GameObject coinPanel;
    public Text coinText;
    public GameObject scorePanel;
    public Text scoreText;

    public GameObject pausePanel;

    public GameObject hpBar;

    public Transform hpLineTransform;

    private GameObject[] hpCellArray;
    private int hpIndex;
    private int maxHpIndex;
    public HorizontalLayoutGroup horizontalLayoutGroup;

    public GameObject rewardPanel;

    public Button restartButton;
    public Button homeButton;

    public GoldBoxSliderController goldBoxSliderController;

    public Text rewardGoldText;
    public TextMeshProUGUI rewardScoreText;
    public TextMeshProUGUI rewardBestScoreText;

    public Image goldBoxImage;

    public GameObject goldUiPanel;
    public Text goldText;
    public GameObject energyUiPanel;
    public Text energyText;

    public GameObject buffState;

    public GameObject revivePanel;
    public GameObject coinTwiceRewardBut;

    public Text reviveWaitText;

    public GameObject mainCanvas;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        OnclickSet();
    }

    private void OnclickSet()
    {
        homeButton.onClick.AddListener(AdventureQuit);
        restartButton.onClick.AddListener(Restart);
        settingBut.onClick.AddListener(PausePanelOn);
    }

    public void PausePanelOn()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }


    public void ScoreTextUpdate(int score)
    {
        scoreText.text = "" + score;
    }

    public void CoinTextUpdate(int coin)
    {
        coinText.text = "" + coin;
    }

    public IEnumerator HpBarSet(int MaxHp)
    {
        horizontalLayoutGroup.enabled = true;

        int maxHpCell = (MaxHp / 10);

        hpCellArray = new GameObject[maxHpCell];

        for (int i = 1; i < maxHpCell + 1; i++)
        {
            hpCellArray[i - 1] = hpLineTransform.Find("HpCell " + "(" + i + ")").gameObject;
            hpCellArray[i - 1].SetActive(true);
        }

        maxHpIndex = maxHpCell - 1;
        hpIndex = maxHpIndex;
        yield return null;

        horizontalLayoutGroup.enabled = false;
    }

    public void HpMinusUpdate(int minusHp)
    {
        int minusHpCell = (minusHp / 10);

        for (int i = 0; i < minusHpCell; i++)
        {
            if (hpIndex >= 0)
            {
                hpCellArray[hpIndex].SetActive(false);
                hpIndex -= 1;
            }
        }
    }

    public void HpPlusUpdate(int plusHp)
    {
        int plusHpCell = (plusHp / 10);

        for (int i = 0; i < plusHpCell; i++)
        {
            if (hpIndex < maxHpIndex)
            {
                hpIndex += 1;
                hpCellArray[hpIndex].SetActive(true);
            }
        }
    }

    public void RewardPanelOn(int score, int coin)
    {
        mainCanvas.gameObject.SetActive(false);

        if (Advertisement.IsReady("Rewarded_Android"))
        {
            coinTwiceRewardBut.gameObject.SetActive(true);
        }
        else
        {
            coinTwiceRewardBut.gameObject.SetActive(false);
        }

        float rewardTime = 3f;
        coinPanel.SetActive(false);
        scorePanel.SetActive(false);

        goldBoxSliderController.presentGoldBoxSlider.gameObject.SetActive(false);

        if (goldBoxSliderController.PresentBoxIndex > 0)
        {
            GoldBox getGoldBox = GoldBoxManager.Instance.GoldBoxArray[goldBoxSliderController.PresentBoxIndex - 1];
            goldBoxImage.sprite = getGoldBox.goldBoxImage;
            goldBoxImage.transform.localScale = getGoldBox.inSlotSacle * 4;
            GoldBoxManager.Instance.GoldBoxAdd(goldBoxSliderController.PresentBoxIndex - 1);
        }

        StartCoroutine(AdventureParticleManager.Instance.RewardBoxEffectPlay(goldBoxSliderController.PresentBoxIndex, goldBoxImage));

        rewardGoldText.text = "+" + coin;
        rewardPanel.gameObject.SetActive(true);

        rewardBestScoreText.text = "" + GameManager.Instance.AdventureBestRecord;
        goldUiPanel.SetActive(true);
        energyUiPanel.SetActive(true);
        energyText.text = GameManager.Instance.Energy + "/" + GameManager.Instance.MaxEnergy;
        StartCoroutine(CoinReward(rewardTime,coin));
        StartCoroutine(ScoreReward(rewardTime, score));
    }

    IEnumerator CoinReward(float duration,int coin)
    {
        double coinOffset = coin / duration;
        double coinCurrent = GameManager.Instance.Gold;
        double coinTarget = GameManager.Instance.Gold + coin;
        GameManager.Instance.GoldPlus(coin);

        SoundManager.Instance.PlaySFXSound("RewardSound");

        while (coinCurrent < coinTarget)
        {
            coinCurrent += coinOffset * Time.deltaTime;
            goldText.text = ((int)coinCurrent).ToString();
            yield return null;
        }

        coinCurrent = coinTarget;

        goldText.text = ((int)coinCurrent).ToString();

        SoundManager.Instance.PlaySFXSound("RewardFinishSound");
    }

    IEnumerator ScoreReward(float duration, int score)
    {
        double scoreOffset = score / duration;
        double scoreCurrent = 0;
        double scoreTarget = score;

        while (scoreCurrent < scoreTarget)
        {
            scoreCurrent += scoreOffset * Time.deltaTime;
            rewardScoreText.text = ((int)scoreCurrent).ToString();
            yield return null;
        }

        scoreCurrent = scoreTarget;

        rewardScoreText.text = ((int)scoreCurrent).ToString();

        if (GameManager.Instance.BestRecordSet(score))
        {
            rewardBestScoreText.text = "" + GameManager.Instance.AdventureBestRecord;
        }
    }

    public void AdventureQuit()
    {
        GameObject animal = FindObjectOfType<PlayerMovement>().gameObject;
        Destroy(animal);
        SceneManager.LoadScene("Main");
    }

    public void Restart()
    {
        if (GameManager.Instance.EnergyMinus(1))
        {
            energyText.text = GameManager.Instance.Energy + "/" + GameManager.Instance.MaxEnergy;
            GameObject animal = FindObjectOfType<PlayerMovement>().gameObject;
            SceneManager.LoadScene("Adventure");
            Destroy(animal);
        }
    }

    public void RevivePanelOn()
    {
        revivePanel.gameObject.SetActive(true);
    }

    public void ReviveButClick()
    {
        AdsManager.Instance.ShowRewardedAd();
    }

    public void RewardCoinTwiceButClick()
    {
        AdsManager.Instance.ShowRewardedAd();
    }

    public IEnumerator RewardCoinTwice()
    {
        int coin = AdventureManager.Instance.GameOverCoin;

        coinTwiceRewardBut.gameObject.SetActive(false);

        float duration = 3f;

        float coinOffset = coin / duration;
        float coinCurrent = GameManager.Instance.Gold;
        float coinTarget = GameManager.Instance.Gold + coin;

        SoundManager.Instance.PlaySFXSound("RewardSound");

        while (coinCurrent < coinTarget)
        {
            coinCurrent += coinOffset * Time.deltaTime;

            goldText.text = ((int)coinCurrent).ToString();
            yield return null;
        }

        coinCurrent = coinTarget;

        goldText.text = ((int)coinCurrent).ToString();


        GameManager.Instance.GoldPlus(coin);
        SoundManager.Instance.PlaySFXSound("RewardFinishSound");
    }
}
