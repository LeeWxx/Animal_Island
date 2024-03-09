using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningManager : MonoBehaviour
{
    private static WarningManager instance;

    public static WarningManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WarningManager>();
            }

            return instance;
        }
    }

    public Text warningText;
    Color textOriginalColor;

    List<Dictionary<string, object>> data_Warning;

    Coroutine warningCoroutine;

    private void Awake()
    {
        data_Warning = CSVReader.Read("WarningData");
        textOriginalColor = warningText.color;

        //animalStates[i].Price = (int)data_Animal[i]["Price"];
    }

    public void ShortageGoldWarning()
    {
        warningText.text = (string)data_Warning[0]["GoldShortage"];
        WarningCheck();
    }

    public void ShortageEnergyWarning()
    {
        warningText.text = (string)data_Warning[0]["EnergyShortage"];
        WarningCheck();
    }

    public void NeedIslandWarning(int islandNum)
    {
        warningText.text = (string)data_Warning[0]["Island"+ islandNum +"Need"];
        WarningCheck();
    }

    private void WarningCheck()
    {
        if (warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
        }

        warningCoroutine = StartCoroutine(Warning());
    }

    public IEnumerator Warning()
    {
        warningText.gameObject.SetActive(true);

        warningText.color = textOriginalColor;

        Color textFadeColor = textOriginalColor;

        float c = textOriginalColor.a;

        yield return new WaitForSeconds(2f);

        while (warningText.color.a > 0.05f)
        {
            yield return null;
            c = Mathf.Lerp(c, 0, Time.deltaTime);

            textFadeColor.a = c;

            warningText.color = textFadeColor;
        }

        warningText.gameObject.SetActive(false);
    }




}
