using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    private Button rightArrow;
    private Button leftArrow;

    public Button goldBoxButton;
    public Button invenButton;
    public Button settingButton;
    public Button cameraButton;
    public Button missionButton;
    public Button adventureButton;

    public Button goldEventButton;
    public Button heartEventButton;

    public GameObject goldBoxPanel;
    private GameObject invenPanel;
    private GameObject adventurePanel;
    private GameObject settingPanel;
    public GameObject missionPanel;

    public Button goldBoxExitButton;
    public Button invenExitButton;
    public Button adventureExitButton;
    public Button settingExitButton;
    public Button missionExitButton;

    public Button followExitButton;


    private GameObject profilePanel;
    private Button profileExitButton;

    public GameObject followPanel;
    private AnimalState followAnimal;
    private Slider followIntimacySlider;
    //private Text followIntimacyText;
    private Text followAnimalNameText;
    private Text followLevelText;
    private Text followMaxHpText;

    private Text goldText;
    private Text energyText;

    public bool isPanelOn = false;

    private delegate void FollowModeOff(bool boolValue, AnimalState animalState);
    private FollowModeOff followModeOff;

    [SerializeField]
    private Button islandInvenBut;

    [SerializeField]
    private Button animalInvenBut;

    [SerializeField]
    private GameObject animalInven;

    [SerializeField]
    private GameObject islandInven;

    private Color selectColor = new Color(255,255,255,255);
    private Color nonSelectColor = new Color(255, 255, 255, 0);

    [SerializeField]
    private Text phaseText;

    [SerializeField]
    private GameObject phaseUI;



    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        ObjFind();
        OnClickSet();
        ArrowSet();
    }

    private void Start()
    {
        GoldTextUpdate(GameManager.Instance.Gold);
        EnergyTextUpdate(GameManager.Instance.Energy);
    }

    void ObjFind()
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        rightArrow = mainCanvas.transform.Find("RightArrow").GetComponent<Button>();
        leftArrow = mainCanvas.transform.Find("LeftArrow").GetComponent<Button>();

        goldBoxButton = GameObject.Find("GoldBoxButton").GetComponent<Button>();
        invenButton = GameObject.Find("InvenButton").GetComponent<Button>();
        settingButton = GameObject.Find("SettingButton").GetComponent<Button>();
        cameraButton = GameObject.Find("CameraButton").GetComponent<Button>();
        missionButton = GameObject.Find("MissionButton").GetComponent<Button>();
        adventureButton = GameObject.Find("AdventureButton").GetComponent<Button>();

        goldEventButton = GameObject.Find("GoldEventButton").GetComponent<Button>();
        heartEventButton = GameObject.Find("HeartEventButton").GetComponent<Button>();

        goldBoxPanel = GameObject.Find("PanelCanvas").transform.Find("GoldBoxPanel").gameObject;
        invenPanel = GameObject.Find("PanelCanvas").transform.Find("InvenPanel").gameObject;
        settingPanel = mainCanvas.transform.Find("SettingPanel").gameObject;
        missionPanel = mainCanvas.transform.Find("MissionPanel").gameObject;
        adventurePanel = GameObject.Find("PanelCanvas").transform.Find("AdventurePanel").gameObject;

        goldBoxExitButton = goldBoxPanel.transform.Find("Node").transform.Find("ExitButton").GetComponent<Button>();
        invenExitButton = GameObject.Find("PanelCanvas").transform.Find("InvenPanel").transform.Find("InvenExitButton").GetComponent<Button>();
        missionExitButton = missionPanel.transform.Find("ExitButton").GetComponent<Button>();
        settingExitButton = settingPanel.transform.Find("Node").transform.Find("Exit Button").GetComponent<Button>();
        adventureExitButton = GameObject.Find("PanelCanvas").transform.Find("AdventurePanel").transform.Find("Adventure Exit").GetComponent<Button>();

        //profilePanel = GameObject.Find("CanvasProfile").transform.Find("AnimalProfilePanel").gameObject;
        //profileExitButton = profilePanel.transform.Find("Node").transform.Find("ProfileExitButton").GetComponent<Button>();

        followPanel = GameObject.Find("Canvas").transform.Find("FollowProfilePanel").gameObject;
        followAnimalNameText = followPanel.transform.Find("FollowAnimalName").GetComponent<Text>();
        followIntimacySlider = followPanel.transform.Find("FollowIntimacySlider").GetComponent<Slider>();
        //followIntimacyText = followIntimacySlider.transform.Find("FollowIntimacyText").GetComponent<Text>();
        followLevelText = followPanel.transform.Find("FollowLevelText").GetComponent<Text>();
        followMaxHpText = followPanel.transform.Find("FollowMaxHpText").GetComponent<Text>();
        followExitButton = followPanel.transform.Find("FollowExitButton").GetComponent<Button>();


        goldText = GameObject.Find("GoldText").GetComponent<Text>();
        energyText = GameObject.Find("EnergyText").GetComponent<Text>();
    }

    public void OnClickSet()
    {
        rightArrow.onClick.AddListener(CameraManager.Instance.RightMoveIsland);
        leftArrow.onClick.AddListener(CameraManager.Instance.LeftMoveIsland);

        goldBoxButton.onClick.AddListener(GoldBoxPanelOn);
        invenButton.onClick.AddListener(InvenPanelOn);
        settingButton.onClick.AddListener(SettingPanelOn);
        missionButton.onClick.AddListener(MissionPanelOn);
        adventureButton.onClick.AddListener(AdventurePanelOn);

        goldBoxExitButton.onClick.AddListener(PanelOff);
        invenExitButton.onClick.AddListener(PanelOff);
        missionExitButton.onClick.AddListener(PanelOff);
        settingExitButton.onClick.AddListener(PanelOff);
        adventureExitButton.onClick.AddListener(PanelOff);

        followModeOff += FollowModeUIControl;
        followExitButton.onClick.AddListener(delegate { followModeOff(false, null); });
        followExitButton.onClick.AddListener(CameraManager.Instance.MainMode);

        animalInvenBut.onClick.AddListener(AnimalInvenOn);
        islandInvenBut.onClick.AddListener(IslandInvenOn);
    }

    public void ArrowSet()
    {
        if (CameraManager.Instance.isFollow == true)
        {
            rightArrow.gameObject.SetActive(false);
            leftArrow.gameObject.SetActive(false);

            return;
        }
        if (CameraManager.Instance.lookAtIsland == IslandManager.Instance.IslandArray[0])
        {
            if (IslandManager.Instance.IslandArray[1].IsSpawn == true)
            {
                rightArrow.gameObject.SetActive(true);
            }
            else
            {
                rightArrow.gameObject.SetActive(false);
            }
            leftArrow.gameObject.SetActive(false);
        }

        else if (CameraManager.Instance.lookAtIsland == IslandManager.Instance.IslandArray[1])
        {

            if (IslandManager.Instance.IslandArray[2].IsSpawn == true)
            {
                rightArrow.gameObject.SetActive(true);
            }
            else
            {
                rightArrow.gameObject.SetActive(false);
            }

            leftArrow.gameObject.SetActive(true);
        }

        else if (CameraManager.Instance.lookAtIsland == IslandManager.Instance.IslandArray[2])
        {
            if (IslandManager.Instance.IslandArray[3].IsSpawn == true)
            {
                rightArrow.gameObject.SetActive(true);
            }
            else
            {
                rightArrow.gameObject.SetActive(false);
            }
            leftArrow.gameObject.SetActive(true);
        }

        else if (CameraManager.Instance.lookAtIsland == IslandManager.Instance.IslandArray[3])
        {
            rightArrow.gameObject.SetActive(false);
            leftArrow.gameObject.SetActive(true);
        }
    } 

    public void ArrowOff()
    {
        rightArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
    }

    public void AnotherUIControl(bool setBool)
    {
        if(followPanel.activeSelf == false)
        {
            invenButton.gameObject.SetActive(setBool);
            goldBoxButton.gameObject.SetActive(setBool);
            adventureButton.gameObject.SetActive(setBool);
            missionButton.gameObject.SetActive(setBool);
            cameraButton.gameObject.SetActive(setBool);
            settingButton.gameObject.SetActive(setBool);
            goldEventButton.gameObject.SetActive(setBool);
            heartEventButton.gameObject.SetActive(setBool);
            ArrowOff();
        }
    }

    public void SettingPanelOn()
    {
        settingPanel.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFXSound("PanelOpenSound");
        isPanelOn = true;
        AnotherUIControl(false);
    }

    public void MissionPanelOn()
    {
        missionPanel.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFXSound("PanelOpenSound");
        isPanelOn = true;
        AnotherUIControl(false);
    }

    public void InvenPanelOn()
    {
        invenPanel.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFXSound("PanelOpenSound");
        isPanelOn = true;
        AnotherUIControl(false);
        AnimalInvenOn();
    }

    public void GoldBoxPanelOn()
    {
        goldBoxPanel.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFXSound("PanelOpenSound");
        isPanelOn = true;
        AnotherUIControl(false);
    }

    public void AdventurePanelOn()
    {
        adventurePanel.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFXSound("PanelOpenSound");
        isPanelOn = true;
        AnotherUIControl(false);
    }

    public void PanelOff()
    {
        isPanelOn = false;

        SoundManager.Instance.PlaySFXSound("PanelCloseSound");

        GameObject[] onPanel = GameObject.FindGameObjectsWithTag("Panel");

        for(int i =0; i<onPanel.Length; i++)
        {
            onPanel[i].SetActive(false);
        }
        AnotherUIControl(true);
        ArrowSet();
    }

    public void FollowModeUIControl(bool setBool,AnimalState touchedAnimal = null)
    {
        followAnimal = touchedAnimal;

        followExitButton.gameObject.SetActive(setBool);

        invenButton.gameObject.SetActive(!setBool);
        goldBoxButton.gameObject.SetActive(!setBool);
        adventureButton.gameObject.SetActive(!setBool);

        followPanel.gameObject.SetActive(setBool);

        if (setBool)
        {
            LocalizeSrcript local = followAnimalNameText.GetComponent<LocalizeSrcript>();
            local.textKey = touchedAnimal.name;
            local.LocalizeChanged();

            UpdateFollowIntimacy();
            UpdateFollowLevelText();
            UpdateFollowMaxHpText();
        }

        ArrowSet();
    }

    public IEnumerator SpawnUiMode(float spawnTime)
    {
        invenButton.gameObject.SetActive(false);
        goldBoxButton.gameObject.SetActive(false);
        adventureButton.gameObject.SetActive(false);

        ArrowOff();

        yield return new WaitForSeconds(spawnTime);

        if(followPanel.activeSelf == false)
        {
            invenButton.gameObject.SetActive(true);
            goldBoxButton.gameObject.SetActive(true);
            adventureButton.gameObject.SetActive(true);

            ArrowSet();
        }
    }

    public void UpdateFollowIntimacy()
    {
        //if (followAnimal.Level < AnimalState.maxLevel)
        //{
        //    followIntimacyText.text = "" + followAnimal.Intimacy + "/" + followAnimal.MaxIntimacy;
        //}
        //else if (followAnimal.Level >= AnimalState.maxLevel)
        //{
        //    followIntimacyText.text = "" + followAnimal.Intimacy;
        //}

        followIntimacySlider.value = (float) followAnimal.Intimacy / followAnimal.MaxIntimacy;
    }

    public void UpdateFollowLevelText()
    {
        followLevelText.text = "" + followAnimal.Level;
        UpdateFollowMaxHpText();
    }

    public void UpdateFollowMaxHpText()
    {
        followMaxHpText.text = "" + followAnimal.MaxHp;
    }

    public void PhaseTextUpdate(int phase)
    {
        phaseText.text = "" + phase;
    }

    public void GoldTextUpdate(int gold)
    {
        goldText.text = "" + gold;
    }

    public void EnergyTextUpdate(int energy)
    {
        energyText.text = energy + "/" + GameManager.Instance.MaxEnergy;
    }

    public void AnimalInvenOn()
    {
        animalInven.SetActive(true);
        islandInven.SetActive(false);
        animalInvenBut.GetComponentInChildren<Image>().color = selectColor;
        islandInvenBut.GetComponentInChildren<Image>().color = nonSelectColor;
    }

    public void IslandInvenOn()
    {
        animalInven.SetActive(false);
        islandInven.SetActive(true);
        animalInvenBut.GetComponentInChildren<Image>().color = nonSelectColor;
        islandInvenBut.GetComponentInChildren<Image>().color = selectColor;
    }
}
