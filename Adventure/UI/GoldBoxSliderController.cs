using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GoldBoxSliderController : MonoBehaviour
{

    public GoldBoxSlider fantasyBoxSlider;
    public GoldBoxSlider modernBoxSlider;
    public GoldBoxSlider presentGoldBoxSlider;



    int presentBoxIndex;

    public int PresentBoxIndex
    {
        get { return presentBoxIndex; }
    }

    int maxBoxIndex;
    int fantasyToModernIndex = 3;



    int presentValue;
    int maxValue = 1000;



    int[] plusValue = new int[6] { 50, 10, 5, 3, 2, 1 };
    bool allClear;



    private void Awake()
    {
        allClear = false;

        presentBoxIndex = 0;
        maxBoxIndex = GoldBoxManager.Instance.GoldBoxArray.Length - 1;

        presentGoldBoxSlider = fantasyBoxSlider;
        presentGoldBoxSlider.goldBoxImage.sprite = GoldBoxManager.Instance.GoldBoxArray[presentBoxIndex].goldBoxImage;
        presentGoldBoxSlider.goldBoxImage.transform.localScale = GoldBoxManager.Instance.GoldBoxArray[presentBoxIndex].inSlotSacle * 0.4f;

        presentValue = 0;
        presentGoldBoxSlider.thisSlider.value = presentValue / maxValue;

    }

    public void ValueUp()
    {

        if (allClear == false)
        {
            presentValue += plusValue[presentBoxIndex];
            presentGoldBoxSlider.thisSlider.value = (float)presentValue / (float)maxValue;



            if (presentValue >= maxValue)
            {
                GoldBoxSliderLevelUp();
            }

        }

    }



    private void GoldBoxSliderLevelUp()
    {
        presentBoxIndex += 1;
        presentValue = 0;

        if (presentBoxIndex <= maxBoxIndex)
        {

            if(presentBoxIndex < fantasyToModernIndex)
            {
                AdventureParticleManager.Instance.FantasyEffectPlay();
            }
            else
            {
                AdventureParticleManager.Instance.ModernEffectPlay();

                if (presentBoxIndex == fantasyToModernIndex)
                {
                    fantasyBoxSlider.gameObject.SetActive(false);
                    modernBoxSlider.gameObject.SetActive(true);

                    presentGoldBoxSlider = modernBoxSlider;

                }
            }



            presentGoldBoxSlider.thisSlider.value = (float)presentValue / (float)maxValue;
            presentGoldBoxSlider.goldBoxImage.sprite = GoldBoxManager.Instance.GoldBoxArray[presentBoxIndex].goldBoxImage;
            presentGoldBoxSlider.goldBoxImage.transform.localScale = GoldBoxManager.Instance.GoldBoxArray[presentBoxIndex].inSlotSacle * 0.4f; ;

        }

        else if (presentBoxIndex > maxBoxIndex)
        {
            allClear = true;
            AdventureParticleManager.Instance.ModernEffectPlay();
            presentGoldBoxSlider.gameObject.SetActive(false);

        }
    }
}