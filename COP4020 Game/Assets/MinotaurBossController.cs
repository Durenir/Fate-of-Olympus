using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

public class MinotaurBossController : MonoBehaviour
{
    // "points" is the list holding the values of the enemy patrol points
    // The order in which points are added at the end of Init() determines what order 
    // the enemy will go in to get to those points
    // Enemy starts at points[0], then points[1], etc. to end of the List, then goes backwards
    // back to points[0], then back up the List again
    public List<Transform> points;
    public Animator animator;
    [SerializeField]LayerMask playerLayer;
    /*public int TimeAtTurnPoint = 4;
    public int nextID;
    int idChangeValue = 1;*/
    public float speed = 2;

    public int maxHealth = 150;
    int currentHealth;

    public float walkRange = 5f;
    public float attackRange = 2f;
    public int attackDamage = 10;
    float nextAttackTime = 0f;
    public float timeBetweenAttacks = 3f;

    public float chargeRange = 4f;
    public int chargeDamage = 40;
    float nextChargeTime = 0f;
    public float timeBetweenCharges = 15f;
    public float chargeMass = 4;
    public float chargeDistance = 4f;

    bool foundPlayer = false;
    private bool charging = false;
    private bool ChargeCalled = false;
    private bool AttackCalled = false;
    private bool dead = false;
    private bool animateEvent = false;
    //private bool BlinkCalled;
    Rigidbody2D rb;
    Transform players;

    void Start()
    {
        currentHealth = maxHealth;
        players = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }

    /*private void Reset()
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
    }*/

    // Called at each frame
    private void Update() 
    {
        if (!ChargeCalled && !AttackCalled) CheckForPlayer();
        //if (!foundPlayer) MoveToNextPoint();
    }

    // Checks if the player is in range walkRange of the enemy
    // If so the enemy will stop patrolling and move toward the player
    // TODO IF YOU WANT: Make a boundary where if the enemy is led too far away from home, it will go back and patrol again
    void CheckForPlayer()
    {
        if (currentHealth > 0 && (!ChargeCalled)) {
            Collider2D[] walk = Physics2D.OverlapCircleAll(transform.position, walkRange, playerLayer);
            if (walk.Length != 0) 
            {
                if(!foundPlayer) animator.SetTrigger("Walk");
                foundPlayer = true;
                Debug.Log("In the code to make him walk");
            }
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
                StartCoroutine(Attack());
            }
            Collider2D[] charges = Physics2D.OverlapCircleAll(transform.position, chargeRange, playerLayer);
            if(charges.Length != 0 && Time.time >= nextChargeTime && (!ChargeCalled))
            {
                StartCoroutine(Charge());
            }
        }
    }

    /*// Keeps the enemy moving toward the next patrol set point, then turns it around if there
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
    }*/

    // Deals damage to player if in range (only damage to objects with tag "Player")
    private IEnumerator Attack()
    {
        AttackCalled = true;
        animator.SetTrigger("Attack");
        nextAttackTime = Time.time + timeBetweenAttacks;
        bool leftOfPlayer;
        if (transform.position.x < players.position.x) leftOfPlayer = true;
        else leftOfPlayer = false;
        yield return new WaitUntil(() => animateEvent == true);
        animateEvent = false;
        if (leftOfPlayer && transform.position.x<players.position.x)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
            if (hitEnemies.Length != 0 && hitEnemies[0].tag == "Player")
            {
                hitEnemies[0].GetComponent<PlayerCombat>().TakeDamage(attackDamage); //TODO: create function for variable damage. Change boss_health to be more generic for npc enemies, etc.
            }
        }
        if (!leftOfPlayer && transform.position.x>players.position.x)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
            if (hitEnemies.Length != 0 && hitEnemies[0].tag == "Player")
            {
                hitEnemies[0].GetComponent<PlayerCombat>().TakeDamage(attackDamage); //TODO: create function for variable damage. Change boss_health to be more generic for npc enemies, etc.
            }
        }
        Debug.Log("Minotaur just attacked");
        AttackCalled = false;
    }

    /*void Charge()
    {
        animator.SetTrigger("Charge");
        if(!ChargeCalled) StartCoroutine(Charging());
        nextChargeTime = Time.time + timeBetweenCharges;
    }*/

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(charging)
        {
            if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(chargeDamage);
                charging = false;
            }
        }
    }

    // Called by other objects to deal damage to this enemy
    // Calls Die() if health drops below 1
    public void TakeDamage(int damage)
    {
        if (!ChargeCalled)
        {
            currentHealth -= damage;
            if (currentHealth > 0) animator.SetTrigger("Hurt");
            else Die();
        }
    }

    // Played when enemy dies
    void Die()
    {
        if(!dead) animator.SetTrigger("IsDead");
                //TODO: Make it so that when enemy dies, player can walk through them like they are not there
        dead = true;
        // Makes enemy stop moving
        transform.position = transform.position;
        // Destroys enemy in 4 seconds, giving time for animation
        Destroy(gameObject, 4);
        FindObjectOfType<Player>().hasBeatenAres = true;
        FindObjectOfType<Player>().berzerkAbility = true;
        GameObject.Find("Rage").GetComponent<Image>().enabled = true;
    }

    private IEnumerator Charge()
    {
        ChargeCalled = true;
        animator.SetTrigger("Charge");
        /*float xvalue;
        if (players.position.x > transform.position.x) xvalue = transform.position.x + chargeDistance;
        else xvalue = transform.position.x - chargeDistance;
        Vector2 target;
        target = new Vector2(xvalue, players.position.y);
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        transform.position = currentPosition;*/
        yield return new WaitUntil(() => animateEvent == true);
        animateEvent = false;

        float timeCounter = 0.8f;
        charging = true;
        Rigidbody2D rigid;
        rigid = GetComponent<Rigidbody2D>();
        float mass;
        mass = rigid.mass;
        rigid.mass = chargeMass;
        Vector2 target;
        if (transform.position.x < players.position.x)
            target = new Vector2(transform.position.x + 100, transform.position.y);
        else 
            target = new Vector2(transform.position.x - 100, transform.position.y);
        if (players.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else 
            transform.localScale = new Vector3(-1, 1, 1);
        while(timeCounter > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, 15*Time.deltaTime);
            /*if (players.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else 
                transform.localScale = new Vector3(-1, 1, 1);*/
            timeCounter -= Time.deltaTime;
            yield return null;
        }
        rigid.mass = mass;
        rigid.velocity = new Vector2(0, 0);
        charging = false;
        ChargeCalled = false;
        nextChargeTime = Time.time + timeBetweenCharges;

        /*transform.position = Vector2.MoveTowards(transform.position, players.position, 7*Time.deltaTime);
        Rigidbody2D rigid;
        rigid = GetComponent<Rigidbody2D>();
        float mass;
        mass = rigid.mass;
        rigid.mass = chargeMass;
        yield return new WaitForSeconds(3);
        rigid.mass = mass;*/
    }

    void AnimateEvent()
    {
        animateEvent = true;
    }
    
    /*// The point of calling Blink() is to make the enemy stop patrolling for a sec and do an idle animation
    // (which is a blink animation in regards to the wraith) when it hits an ending turnpoint in its patrol
    // Without the bool BlinkCalled, Blink will continually be called, which messes up NextID
    private IEnumerator Blink() {
        BlinkCalled = true;
        transform.position = transform.position = Vector2.MoveTowards(transform.position, transform.position, speed*Time.deltaTime);
        animator.SetTrigger("TurnPoint");
        yield return new WaitForSeconds(TimeAtTurnPoint);
        nextID += idChangeValue;
        BlinkCalled = false;
    }*/
}