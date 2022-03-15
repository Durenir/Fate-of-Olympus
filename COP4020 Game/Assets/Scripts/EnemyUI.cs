using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class EnemyUI : MonoBehaviour
{
    // "points" is the list holding the values of the enemy patrol points
    // The order in which points are added at the end of Init() determines what order 
    // the enemy will go in to get to those points
    // Enemy starts at points[0], then points[1], etc. to end of the List, then goes backwards
    // back to points[0], then back up the List again
    public List<Transform> points;
    public Animator animator;
    [SerializeField]LayerMask playerLayer;
    public int TimeAtTurnPoint = 4;
    public int nextID;
    int idChangeValue = 1;
    public float speed = 2;

    public int maxHealth = 50;
    int currentHealth;

    public float walkRange = 5f;
    public float attackRange = 1f;
    public int attackDamage = 10;
    float nextAttackTime = 0f;

    private bool dead = false;
    bool foundPlayer = false;
    private bool BlinkCalled;
    Rigidbody2D rb;
    Transform players;

    void Start()
    {
        currentHealth = maxHealth;
        players = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }

    private void Reset()
    {
        Init();
    }

    void Init()
    {
        // Creates the hierarchy for the enemy in question
        // For example, if this script component is added to an object named "Wraith",
        // the object will be automatically put under a more general object "Wraith_Root",
        // which contains "Wraith" and "Waypoints", which are the set points an enemy will
        // patrol to before it turns around
        GetComponent<BoxCollider2D>().isTrigger = true;
        GameObject root = new GameObject(name + "_Root");
        root.transform.position = transform.position;
        transform.SetParent(root.transform);

        GameObject waypoints = new GameObject("Waypoints");
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;

        GameObject p1 = new GameObject("Point1");
        p1.transform.SetParent(waypoints.transform);
        p1.transform.position = root.transform.position;
        GameObject p2 = new GameObject("Point2");
        p2.transform.SetParent(waypoints.transform);
        p2.transform.position = root.transform.position;

        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    // Called at each frame
    private void Update() 
    {
        CheckForPlayer();
        if (!foundPlayer) MoveToNextPoint();
    }

    // Checks if the player is in range walkRange of the enemy
    // If so the enemy will stop patrolling and move toward the player
    // TODO IF YOU WANT: Make a boundary where if the enemy is led too far away from home, it will go back and patrol again
    void CheckForPlayer()
    {
        if (currentHealth > 0) {
            Collider2D[] walk = Physics2D.OverlapCircleAll(transform.position, walkRange, playerLayer);
            if (walk.Length != 0) foundPlayer = true;
            if (foundPlayer) {
                transform.position = Vector2.MoveTowards(transform.position, players.position, speed*Time.deltaTime);
                if (players.position.x > transform.position.x)
                    transform.localScale = new Vector3(1, 1, 1);
                else 
                    transform.localScale = new Vector3(-1, 1, 1);
            }
            // Attacks the player is within range and it hasn't attacked within time nextAttackTime
            Collider2D[] attacks = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
            if(attacks.Length != 0 && Time.time >= nextAttackTime)
            {
                Attack();
            }
        }
    }

    // Keeps the enemy moving toward the next patrol set point, then turns it around if there
    void MoveToNextPoint()
    {
        Transform goalPoint = points[nextID];

        if (goalPoint.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else 
            transform.localScale = new Vector3(-1, 1, 1);
        if (!BlinkCalled) transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed*Time.deltaTime);
        if(Vector2.Distance(transform.position, goalPoint.position) < 0.5f)
        {
            if (nextID == points.Count - 1) {
                idChangeValue = -1;
                if (!BlinkCalled) StartCoroutine(Blink());
            }    
            else if (nextID == 0) {
                idChangeValue = 1;
                if (!BlinkCalled) StartCoroutine(Blink());
            }    
            else
                nextID += idChangeValue;
        }
    }

    // Deals damage to player if in range (only damage to objects with tag "Player")
    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        if (hitEnemies[0].tag == "Player")
        {
            hitEnemies[0].GetComponent<PlayerCombat>().TakeDamage(attackDamage); //TODO: create function for variable damage. Change boss_health to be more generic for npc enemies, etc.
        }
        nextAttackTime = Time.time + 3f;
    }

    // Called by other objects to deal damage to this enemy
    // Calls Die() if health drops below 1
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0) animator.SetTrigger("Hurt");
        else Die();
    }

    // Played when enemy dies
    void Die()
    {
        if (!dead) animator.SetTrigger("IsDead");
                //TODO: Make it so that when enemy dies, player can walk through them like they are not there
        dead = true;
        // Makes enemy stop moving
        transform.position = transform.position;
        // Destroys enemy in 4 seconds, giving time for animation
        Destroy(gameObject, 2);
    }
    
    // The point of calling Blink() is to make the enemy stop patrolling for a sec and do an idle animation
    // (which is a blink animation in regards to the wraith) when it hits an ending turnpoint in its patrol
    // Without the bool BlinkCalled, Blink will continually be called, which messes up NextID
    private IEnumerator Blink() {
        BlinkCalled = true;
        transform.position = transform.position = Vector2.MoveTowards(transform.position, transform.position, speed*Time.deltaTime);
        animator.SetTrigger("TurnPoint");
        yield return new WaitForSeconds(TimeAtTurnPoint);
        nextID += idChangeValue;
        BlinkCalled = false;
    }
}