using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialoguer : MonoBehaviour
{
    Transform players;
    public LayerMask playerLayer;
    public Transform triggerWave;
    public float triggerRange = 3f;
    public Conversation convo;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectWithTag("Player").transform;
        DialogueManager.StartConversation(convo);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Collider2D[] trigger = Physics2D.OverlapCircleAll(triggerWave.position, triggerRange, playerLayer);
        //if player is inside the circle allow him to press e and trigger dialogue 
        if (trigger.Length != 0)
        {
            DialogueManager.StartConversation(convo);
        }
        */
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(triggerWave.position, triggerRange);
    }
}
