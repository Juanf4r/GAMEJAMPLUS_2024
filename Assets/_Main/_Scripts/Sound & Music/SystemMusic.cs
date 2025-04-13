using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMusic : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    public float sliderValue;
    public Image imagenMute;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("RevisarSiEstoyMute", 0.5f);
        AudioListener.volume = musicSlider.value;
        RevisarSiEstoyMute();
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("RevisarSiEstoyMute", sliderValue);
        AudioListener.volume = musicSlider.value;
        RevisarSiEstoyMute();
    }

    public void RevisarSiEstoyMute()
    {
        if (sliderValue == 0)
        {
            imagenMute.enabled = true;
        }
        else 
        {
            imagenMute.enabled = false;
        }
    }
}
