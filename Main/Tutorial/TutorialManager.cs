using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialManager>();
            }

            return instance;
        }
    }

    bool tutorialEnd;
    bool phaseCheck;

    public bool PhaseCheck;

    static int tutorialIndex = 0;

    List<Dictionary<string, object>> data_Tutorial;

    public MainSceneTutorial mainSceneTutorial;

    public MainSceneTutorial MainSceneTutorial
    {
        get 
        { 
            if(mainSceneTutorial == null)
            {
                mainSceneTutorial = FindObjectOfType<MainSceneTutorial>();
            }

            return mainSceneTutorial;
        }
    }

    public AdventureSceneTutorial adventureSceneTutorial;

    public WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    public float changeTimeInterval = 0.5f;

    public bool isFirstMainSceneCheck = true;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance != this)
        {
            Destroy(this.gameObject);
        }


        //if(tutorialIndex == 0)
        //{
        //    tutorialIndex = 1;
        //}

        if(PlayerPrefs.GetInt("TutorialEnd") == 0)
        {
            tutorialEnd = false;
        }

        else if (PlayerPrefs.GetInt("TutorialEnd") == 1)
        {
            tutorialEnd = true;
        }

        if (tutorialEnd == false)
        {
            phaseCheck = false;
            data_Tutorial = CSVReader.Read("TutorialData");
        }
        
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator TutorialProceed()
    {
        while (tutorialEnd == false)
        {
            if (phaseCheck == false)
            {
                string dialouge = data_Tutorial[tutorialIndex]["Dialogue"].ToString();
                string behavior = data_Tutorial[tutorialIndex]["Behavior"].ToString();
                string sceneName = data_Tutorial[tutorialIndex]["SceneName"].ToString();

                if(sceneName == "Main")
                {
                    MainSceneTutorial.TutorialProceed(dialouge, behavior);
                }
                else if(sceneName == "Adventure")
                {
                    adventureSceneTutorial.TutorialProceed(dialouge, behavior);
                }

                phaseCheck = true;
            }
            yield return null;
        }
    }

    public void PhaseClear()
    {
        phaseCheck = false;
        tutorialIndex++;
    }

    public IEnumerator ClickInduceCoroutine(Image induceBut, GameObject targetObj)
    {
        Color basicOriginalColor = induceBut.color;
        Color basicFadeColor = new Color(induceBut.color.r, induceBut.color.g, induceBut.color.b, induceBut.color.a * 0.5f);

        induceBut.transform.SetAsLastSibling();

        float time;
        while (targetObj.activeSelf == false)
        {
            induceBut.color = basicFadeColor;

            time = Time.time + changeTimeInterval;
            while (targetObj.activeSelf == false && Time.time <= time)
            {
                yield return waitTime;
            }

            induceBut.color = basicOriginalColor;
            time = Time.time + changeTimeInterval;
            while (targetObj.activeSelf == false && Time.time <= time)
            {
                yield return waitTime;
            }
        }

        induceBut.color = basicOriginalColor;
        induceBut.transform.SetAsFirstSibling();

        PhaseClear();
    }

    public void TutorialEnd()
    {
        tutorialEnd = true;
        PlayerPrefs.SetInt("TutorialEnd", 1);

        UIManager.Instance.missionButton.onClick.RemoveAllListeners();
        UIManager.Instance.missionButton.onClick.AddListener(UIManager.Instance.MissionPanelOn);

        UIManager.Instance.adventureButton.enabled = true;
        UIManager.Instance.missionButton.enabled = true;
        UIManager.Instance.invenButton.enabled = true;
        UIManager.Instance.heartEventButton.enabled = true;
        UIManager.Instance.goldEventButton.enabled = true;

        Destroy(mainSceneTutorial.gameObject);
    }

}
