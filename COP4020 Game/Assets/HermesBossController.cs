using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HermesBossController : MonoBehaviour
{
    public Animator animator;
    public Transform bossAttack;
    public Transform bossWalk;
    public float attackRange = 0.5f;
    public float walkRange = 3f;
    public LayerMask playerLayer;
    public int attackDamage = 10;
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    public int maxhealth = 100;
    int currentHealth;
    Transform players;
    Rigidbody2D rb;
    public float speed = 2.5f;
    Boss boss;
    public bool isFlipped = false;
    bool dead = false;
    public Conversation convo;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxhealth;
        players = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {      
        if(currentHealth > 0)
        {
            Collider2D[] walk = Physics2D.OverlapCircleAll(bossWalk.position, walkRange, playerLayer);
            foreach (Collider2D player in walk)
            {
                LookAtPlayer();
                animator.SetBool("Walk", true);
                Vector2 target = new Vector2(players.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
            Collider2D[] attacks = Physics2D.OverlapCircleAll(bossAttack.position, attackRange, playerLayer);
            if(attacks.Length != 0 && Time.time >= nextAttackTime)
            {
                Attack();
            }
        }
    }
        
    void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > players.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < players.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }


    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(bossAttack.position, attackRange, playerLayer);
        if (hitEnemies.Length != 0 && hitEnemies[0].tag == "Player")
        {
            hitEnemies[0].GetComponent<PlayerCombat>().TakeDamage(attackDamage); //TODO: create function for variable damage. Change boss_health to be more generic for npc enemies, etc.
        }
        nextAttackTime = Time.time + attackRate;
    }

    public void TakeDamage(int damage)
    {
        if(!dead) {
            currentHealth -= damage;
            animator.SetTrigger("Hurt");
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
    void Die()
    {
        dead = true;
        StartCoroutine(killBoss());
        DialogueManager.StartConversation(convo);
        //GetComponent<SpriteRenderer>().enabled = false;
        //Disable character completely, restart depending on difficulty.
    }

    private IEnumerator killBoss()
    {
        FindObjectOfType<Player>().hasBeatenHermes = true;
        FindObjectOfType<Player>().doubleJumpAbility = true;
        GameObject.Find("Jump").GetComponent<Image>().enabled = true;
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(3f);
        GetComponent<Animator>().enabled = false;
        Loader.Load(Loader.Scene.hub); 
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(bossAttack.position, attackRange);
        Gizmos.DrawWireSphere(bossWalk.position, walkRange);
    }
}
