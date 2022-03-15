using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Player player;
    public string backgroundMusicName;
    public Slider optionsMusicVolume;
    public Slider optionsSfxVolume;

    void Start()
    {
        if(SaveSystem.SaveExists("Player") || SaveSystem.TempExists("Player"))
        {
            PlayerConfig config = SaveSystem.Load<PlayerConfig>("Player");
            Debug.Log("Old health is " +  config.health + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            player.health = config.health;
            Debug.Log("Health is: " + player.health);
            player.maxhealth = config.maxhealth;
            player.power = config.power;
            player.maxPower = config.maxPower;
            player.money = config.money;
            player.shieldAbility = config.shieldAbility;
            player.doubleJumpAbility = config.doubleJumpAbility;
            player.berzerkAbility = config.berzerkAbility;
            player.godMode = config.godMode;
            player.hasBeatenAthena = config.hasBeatenAthena;
            player.hasBeatenHermes = config.hasBeatenHermes;
            player.hasBeatenAres = config.hasBeatenAres;
            player.hasBeatenHades = config.hasBeatenHades;
            player.hasBeatenAreas2 = config.hasBeatenAreas2;
        }
        GameManager.instance.activeScene = SceneManager.GetActiveScene().name;
        // AudioManager.instance.Stop(GameManager.instance.activeSceneMusic);
        AudioManager.instance.Play(backgroundMusicName);
        GameManager.instance.activeSceneMusic = backgroundMusicName;
        GameManager.instance.createTempSave = true;
        Debug.Log(AudioManager.instance.musicVolume);
        optionsMusicVolume.value = AudioManager.instance.musicVolume;
        optionsSfxVolume.value = AudioManager.instance.sfxVolume;
        Debug.Log("SFX in options menu is: " + optionsSfxVolume.value);
        if(!player.shieldAbility)
            GameObject.Find("Shield").GetComponent<Image>().enabled = false;
        if(!player.doubleJumpAbility)
            GameObject.Find("Jump").GetComponent<Image>().enabled = false;
        if(!player.berzerkAbility)
            GameObject.Find("Rage").GetComponent<Image>().enabled = false;
    }

    void OnDestroy()
    {
        if(GameManager.instance.createTempSave)
        {
            Debug.Log("Saving");
            Save();
        }
    }

    public void Save()
    {
        GameEvents.OnSaveInitiated();
    }

    public void DeleteAllProgress()
    {
        SaveSystem.SeriouslyDeleteAllSaveFiles();
    }

        // Update is called once per frame
    public void UpdateMusic()
    {
        FindObjectOfType<AudioManager>().musicVolume = optionsMusicVolume.value;
    }
    public void UpdateSFX()
    {
        FindObjectOfType<AudioManager>().sfxVolume = optionsSfxVolume.value;
        Debug.Log("SFX volume in level manager is: " + FindObjectOfType<AudioManager>().sfxVolume);
    }

}
