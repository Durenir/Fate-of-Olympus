using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerConfig
{
    public int health;
    public int maxhealth;
    public float power;
    public float maxPower;
    public int money;
    public float[] position;
    public bool shieldAbility;
    public bool doubleJumpAbility;
    public bool berzerkAbility;
    public bool godMode;
    public bool hasBeatenAthena;
    public bool hasBeatenHermes;
    public bool hasBeatenAres;
    public bool hasBeatenHades;
    public bool hasBeatenAreas2;

    public PlayerConfig(Player player)
    {
        //Add something that stores the current scene.
        health = player.health;
        maxhealth = player.maxhealth;
        power = player.power;
        maxPower = player.maxPower;
        money = player.money;
        shieldAbility = player.shieldAbility;
        doubleJumpAbility = player.doubleJumpAbility;
        berzerkAbility = player.berzerkAbility;
        godMode = player.godMode;
        hasBeatenAthena = player.hasBeatenAthena;
        hasBeatenHermes = player.hasBeatenHermes;
        hasBeatenAres = player.hasBeatenAres;
        hasBeatenHades = player.hasBeatenHades;
        hasBeatenAreas2 = player.hasBeatenAreas2;
    }
}
