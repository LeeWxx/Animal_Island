using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int gold = 0;

    public int Gold
    {
        get { return gold; }
    }

    private int energy = 5;

    public int Energy
    {
        get { return energy; }
    }

    private int maxEnergy = 5;

    public int MaxEnergy
    {
        get { return maxEnergy; }
    }

    private int playerPhase = 0;

    public int PlayerPhase
    {
        get { return playerPhase; }
    }

    private int adventureBestRecord;

    public int AdventureBestRecord
    {
        get { return adventureBestRecord; }
    }

    static bool isFirstMainScene = true;

    public TerminationWarningPanel terminationWarningPanel;


    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {

            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
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

        DataLoad();

        if (isFirstMainScene == true)
        {
            isFirstMainScene = false;

            if (PlayerPrefs.GetInt("TutorialEnd") != 0)
            {
                terminationWarningPanel.gameObject.SetActive(true);
            }
        }
    }

    void DataLoad()
    {
        gold = PlayerPrefs.GetInt("Gold", gold);
        UIManager.Instance.GoldTextUpdate(gold);

        energy = PlayerPrefs.GetInt("Energy", energy);
        UIManager.Instance.EnergyTextUpdate(energy);

        playerPhase = PlayerPrefs.GetInt("PlayerPhase");
        UIManager.Instance.PhaseTextUpdate(playerPhase);

        adventureBestRecord = PlayerPrefs.GetInt("AdventureBestRecord");
    }

    private void OnApplicationQuit()
    {
        GameDataSave();   
    }

    public void GameDataSave()
    {
        AnimalManager.Instance.DataSave();
        IslandManager.Instance.DataSave();

        PlayerPrefs.SetInt("PlayerPhase", playerPhase);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Energy", energy);
        PlayerPrefs.SetInt("AdventureBestRecord", adventureBestRecord);
    }

    public void PlayerPhaseUp()
    {
        playerPhase += 1;
        UIManager.Instance.PhaseTextUpdate(playerPhase);
    }

    public void GoldPlus(int plusGold)
    {
        gold += plusGold;

        if (SceneManager.GetActiveScene().name == "Main")
        {
            UIManager.Instance.GoldTextUpdate(gold);
        }
    }

    public void GoldMinus(int minusGold)
    {
        gold -= minusGold;

        UIManager.Instance.GoldTextUpdate(gold);
        MissionManager.Instance.MissionValueUp("Coin consumtion", minusGold);
    }

    public void EnergyPlus(int plusEnergy = 1)
    {
        if(energy < maxEnergy)
        {
            energy += plusEnergy;

            if (SceneManager.GetActiveScene().name == "Main")
            {
                UIManager.Instance.EnergyTextUpdate(energy);
            }
        }
    }

    public bool EnergyMinus(int minusEnergy)
    {
        if(energy > 0)
        {
            energy -= minusEnergy;
            if (SceneManager.GetActiveScene().name == "Main")
            {
                UIManager.Instance.EnergyTextUpdate(energy);
            }
            return true;
        }

        return false;
    }

    public bool BestRecordSet(int score)
    {
        if(score > adventureBestRecord)
        {
            adventureBestRecord = score;
            return true;
        }
        else
        {
            return false;
        }
    }

    [ContextMenu("리셋")]
    public void AllReset()
    {
        PlayerPrefs.DeleteAll();
    }


    [ContextMenu("에너지 추가")]
    public void Test()
    {
        GoldMinus(0);
        EnergyPlus(1);
    }
}
