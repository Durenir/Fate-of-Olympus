using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Health : MonoBehaviour
{
    public int Health = 500;
    private int temp;
    public bool damagetaken = false;
    public GameObject deathEffect;
    public bool isInvulnerable = false;
    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        temp = Health;
        Health -= damage;

        if (Health < temp)
        {
            damagetaken = true;
            BossHurt();
        }         
  
        if(Health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void BossHurt()
    {
        damagetaken = false;
    }
}
