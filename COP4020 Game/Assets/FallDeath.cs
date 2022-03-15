using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour
{

    public Player player;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerCombat>().TakeDamage(200);
        }
    }
}
