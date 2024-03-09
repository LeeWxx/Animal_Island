using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneTutorial : MonoBehaviour
{
    TutorialManager tutorialManager;

    public GameObject languagePanel;

    public GameObject mainInducePanel;
    public GameObject invenInducePanel;

    public Image invenBut;
    public GameObject invenPanel;

    public Image islandBut;
    public GameObject islandPanel;

    public Image islandPurchaseBut;

    public Image invenExitBut;

    public Image animalPurchaseBut;
    public GameObject animalInven;

    public Image exitArrow;

    public Image adventureBut;
    public GameObject adventurePanel;

    public GameObject adventureAnimalList;
    public Image animalAdventureBut;

    public GameObject speechBubble;
    private Text speechBubbleText;
    LocalizeSrcript localText;

    public GameObject tutorialInducePanel;
    public Image missionBut;
    public GameObject tutorialMissionNode;
    public Image tutorialMissionClearBut;
    public Image tutorialMissionPanelExitBut;
    public Image goldBoxBut;
    public GameObject tutorialMissionPanel;

    public Image goldBoxOpenBut;
    public GameObject getGoldPanel;
    public Image goldBoxExitBut;


    private Color basicFadeColor = new Color(1f, 1f, 1f, 0.5f);
    private Color basicOriginalColor = new Color(1f, 1f, 1f, 1f);


    bool screenTouchCheck;
    bool screenTouchStandBy;

    bool isAdventureButClick;
    bool isTutorialMissionClear;

    public TerminationWarningPanel terminationWarningPanel;

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialEnd") == 0)
        {
            UIManager.Instance.missionButton.enabled = false;
            UIManager.Instance.goldEventButton.enabled = false;
            UIManager.Instance.heartEventButton.enabled = false;


            tutorialManager = TutorialManager.Instance;
            tutorialManager.mainSceneTutorial = this;

            screenTouchCheck = false;
            speechBubbleText = speechBubble.GetComponentInChildren<Text>();
            speechBubble.gameObject.SetActive(true);

            localText = speechBubbleText.GetComponent<LocalizeSrcript>();

            StartCoroutine(tutorialManager.TutorialProceed());

            if(tutorialManager.isFirstMainSceneCheck == false)
            {
                tutorialManager.PhaseClear();
            }
        }
        else
        {
            Destroy(speechBubble.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void TutorialProceed(string dialouge, string behavior)
    {
        if (dialouge == "Empty")
        {
            if (speechBubble.activeSelf == true)
            {
                speechBubble.gameObject.SetActive(false);
            }
        }
        else
        {
            if (speechBubble.activeSelf == false)
            {
                speechBubble.gameObject.SetActive(true);
            }
            localText.textKey = dialouge;
            localText.LocalizeChanged();
        }


        StartCoroutine(behavior);
    }

    private IEnumerator LanguageSelect()
    {
        languagePanel.SetActive(true);
        yield return null;
    }

    public IEnumerator TerminationWarning()
    {
        languagePanel.gameObject.SetActive(false);
        terminationWarningPanel.gameObject.SetActive(true);
        while (terminationWarningPanel.gameObject.activeSelf == true)
        {
            yield return null;
        }

        tutorialManager.PhaseClear();
    }

    public void TutorialStart()
    {
        languagePanel.SetActive(false);
        tutorialManager.PhaseClear();
    }

    private IEnumerator MainTutorialPanelOn()
    {
        mainInducePanel.SetActive(true);
        if (invenInducePanel.activeSelf == true)
        {
            invenInducePanel.SetActive(false);
        }

        yield return null;
        tutorialManager.PhaseClear();
    }

    private IEnumerator InvenTutorialPanelOn()
    {
        invenInducePanel.SetActive(true);
        if (mainInducePanel.activeSelf == true)
        {
            mainInducePanel.SetActive(false);
        }

        yield return null;
        tutorialManager.PhaseClear();
    }

    private IEnumerator ScreenTouchStandBy()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.01f);
        float changeTimeInterval = 1f;
        Image  inducePanel;

        if(mainInducePanel.activeSelf == true)
        {
            inducePanel = mainInducePanel.GetComponent<Image>();
        }
        else if(invenInducePanel.activeSelf == true)
        {
            inducePanel = invenInducePanel.GetComponent<Image>();
        }
        else
        {
            inducePanel = null;
        }

        screenTouchStandBy = true;
        Color basicOriginalColor = inducePanel.color;
        Color basicFadeColor = new Color(inducePanel.color.r, inducePanel.color.g, inducePanel.color.b, inducePanel.color.a * 0.8f);

        float time;

        while (screenTouchCheck == false)
        {
            inducePanel.color = basicFadeColor;

            time = Time.time + changeTimeInterval;
            while (screenTouchCheck == false && Time.time <= time)
            {
                yield return waitTime;
            }

            inducePanel.color = basicOriginalColor;
            time = Time.time + changeTimeInterval;
            while (screenTouchCheck == false && Time.time <= time)
            {
                yield return waitTime;
            }

            yield return null;
        }

        inducePanel.color = basicOriginalColor;

        screenTouchCheck = false;
        screenTouchStandBy = false;
        tutorialManager.PhaseClear();
    }

    public void ScreenTouch()
    {
        if (screenTouchStandBy == true)
        {
            screenTouchCheck = true;
        }
    }

    private IEnumerator InvenButtonInduce()
    {
        yield return null;
        StartCoroutine(tutorialManager.ClickInduceCoroutine(invenBut, invenPanel));
    }

    private IEnumerator IslandBarInduce()
    {
        yield return null;

        Color basicOriginalColor = islandBut.color;
        Color basicFadeColor = new Color(islandBut.color.r, islandBut.color.g, islandBut.color.b, islandBut.color.a * 0.5f);

        invenInducePanel.transform.SetParent(islandBut.transform.parent.parent);
        islandBut.transform.parent.transform.SetAsLastSibling();

        float time;
        while (islandPanel.activeSelf == false)
        {
            islandBut.color = basicFadeColor;

            time = Time.time + tutorialManager.changeTimeInterval;
            while (islandPanel.activeSelf == false && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }

            islandBut.color = basicOriginalColor;
            time = Time.time + tutorialManager.changeTimeInterval;
            while (islandPanel.activeSelf == false && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }
        }

        islandBut.color = basicOriginalColor;

        tutorialManager.PhaseClear();

        islandBut.transform.parent.transform.SetAsFirstSibling();
    }

    private IEnumerator IslandPurchaseButtonInduce()
    {
        invenInducePanel.transform.SetParent(islandPurchaseBut.transform.parent.parent);
        invenInducePanel.transform.SetAsLastSibling();
        islandPurchaseBut.transform.parent.transform.SetAsLastSibling();

        yield return null;

        Color basicOriginalColor = islandPurchaseBut.color;
        Color basicFadeColor = new Color(islandPurchaseBut.color.r, islandPurchaseBut.color.g, islandPurchaseBut.color.b, islandPurchaseBut.color.a * 0.5f);

        float time;
        while (invenPanel.gameObject.activeSelf == true)
        {
            islandPurchaseBut.color = basicFadeColor;

            time = Time.time + tutorialManager.changeTimeInterval;
            while (invenPanel.gameObject.activeSelf == true && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }

            islandPurchaseBut.color = basicOriginalColor;
            time = Time.time + tutorialManager.changeTimeInterval;
            while (invenPanel.gameObject.activeSelf == true && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }
        }

        islandPurchaseBut.color = basicOriginalColor;

        invenInducePanel.transform.SetParent(invenPanel.transform);
        speechBubble.gameObject.SetActive(false);
        tutorialManager.PhaseClear();
    }

    private IEnumerator InvenExitButtonInduce()
    {
        yield return null;
        invenExitBut.transform.parent.transform.SetAsLastSibling();
        StartCoroutine(tutorialManager.ClickInduceCoroutine(invenExitBut, invenBut.gameObject));
    }

    private IEnumerator TutorialPanelOff()
    {
        yield return null;
        invenInducePanel.gameObject.SetActive(false);
        mainInducePanel.gameObject.SetActive(false);
        tutorialManager.PhaseClear();
    }

    private IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(1f);
        tutorialManager.PhaseClear();
    }

    private IEnumerator IslandClickInduce()
    {
        int targetGold = GameManager.Instance.Gold;

        while (targetGold >= GameManager.Instance.Gold)
        {
            yield return null;
        }

        tutorialManager.PhaseClear();
    }

    private IEnumerator AnotherUiOn()
    {
        UIManager.Instance.AnotherUIControl(true);
        yield return null;
        tutorialManager.PhaseClear();
    }

    private IEnumerator AnotherUiOff()
    {
        UIManager.Instance.AnotherUIControl(false);
        yield return null;
        tutorialManager.PhaseClear();
    }

    private IEnumerator TouchBlocking()
    {
        UIManager.Instance.isPanelOn = true;
        yield return null;
        tutorialManager.PhaseClear();
    }

    private IEnumerator TouchBlockingOff()
    {
        UIManager.Instance.isPanelOn = false;
        yield return null;
        tutorialManager.PhaseClear();
    }

    private IEnumerator AnimalPurchaseButtonInduce()
    {
        invenInducePanel.transform.SetParent(animalInven.transform.parent);
        invenInducePanel.transform.SetAsLastSibling();
        animalInven.transform.SetAsLastSibling();

        yield return null;

        Color basicOriginalColor = animalPurchaseBut.color;
        Color basicFadeColor = new Color(animalPurchaseBut.color.r, animalPurchaseBut.color.g, animalPurchaseBut.color.b, animalPurchaseBut.color.a * 0.5f);

        float time;
        while (invenPanel.gameObject.activeSelf == true)
        {
            animalPurchaseBut.color = basicFadeColor;

            time = Time.time + tutorialManager.changeTimeInterval;
            while (invenPanel.gameObject.activeSelf == true && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }

            animalPurchaseBut.color = basicOriginalColor;
            time = Time.time + tutorialManager.changeTimeInterval;
            while (invenPanel.gameObject.activeSelf == true && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }
        }

        islandPurchaseBut.color = basicOriginalColor;

        invenInducePanel.transform.SetParent(invenPanel.transform);
        speechBubble.gameObject.SetActive(false);
        tutorialManager.PhaseClear();
    }

    private IEnumerator AnimalClickInduce()
    {
        yield return null;
        AnimalState rabbitState = AnimalManager.Instance.animalArray[0].GetComponent<AnimalState>();

        int targetIntimacy = rabbitState.Intimacy + 1;

        while (targetIntimacy > rabbitState.Intimacy)
        {
            yield return null;
        }

        tutorialManager.PhaseClear();
    }

    private IEnumerator ZoomExitInduce()
    {
        TouchManager.Instance.enabled = false;
        yield return null;
        StartCoroutine(tutorialManager.ClickInduceCoroutine(exitArrow, invenBut.gameObject));
    }

    private IEnumerator AdventureButtonInduce()
    {
        yield return null;
        StartCoroutine(tutorialManager.ClickInduceCoroutine(adventureBut, adventurePanel));
    }

    private IEnumerator AdventureStartInduce()
    {
        isAdventureButClick = false;

        Button tutorialAdventureBut = animalAdventureBut.GetComponent<Button>();
        tutorialAdventureBut.onClick.RemoveAllListeners();
        tutorialAdventureBut.onClick.AddListener(TutorialAdventure);

        TouchManager.Instance.enabled = true;
        invenInducePanel.transform.SetParent(adventureAnimalList.transform.parent);
        invenInducePanel.transform.SetAsLastSibling();
        adventureAnimalList.transform.SetAsLastSibling();
        yield return null;

        Color basicOriginalColor = animalAdventureBut.color;
        Color basicFadeColor = new Color(animalAdventureBut.color.r, animalAdventureBut.color.g, animalAdventureBut.color.b, animalAdventureBut.color.a * 0.5f);

        float time;
        while (isAdventureButClick == false)
        {
            animalAdventureBut.color = basicFadeColor;

            time = Time.time + tutorialManager.changeTimeInterval;
            while (isAdventureButClick == false && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }

            animalAdventureBut.color = basicOriginalColor;
            time = Time.time + tutorialManager.changeTimeInterval;
            while (isAdventureButClick == false && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }
        }

        animalAdventureBut.color = basicOriginalColor;
    }

    private void TutorialAdventure()
    {
        tutorialManager.isFirstMainSceneCheck = false;

        isAdventureButClick = true;
        UIManager.Instance.PanelOff();
        AdventureSetting.Instance.AdventureAnimalSet("Rabbit");
        SceneManager.LoadScene("TutorialAdventure");
        GameManager.Instance.GameDataSave();
    }

    private IEnumerator AnimalZoomeInduce()
    {
        UIManager.Instance.invenButton.enabled = false;
        UIManager.Instance.adventureButton.enabled = false;
        UIManager.Instance.goldBoxButton.enabled = false;

        while(CameraManager.Instance.isFollow == false)
        {
            yield return null;
        }

        tutorialManager.PhaseClear();
    }

    private IEnumerator AnimalLevelUpWait()
    {
        AnimalState rabbitState = AnimalManager.Instance.animalArray[0].GetComponent<AnimalState>();

        UIManager.Instance.followExitButton.enabled = false;

        while (rabbitState.Level == 1)
        {
            yield return null;
        }

        UIManager.Instance.followExitButton.enabled = true;
        mainInducePanel.SetActive(true);
        mainInducePanel.transform.SetParent(UIManager.Instance.followPanel.transform);
        mainInducePanel.transform.SetAsLastSibling();
        tutorialManager.PhaseClear();
    }

    private IEnumerator MissionPanelInduce()
    {
        UIManager.Instance.missionButton.enabled = true;

        UIManager.Instance.missionButton.onClick.RemoveAllListeners();
        UIManager.Instance.missionButton.onClick.AddListener(TutorialMissionPanelOn);

        yield return null;
        mainInducePanel.transform.SetParent(missionBut.transform.parent);
        mainInducePanel.transform.SetAsLastSibling();
        mainInducePanel.gameObject.SetActive(true);
        StartCoroutine(tutorialManager.ClickInduceCoroutine(missionBut, tutorialMissionPanel));
    }

    private IEnumerator MissionClearlnduce()
    {
        mainInducePanel.SetActive(false);

        yield return null;

        Color basicOriginalColor = tutorialMissionClearBut.color;
        Color basicFadeColor = new Color(tutorialMissionClearBut.color.r, tutorialMissionClearBut.color.g, tutorialMissionClearBut.color.b, tutorialMissionClearBut.color.a * 0.5f);

        float time;
        while (isTutorialMissionClear == false)
        {
            tutorialMissionClearBut.color = basicFadeColor;

            time = Time.time + tutorialManager.changeTimeInterval;
            while (isTutorialMissionClear == false && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }

            tutorialMissionClearBut.color = basicOriginalColor;
            time = Time.time + tutorialManager.changeTimeInterval;
            while (isTutorialMissionClear == false && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }
        }

        tutorialManager.PhaseClear();
    }

    public void TutorialMissionPanelOn()
    {
        tutorialMissionPanel.SetActive(true);
        UIManager.Instance.AnotherUIControl(false);
    }

    public void TutorialMissionPanelOff()
    {
        tutorialMissionPanel.SetActive(false);
        UIManager.Instance.AnotherUIControl(true);
    }

    public void TutorialMissionClear()
    {
        isTutorialMissionClear = true;
        tutorialMissionNode.SetActive(false);
    }

    private IEnumerator MissionPanelExitInduce()
    {
        yield return null;

        tutorialInducePanel.transform.SetParent(tutorialMissionPanelExitBut.transform.parent);
        tutorialInducePanel.transform.SetAsLastSibling();
        tutorialMissionPanelExitBut.transform.SetAsLastSibling();

        StartCoroutine(tutorialManager.ClickInduceCoroutine(tutorialMissionPanelExitBut, invenBut.gameObject));

        GoldBoxManager.Instance.GoldBoxAdd(0);
    }

    private IEnumerator GoldBoxPanelnduce()
    {
        UIManager.Instance.goldBoxButton.enabled = true;
        yield return null;
        mainInducePanel.SetActive(true);
        mainInducePanel.transform.SetAsLastSibling();
        StartCoroutine(tutorialManager.ClickInduceCoroutine(goldBoxBut, UIManager.Instance.goldBoxPanel));
    }

    private IEnumerator GoldBoxInducePanelOn()
    {
        yield return null;
        StartCoroutine(InvenTutorialPanelOn());
    }

    private IEnumerator GiveGoldBox()
    {
        goldBoxOpenBut.gameObject.GetComponent<Button>().enabled = false;
        yield return null;
        GoldBoxManager.Instance.GoldBoxAdd(0);
        tutorialManager.PhaseClear();
    }


    private IEnumerator BoxMergeInduce()
    {
        invenInducePanel.transform.SetAsFirstSibling();

        Slot slot1 = GoldBoxManager.Instance.inventory.slots[0];
        Slot slot2 = GoldBoxManager.Instance.inventory.slots[1];

        GoldBox targetGoldBox = GoldBoxManager.Instance.GoldBoxArray[1];

        while(slot1.slotGoldBox.name != targetGoldBox.name && slot2.slotGoldBox.name != targetGoldBox.name)
        {
            yield return null;
        }

        tutorialManager.PhaseClear();
    }

    private IEnumerator BoxOpenInduce()
    {
        goldBoxOpenBut.gameObject.GetComponent<Button>().enabled = true;
        yield return null;
        invenInducePanel.transform.SetParent(goldBoxOpenBut.transform.parent);
        invenInducePanel.transform.SetAsLastSibling();
        goldBoxOpenBut.transform.SetAsLastSibling();
        StartCoroutine(tutorialManager.ClickInduceCoroutine(goldBoxOpenBut, getGoldPanel));
    }

    private IEnumerator GetGoldFocus()
    {
        yield return null;
        invenInducePanel.transform.SetParent(getGoldPanel.transform.parent);
        invenInducePanel.transform.SetAsLastSibling();
        getGoldPanel.transform.SetAsLastSibling();

        tutorialManager.PhaseClear();
    }

    private IEnumerator GoldBoxPaneExitInduce()
    {
        yield return null;
        StartCoroutine(tutorialManager.ClickInduceCoroutine(goldBoxExitBut, invenBut.gameObject));

        invenInducePanel.transform.SetParent(goldBoxExitBut.transform.parent.parent);
        invenInducePanel.transform.SetAsLastSibling();
        goldBoxExitBut.transform.parent.SetAsLastSibling();

        speechBubble.transform.SetAsLastSibling();
    }

    private IEnumerator TutorialEnd()
    {
        yield return null;

        Destroy(speechBubble.gameObject);
        Destroy(invenInducePanel);
        Destroy(mainInducePanel);

        Button tutorialAdventureBut = animalAdventureBut.GetComponent<Button>();
        tutorialAdventureBut.onClick.RemoveAllListeners();

        TouchManager.Instance.enabled = true;

        tutorialManager.TutorialEnd();
    }
}
