using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using UnityEngine.SceneManagement;



public class PausePanel : MonoBehaviour
{
    public Button homeButton;
    public Button resumeButton;

    private void Awake()
    {
        OnClickSet();
    }

    private void OnClickSet()
    {
        homeButton.onClick.AddListener(AdventureQuit);
        resumeButton.onClick.AddListener(Resume);
    }

    public void AdventureQuit()
    {
        GameObject animal = FindObjectOfType<PlayerMovement>().gameObject;
        Destroy(animal);
        Time.timeScale = 1.0f;
        AdventureManager.Instance.GameOver();
    }



    public void Resume()
    {
        Time.timeScale = 1.0f;
        AdventureUIManager.Instance.goldBoxSliderController.presentGoldBoxSlider.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

}
