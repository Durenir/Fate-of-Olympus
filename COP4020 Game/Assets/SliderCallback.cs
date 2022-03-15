using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderCallback : MonoBehaviour
{
    public Slider musicVolume;
    public Slider sfxVolume;
    void Awake()
    {
        if(!GameManager.instance.createTempSave)
        {
            musicVolume.value = AudioManager.instance.musicVolume;
            sfxVolume.value = AudioManager.instance.sfxVolume;
        }
    }
}
