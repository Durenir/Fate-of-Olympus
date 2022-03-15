using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public Slider musicVolume;
    public Slider sfxVolume;
    public bool dialogueBoxes = true;
    public bool useMainSave = false;
    public string activeScene;
    public string activeSceneMusic;
    public bool createTempSave = true;
    public string sceneToLoad;
    public bool sceneLoading;
    public bool clearTemp = true;
    public bool gameIsPaused = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            clearTemp = true;
        }
        else
        {
            clearTemp = false;
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    //Set up manager for mainmenu
    void Start()
    {
        if(SaveSystem.SaveExists("AudioSettings"))
        {
            instance.useMainSave = true;
            float[] settings = SaveSystem.Load<float[]>("AudioSettings");
            musicVolume.value = settings[0];
            Debug.Log("Save data is: " + musicVolume.value);
            sfxVolume.value = settings[1];
            instance.useMainSave = false;
        } else {
            musicVolume.value = 1f;
            sfxVolume.value = 1f;
        }
        UpdateMusic();
        UpdateSFX();
    }

    public void UpdateMusic()
    {
        Debug.Log("Giving " + musicVolume.value + " to audio manager");
        FindObjectOfType<AudioManager>().musicVolume = musicVolume.value;
    }
    public void UpdateSFX()
    {
        FindObjectOfType<AudioManager>().sfxVolume = sfxVolume.value;
    }
    void OnDestroy()
    {
        if(clearTemp)
        {
            GameManager.instance.createTempSave = false;
            Debug.Log("Destroying temp files");
            SaveSystem.ClearTempFiles();
        }
    }
}
