using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDialogue : MonoBehaviour
{
    Transform players;
    public LayerMask playerLayer;
    public Transform startWave;
    public float startRange = 3f;
    public Conversation convos;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        Collider2D[] start = Physics2D.OverlapCircleAll(startWave.position, startRange, playerLayer);
        //if player is inside the circle trigger dialogue 
        if (start.Length != 0)
        {
            DialogueManager.StartConversation(convos);
        }
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startWave.position, startRange);
    }
}