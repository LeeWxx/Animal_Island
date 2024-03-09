using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    private static AdsManager instance;

    public static AdsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdsManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
#if UNITY_ANDROID
        string gameId = "4465845";
        Advertisement.Initialize(gameId, true);
#endif

//#if UNITY_IOS
//        string gameId = "4465844";
//        Advertisement.Initialize(gameId, true);
//#endif


    }
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("Rewarded_Android"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("Rewarded_Android", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                if (SceneManager.GetActiveScene().name == "Main")
                {
                    if (EventManager.Instance.nowPressBtn == EventManager.NowPressBtn.GoldEvent)
                    {
                        StartCoroutine(EventManager.Instance.GoldEvent());
                    }

                    else if (EventManager.Instance.nowPressBtn == EventManager.NowPressBtn.HeartEvent)
                    {
                        StartCoroutine(EventManager.Instance.HeartEvent());
                    }
                }

                else if (SceneManager.GetActiveScene().name == "Adventure")
                {
                    if(AdventureUIManager.Instance.rewardPanel.activeSelf == true)
                    {
                        StartCoroutine(AdventureUIManager.Instance.RewardCoinTwice());
                    }
                    else if (AdventureUIManager.Instance.revivePanel.activeSelf == true)
                    {
                        StartCoroutine(AdventureManager.Instance.Revive());
                    }
                }

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
