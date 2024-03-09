using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    private static IslandManager instance;

    Island[] islandArray = new Island[4];

    public Island[] IslandArray
    {
        get { return islandArray; }
    }

    public int islandInterval = 500;

    private int islandPhase = 1;

    public int IslandPhase
    {
        get { return islandPhase; }
    }

    private int spawnTime = 3;

    public static IslandManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<IslandManager>();
            }

            return instance;
        }
    }

    List<Dictionary<string, object>> data_Island;

    public List<Dictionary<string, object>> Data_Island
    {
        get { return data_Island; }
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        data_Island = CSVReader.Read("IslandData");

        ArraySet();
        DataLoad();
        SpawnCheck();
    }

    public void ArraySet()
    {
        for(int i =0; i<islandArray.Length;i++)
        {
            islandArray[i] = GameObject.Find("Island" + (i+1)).GetComponent<Island>();
            islandArray[i].islandNum = i;
            islandArray[i].Price = (int)data_Island[i]["Price"];
            islandArray[i].NeedPhase = (int)data_Island[i]["NeedPhase"];
            islandArray[i].name = (string)data_Island[i]["Name"];
            islandArray[i].islandPosX = (int)data_Island[i]["PosX"];
        }
    }

    public void DataSave()
    {
        for (int i = 0; i < islandArray.Length; i++)
        {
            PlayerPrefs.SetString(islandArray[i].name + "spawnCheck", islandArray[i].IsSpawn.ToString());
            PlayerPrefs.SetInt("islandPhase", islandPhase);
        }
    }

    public void DataLoad()
    {
        for (int i = 0; i < islandArray.Length; i++)
        {
            if (PlayerPrefs.GetString(islandArray[i].name + "spawnCheck") == "True")
            {
                islandArray[i].IsSpawn = true;
            }
            else
            {
                islandArray[i].IsSpawn = false;
            }

           islandPhase = PlayerPrefs.GetInt("islandPhase");
        }
    }

    public void SpawnCheck()
    {
        for (int i = 0; i < islandArray.Length; i++)
        {
            if (islandArray[i].IsSpawn == false)
            {
                islandArray[i].gameObject.SetActive(false);
            }
        }
    }

    public void IslandPhaseUp()
    {
        islandPhase += 1;
    }

    public void SpawnReady(Island island)
    {
        Vector3 islandPos = new Vector3(island.islandPosX, 0, 0);
        StartCoroutine(ParticleManager.Instance.IslandSpawnEffect(islandPos, spawnTime));
        CameraManager.Instance.CamPosSet(island);
        StartCoroutine(Spawn(island));
        UIManager.Instance.PanelOff();
        StartCoroutine(UIManager.Instance.SpawnUiMode(spawnTime));
    }

    public IEnumerator Spawn(Island island)
    {
        yield return new WaitForSeconds(spawnTime);
        island.gameObject.SetActive(true);
        island.IsSpawn = true;
    }
}
