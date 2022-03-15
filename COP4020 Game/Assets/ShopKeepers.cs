using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeepers : MonoBehaviour
{
    public Animator animator;
    Transform players;
    Rigidbody2D rb;
    ShopKeepers shop;
    public LayerMask playerLayer;
    public Transform shopWave;
    public float waveRange = 3f;
    public Conversation convo; 

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
        if (wave.Length != 0  && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.StartConversation(convo);
            GameManager.instance.useMainSave = true;
            Debug.Log("Saving progress...");
            GameEvents.OnSaveInitiated();
            GameManager.instance.useMainSave = false;

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
