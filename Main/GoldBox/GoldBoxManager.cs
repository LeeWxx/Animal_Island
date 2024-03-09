using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


public class GoldBoxManager : MonoBehaviour
{
    private static GoldBoxManager instance;

    public static GoldBoxManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GoldBoxManager>();
            }

            return instance;
        }
    }

    GoldBox fantasyBox_1;
    GoldBox fantasyBox_2;
    GoldBox fantasyBox_3;

    GoldBox modernBox_1;
    GoldBox modernBox_2;
    GoldBox modernBox_3;

    GoldBox[] goldBoxArray;

    public GoldBox[] GoldBoxArray
    {
        get { return goldBoxArray; }
        set { goldBoxArray = value; }
    }

    List<Dictionary<string, object>> data_Box;

    public List<Dictionary<string, object>> Data_Box
    {
        get { return data_Box; }
    }

    public Inventory inventory;
    public Inventory InventroyProperty
    {
        get 
        { 
            if(inventory == null)
            {
                inventory = GameObject.Find("PanelCanvas").transform.Find("GoldBoxPanel").transform.Find("Node").transform.Find("GoldBoxInventory").GetComponent<Inventory>();
            }

            return inventory;
        }
    }

    public Storage storage;

    public Storage StorageProperty
    {
        get
        {
            if (storage == null)
            {
                storage = GameObject.Find("PanelCanvas").transform.Find("GoldBoxPanel").transform.Find("Node").transform.Find("Storage").GetComponent<Storage>();
            }

            return storage;
        }
    }

    private List<GoldBox> getReadyGoldBoxList = new List<GoldBox>();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        GoldBoxSet();
        StorageProperty.StroageSet();

        InventroyProperty.DataLoad();
        StorageProperty.DataLoad();
    }

    private void GoldBoxSet()
    {
        data_Box = CSVReader.Read("BoxData");

        goldBoxArray = new GoldBox[6];


        goldBoxArray[0] = fantasyBox_1;
        goldBoxArray[1] = fantasyBox_2;
        goldBoxArray[2] = fantasyBox_3;
        goldBoxArray[3] = modernBox_1;
        goldBoxArray[4] = modernBox_2;
        goldBoxArray[5] = modernBox_3;

        goldBoxArray[0].name = "Fantasy Chest 1";
        goldBoxArray[1].name = "Fantasy Chest 2";
        goldBoxArray[2].name = "Fantasy Chest 3";
        goldBoxArray[3].name = "Modern Chest 1";
        goldBoxArray[4].name = "Modern Chest 2";
        goldBoxArray[5].name = "Modern Chest 3";

        for (int i = 0; i < goldBoxArray.Length; i++)
        {
            goldBoxArray[i].Phase = (int)data_Box[i]["Phase"];

            goldBoxArray[i].inSlotPos = new Vector3(float.Parse(data_Box[i]["InSlotPosX"].ToString()),
                float.Parse(data_Box[i]["InSlotPosY"].ToString()), 0f);

            goldBoxArray[i].inSlotSacle = new Vector3(float.Parse(data_Box[i]["InSlotScaleX"].ToString()),
                float.Parse(data_Box[i]["InSlotScaleY"].ToString()), 0f);

            goldBoxArray[i].GetGoldMinValue = (int)data_Box[i]["GetGoldMinValue"];
            goldBoxArray[i].GetGoldMaxValue = (int)data_Box[i]["GetGoldMaxValue"];

            goldBoxArray[i].goldBoxImage = Resources.Load(goldBoxArray[i].name, typeof(Sprite)) as Sprite;

        }
    }

    public void GoldBoxAdd(int index = -1)
    {
        if(index == -1)
        {
            index = Random.Range(0, goldBoxArray.Length);
        }
        if (SceneManager.GetActiveScene().name == "Main")
        {
            InventroyProperty.ItemAdd(goldBoxArray[index]);
        }
        else if (SceneManager.GetActiveScene().name == "Adventure")
        {
            getReadyGoldBoxList.Add(goldBoxArray[index]);
        }
    }

    private void OnApplicationQuit()
    {
        InventroyProperty.DataSave();
        StorageProperty.DataSave();
    }

    public void ReadyGoldBoxGet()
    {
        for(int i =0; i< getReadyGoldBoxList.Count; i++)
        {
            InventroyProperty.ItemAdd(getReadyGoldBoxList[i]);
        }

        getReadyGoldBoxList.RemoveAll(x => true);
    }
}
