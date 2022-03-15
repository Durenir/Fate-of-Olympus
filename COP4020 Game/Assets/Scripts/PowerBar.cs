using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{

    public Player player;
    public Image fillBar;

    // Update is called once per frame
    void Update()
    {
        //Fill bar is a float fron 0f t0 1f. Get the ratio and cast the int to a float
        fillBar.fillAmount = player.power/player.maxPower;
    }
}
