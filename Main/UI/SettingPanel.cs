using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;

    public GameObject musicSliderIcon;
    public GameObject muteMusicSliderIcon;

    public GameObject soundSliderIcon;
    public GameObject muteSoundSliderIcon;


    private void Awake()
    {
        SliderSet();
        musicSlider.value = SoundManager.Instance.masterVolumeBGM;
        soundSlider.value = SoundManager.Instance.masterVolumeSFX;

        MusicVolumeChange();
        SoundVolumeChange();
    }

    private void SliderSet()
    {
        musicSlider.onValueChanged.AddListener(delegate { MusicVolumeChange(); });
        soundSlider.onValueChanged.AddListener(delegate { SoundVolumeChange(); });
    }

    public void MusicVolumeChange()
    {
        if(musicSlider.value == 0f)
        {
            muteMusicSliderIcon.gameObject.SetActive(true);
            musicSliderIcon.gameObject.SetActive(false);
        }
        
        if(muteMusicSliderIcon.activeSelf == true)
        {
            if(musicSlider.value != 0f)
            {
                muteMusicSliderIcon.gameObject.SetActive(false);
                musicSliderIcon.gameObject.SetActive(true);
            }
        }
        SoundManager.Instance.BGMVolumChange(musicSlider.value);
    }

    public void SoundVolumeChange()
    {
        if (soundSlider.value == 0f)
        {
            muteSoundSliderIcon.gameObject.SetActive(true);
            soundSliderIcon.gameObject.SetActive(false);
        }

        if (muteSoundSliderIcon.activeSelf == true)
        {
            if (soundSlider.value != 0f)
            {
                muteSoundSliderIcon.gameObject.SetActive(false);
                soundSliderIcon.gameObject.SetActive(true);
            }
        }
        SoundManager.Instance.masterVolumeSFX = soundSlider.value;
    }
    public void MusicMute()
    {
        musicSlider.value = 0f;
        MusicVolumeChange();
    }

    public void SoundMute()
    {
        soundSlider.value = 0f;
        SoundVolumeChange();
    }
}
