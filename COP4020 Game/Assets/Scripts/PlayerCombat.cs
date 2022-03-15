using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;
    public Player player;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public bool isDead;
    public bool blockPressed = false;

    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        if(player.shieldAbility)
        {
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                blockPressed = true;
            }else if(Input.GetKeyUp(KeyCode.Mouse1))
            {
                blockPressed = false;
            }
            animator.SetBool("Block", blockPressed);
        }
    }

    void Attack()
    {
        int damage = attackDamage;
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        if(player.GetComponent<KnightController>().berzerkActive)
        {
                damage = (int)(damage * 1.5f);
        }
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Damage delt: " + damage);
            if (hitEnemies[0].tag == "Enemy")
                enemy.GetComponent<EnemyUI>().TakeDamage(damage);
            if (hitEnemies[0].tag == "Boss")
                enemy.GetComponent<BossController>().TakeDamage(damage);
            if (hitEnemies[0].tag == "Minotaur")
                enemy.GetComponent<MinotaurBossController>().TakeDamage(damage);
            if (hitEnemies[0].tag == "HermesBoss")
                enemy.GetComponent<HermesBossController>().TakeDamage(damage);
            if (hitEnemies[0].tag == "ReaperBoss")
                enemy.GetComponent<ReaperController>().TakeDamage(damage);
            if (hitEnemies[0].tag == "BossRock")
                enemy.GetComponent<BossRocks>().TakeDamage(damage);
        }
        AudioManager.instance.Play("Attack");
    }

    public void TakeDamage(int damage)
    {
        if (!isDead && !blockPressed)
        {
            if (player.GetComponent<KnightController>().berzerkActive)
            {
                damage = (int)(damage * 1.25f);
            }
            Debug.Log("Current health is " + player.health);
            player.health -= damage;
            Debug.Log("Damage was: " + damage);
            Debug.Log("New health is: " + player.health);
            animator.SetTrigger("Hurt");
            FindObjectOfType<AudioManager>().Play("Hurt");
            if (player.health <= 0)
            {
                isDead = true;
                Debug.Log("Were dead!!!!!");
                Die();
            }
        }
        else if (blockPressed)
        {
            player.power -= 5f;
            AudioManager.instance.Play("Block");
        }
        
    }

    void Die()
    {
        StartCoroutine(killPlayer());
    }

    private IEnumerator killPlayer()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(1.1f);
        GetComponent<SpriteRenderer>().enabled = false;
        player.health = player.maxhealth;
        player.power = player.maxPower;
        Loader.Load(Loader.Scene.hub);
    }

    public void Heal(int amount)
    {
        player.health += amount;
        if (player.health > player.maxhealth) player.health = player.maxhealth;
    }

    public void Power(int amount)
    {
        player.power += amount;
        if (player.power > player.maxPower) player.power = player.maxPower;
    }

    void OnDrawGizmos()
    {
        if(attackPoint == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
