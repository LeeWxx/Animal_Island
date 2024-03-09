using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AdventureInven : MonoBehaviour
{
    public AdventureNode[] animalNodes = new AdventureNode[40];

    private void Awake()
    {

        for (int i = 0; i < animalNodes.Length; i++)
        {
            animalNodes[i].NodeSet();
        }

        AnimalNodesSet();
        OnClickSet();
    }

    public void OnClickSet()
    {
        for (int i = 0; i < animalNodes.Length; i++)
        {
            int _i = i;
            animalNodes[i].adventureBut.onClick.AddListener(delegate { Adventure(animalNodes[_i].nodeAnimalState); });
        }
    }

    private void OnEnable()
    {
        AnimalNodesCheck();
    }

    void AnimalNodesSet()
    {

        GameObject[] animalArray = AnimalManager.Instance.animalArray;

        for (int i = 0; i < animalNodes.Length; i++)
        {
            animalNodes[i].nodeAnimalState = animalArray[i].GetComponent<AnimalState>();
            animalNodes[i].nodeAnimalNameText.text = animalNodes[i].nodeAnimalState.name;
            animalNodes[i].nodeAnimalIsland = animalArray[i].GetComponent<AnimalMovement>().myIsland;

            LocalizeSrcript local = animalNodes[i].nodeAnimalNameText.GetComponent<LocalizeSrcript>();
            local.textKey = animalNodes[i].nodeAnimalState.name;
            local.LocalizeChanged();
        }
    }

    void AnimalNodesCheck()
    {
        for (int i = 0; i < animalNodes.Length; i++)
        {
            animalNodes[i].NodeCheck();
        }
    }


    public void Adventure(AnimalState animal)
    {
        bool energyCheck = GameManager.Instance.EnergyMinus(1);
        if (energyCheck == true)
        {
            GameManager.Instance.GameDataSave();
            UIManager.Instance.PanelOff();
            AdventureSetting.Instance.AdventureAnimalSet(animal.name);
            SceneManager.LoadScene("Adventure");
        }
        else
        {
            WarningManager.Instance.ShortageEnergyWarning();
        }
    }
}
