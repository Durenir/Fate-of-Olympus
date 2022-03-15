using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReaperController : MonoBehaviour
{
    public Animator animator;
    public float attackRange = 0.5f;
    public float walkRange = 3f;
    public LayerMask playerLayer;
    public int attackDamage = 10;
    public float attackRate = 150f;
    float nextAttackTime = 0f;
    public int maxhealth = 100;
    int currentHealth;
    Transform players;
    Rigidbody2D rb;
    public float speed = 0.1f;
    public bool isFlipped = false;
    public List<GameObject> rocks;
    public bool rockMove;
    public bool staggered;
    public bool waitStagger;
    public GameObject fireBall;
    public Transform shootPos;
    public float shootSpeed = 5f;
    bool canbeAttacked;
    public Text text;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxhealth;
        players = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(currentHealth > 0 && !waitStagger)
        {
            if(Time.time >= nextAttackTime)
            {
                if(!rockMove)
                {
                    GameObject walkToRock = getRandomRock();
                    LookAtRock(walkToRock);
                    animator.SetBool("WalkToPlayer", true);
                    Vector2 target = new Vector2(walkToRock.transform.position.x, rb.position.y);
                    StartCoroutine(moveShit(target));
                }
            }
        }
        if(staggered)
        {
            waitStagger = true;
            staggered = false;
            Debug.Log("Oh no! Where staggered!");
            animator.SetTrigger("Staggered");
            StartCoroutine(waitForStagger());
        }
    }

    private IEnumerator waitForStagger()
    {
        canbeAttacked = true;
        yield return new WaitForSeconds(15f);
        animator.SetTrigger("BackToIdle");
        Debug.Log("Were moving again");
        canbeAttacked = false;
        yield return new WaitForSeconds(4f);
        waitStagger = false;
    }

    private IEnumerator moveShit(Vector2 target)
    {
        rockMove = true;
        while(transform.position.x != target.x)
        {
            transform.position = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            yield return null;
        }
        animator.SetBool("WalkToPlayer", false);
        LookAtPlayer();
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(1f);
        Attack();
        nextAttackTime = Time.time + 15f;
        rockMove = false;
    }

    void Attack()
    {
        GameObject newFireBall = Instantiate(fireBall, shootPos.position, Quaternion.identity);
        Vector2 target = new Vector2(players.transform.position.x, players.transform.position.y);
        Debug.Log(newFireBall.transform.rotation);
        newFireBall.transform.LookAt(players);
        newFireBall.transform.rotation = new Quaternion(newFireBall.transform.rotation.x, newFireBall.transform.rotation.y, newFireBall.transform.rotation.z, 1f);
        Debug.Log(newFireBall.transform.rotation);
        StartCoroutine(moveFire(newFireBall, target));
    }

    private IEnumerator moveFire(GameObject newFireBall, Vector2 target)
    {
        while(newFireBall != null && newFireBall.transform.position.x != target.x && newFireBall.transform.position.y != target.y)
        {
            newFireBall.transform.position = Vector2.MoveTowards(newFireBall.GetComponent<Rigidbody2D>().position, target, speed * Time.fixedDeltaTime);
            yield return null;
        }
    }
    

    GameObject getRandomRock()
    {
        int index = Random.Range(0, rocks.Count);

        return rocks[index];
    }

    void LookAtRock(GameObject rock)
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x < rock.transform.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x > rock.transform.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
        
    void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x < players.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x > players.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if(canbeAttacked)
        {
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
        StartCoroutine(killBoss());
    }

    private IEnumerator killBoss()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetTrigger("IsDead");
        yield return new WaitForSeconds(0.7f);
        GetComponent<Animator>().enabled = false;
        StartCoroutine(FadeTextToFullAlpha(1f, text));
        yield return new WaitForSeconds(4f);
        SaveSystem.SeriouslyDeleteAllSaveFiles();
        SaveSystem.ClearTempFiles();
        Loader.Load(Loader.Scene.mainMenu);
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}
