using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int maxhealth = 100;
    public float power = 100;
    public float maxPower = 100;
    public int money;
    public bool shieldAbility;
    public bool doubleJumpAbility;
    public bool berzerkAbility;
    public bool godMode;
    public bool hasBeatenAthena;
    public bool hasBeatenHermes;
    public bool hasBeatenAres;
    public bool hasBeatenHades;
    public bool hasBeatenAreas2;

    public void Start()
    {
        GameEvents.SaveInitiated += SavePlayer;
    }

    void SavePlayer()
    {
        PlayerConfig config = new PlayerConfig(this);
        SaveSystem.Save(config, "Player");
        if(SaveSystem.SaveExists("Player"))
            Debug.Log("Save created");
    }

    public void LoadPlayer()
    {
        if(SaveSystem.SaveExists("Player"))
        {
            PlayerConfig config = SaveSystem.Load<PlayerConfig>("Player");
            //Add something that stores the current scene
            health = config.health;
            maxhealth = config.maxhealth;
            power = config.power;
            maxPower = config.maxPower;
            money = config.money;
            shieldAbility = config.shieldAbility;
            doubleJumpAbility = config.doubleJumpAbility;
            berzerkAbility = config.berzerkAbility;
            godMode = config.godMode;
            hasBeatenAthena = config.hasBeatenAthena;
            hasBeatenHermes = config.hasBeatenHermes;
            hasBeatenAres = config.hasBeatenAres;
            hasBeatenHades = config.hasBeatenHades;
            hasBeatenAreas2 = config.hasBeatenAreas2;
        }
    }
}
