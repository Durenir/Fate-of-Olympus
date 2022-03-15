using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : MonoBehaviour
{
    public Animator animator;
    Transform players;
    Rigidbody2D rb;
    ShopKeepers shop;
    public LayerMask playerLayer;
    public Transform shopWave;
    public float waveRange = 3f;
    public Tester test; 

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        shop = animator.GetComponent<ShopKeepers>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] wave = Physics2D.OverlapCircleAll(shopWave.position, waveRange, playerLayer);
        //if player is inside the circle allow him to press e and trigger dialogue 
        if (wave != null && Input.GetKeyDown(KeyCode.E))
        {
            test.StartConvo();
        }
        foreach (Collider2D player in wave)
        {
            StartCoroutine(keeperwave());
        }
    }

    private IEnumerator keeperwave()
    {
        animator.SetBool("wave", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("wave", false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(shopWave.position, waveRange);
    }

}
