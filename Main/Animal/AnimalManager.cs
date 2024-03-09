using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimalManager : MonoBehaviour
{
    private static AnimalManager instance;

    List<Dictionary<string, object>> data_Animal;

    public List<Dictionary<string, object>> Data_Animal
    {
        get { return data_Animal; }
    }

    public static AnimalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AnimalManager>();
            }

            return instance;
        }
    }

    public GameObject[] animalArray = new GameObject[40];
    private AnimalState[] animalStates = new AnimalState[40];

    public Dictionary<GameObject, Vector3> animalFollowDic = new Dictionary<GameObject, Vector3>();

    public AnimalInven animalInven;
    private int spawnTime = 3;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        data_Animal = CSVReader.Read("AnimalData");

        ArraySet();
        AnimalValueSet();
        DataLoad();
        SpawnCheck();
    }

    public void ArraySet()
    {
        for (int i = 0; i < data_Animal.Count; i++)
        {
            animalArray[i] = GameObject.Find(data_Animal[i]["Name"].ToString());
            animalStates[i] = animalArray[i].GetComponent<AnimalState>();
        }
    }

    public void AnimalValueSet()
    {
        AnimalMovement[] AnimalMovementArray = new AnimalMovement[40];

        for (int i = 0; i < data_Animal.Count; i++)
        {
            animalStates[i] = animalArray[i].GetComponent<AnimalState>();
            AnimalMovementArray[i] = animalArray[i].GetComponent<AnimalMovement>();
        }

        for (int i = 0; i < data_Animal.Count; i++)
        {
            animalStates[i].Price = (int)data_Animal[i]["Price"];
            animalStates[i].EarlyMaxHp = (int)data_Animal[i]["EarlyMaxHp"];
            animalStates[i].Phase = (int)data_Animal[i]["Phase"];
            animalStates[i].animalNum = i;

            AnimalMovementArray[i].boxColliderSize = new Vector3(float.Parse(data_Animal[i]["ColliderSizeX"].ToString()),
                float.Parse(data_Animal[i]["ColliderSizeY"].ToString()), float.Parse(data_Animal[i]["ColliderSizeZ"].ToString()));

            AnimalMovementArray[i].boxColliderCenter = new Vector3(float.Parse(data_Animal[i]["ColliderCenterX"].ToString()),
                float.Parse(data_Animal[i]["ColliderCenterY"].ToString()), float.Parse(data_Animal[i]["ColliderCenterZ"].ToString()));

            AnimalMovementArray[i].navSpeed = float.Parse(data_Animal[i]["NavSpeed"].ToString());
            AnimalMovementArray[i].navRadius = float.Parse(data_Animal[i]["NavRadius"].ToString());
            AnimalMovementArray[i].navHeight = float.Parse(data_Animal[i]["NavHeight"].ToString());

            animalFollowDic.Add(animalArray[i], new Vector3(float.Parse(data_Animal[i]["ZoomX"].ToString()),
                float.Parse(data_Animal[i]["ZoomY"].ToString()), float.Parse(data_Animal[i]["ZoomZ"].ToString())));

            AnimalMovementArray[i].myIsland = GameObject.Find("Island" + (int)data_Animal[i]["Island"]).GetComponent<Island>();
        }
    }

    public void DataLoad()
    {
        for (int i = 0; i < data_Animal.Count; i++)
        {
            animalStates[i] = animalArray[i].GetComponent<AnimalState>();
        }
        for (int i = 0; i < animalStates.Length; i++)
        {
            if (PlayerPrefs.GetString(animalArray[i].name + "spawnCheck") == "True")
            {
                animalStates[i].IsSpawn = true;
            }
            else
            {
                animalStates[i].IsSpawn = false;
            }

            animalStates[i].Level = PlayerPrefs.GetInt(animalArray[i].name + "Level");
            if (animalStates[i].Level == 0)
            {
                animalStates[i].Level = 1;
            }

            animalStates[i].Intimacy = PlayerPrefs.GetInt(animalArray[i].name + "Intimacy");
        }
    }

    public void DataSave()
    {
        for (int i = 0; i < data_Animal.Count; i++)
        {
            animalStates[i] = animalArray[i].GetComponent<AnimalState>();
        }
        for (int i = 0; i < animalStates.Length; i++)
        {
            PlayerPrefs.SetString(animalArray[i].name + "spawnCheck", animalStates[i].IsSpawn.ToString());
            PlayerPrefs.SetInt(animalArray[i].name + "Level", animalStates[i].Level);
            PlayerPrefs.SetInt(animalArray[i].name + "Intimacy", animalStates[i].Intimacy);
        }
    }

    public void SpawnCheck()
    {
        for(int i=0; i<animalArray.Length; i++)
        {
            if (animalStates[i].IsSpawn == false)
            {
                animalArray[i].SetActive(false);
            }
        }
    }

    public void SpawnReady(AnimalState animal)
    {
        Island animalIsland = animal.GetComponent<AnimalMovement>().myIsland;
        Vector3 spawnPos = animalIsland.GetDestination(animalIsland.transform.position);
        animal.transform.position = spawnPos;
        spawnPos.y += 1;
        StartCoroutine(ParticleManager.Instance.AnimalSpawnEffect(spawnPos, spawnTime));
        UIManager.Instance.PanelOff();
        CameraManager.Instance.CamPosSet(animalIsland);
        StartCoroutine(Spawn(animal));
        StartCoroutine(UIManager.Instance.SpawnUiMode(spawnTime));
    }

    public IEnumerator Spawn(AnimalState animal)
    {
        yield return new WaitForSeconds(spawnTime);
        animal.gameObject.SetActive(true);
        animal.IsSpawn = true;
        TouchManager.Instance.touchedAnimal = animal;
        CameraManager.Instance.BrainMode(animal.gameObject);
        PlayerPhaseCheck();
    }


    public void PlayerPhaseCheck()
    {
        for (int i = 0; i < animalInven.animalNodes.Length; i++)
        {
            if (animalInven.animalNodes[i].nodeAnimalState.Phase <= GameManager.Instance.PlayerPhase + 1)
            {
                if (animalInven.animalNodes[i].nodeAnimalState.IsSpawn == false)
                {
                    return;
                }
            }
        }
        GameManager.Instance.PlayerPhaseUp();
    }
}
