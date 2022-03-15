using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charontrigger : MonoBehaviour
{
    public LayerMask playerLayer;
    public Transform talkWave;
    public float talkRange = 3f;
    public Conversation convo;
    public Conversation convo2;
    Transform players;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int x = FindObjectOfType<GameManager>().GetComponent<CollectibleItemSet>().CollectedItems.Count;
        Collider2D[] talk = Physics2D.OverlapCircleAll(talkWave.position, talkRange, playerLayer);
        if (talk.Length != 0 && Input.GetKeyDown(KeyCode.E) && x == 32)
        {
            StartCoroutine(loadlevel());
            DialogueManager.StartConversation(convo);            
        }
        if((talk.Length != 0 && Input.GetKeyDown(KeyCode.E) && x != 32))
        {
            DialogueManager.StartConversation(convo2);
        }
                
    }
    private IEnumerator loadlevel()
    {
        yield return new WaitForSeconds(5f);
        Loader.Load(Loader.Scene.hades);
    }
}
