using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class WalkingVillager : MonoBehaviour
{
    public List<Transform> points;
    public Animator animator;
    [SerializeField] LayerMask playerLayer;
    public int TimeAtTurnPoint = 1;
    public int nextID;
    int idChangeValue = 1;
    public float speed = 2;
    public float waveRange = 3f;
    public Transform shopWave;
    public Conversation convo;

    Rigidbody2D rb;
    Transform players;
    private bool BlinkCalled;

        void Start()
        {

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
             MoveToNextPoint();
            Collider2D[] talk = Physics2D.OverlapCircleAll(shopWave.position, waveRange, playerLayer);
            //if player is inside the circle allow him to press e and trigger dialogue 
            
            if (talk.Length != 0 && Input.GetKeyDown(KeyCode.Q))
            {
                DialogueManager.StartConversation(convo);
            }
            
    }


    // Keeps the enemy moving toward the next patrol set point, then turns it around if there
    void MoveToNextPoint()
    {
        Transform goalPoint = points[nextID];

        if (goalPoint.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        if (!BlinkCalled) transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.5f)
        {
            if (nextID == points.Count - 1)
            {
                idChangeValue = -1;
                if (!BlinkCalled) StartCoroutine(Blink());
            }
            else if (nextID == 0)
            {
                idChangeValue = 1;
                if (!BlinkCalled) StartCoroutine(Blink());
            }
            else
                nextID += idChangeValue;
        }

    }


        // The point of calling Blink() is to make the enemy stop patrolling for a sec and do an idle animation
        // (which is a blink animation in regards to the wraith) when it hits an ending turnpoint in its patrol
        // Without the bool BlinkCalled, Blink will continually be called, which messes up NextID
        private IEnumerator Blink()
        {
            BlinkCalled = true;
            transform.position = transform.position = Vector2.MoveTowards(transform.position, transform.position, speed * Time.deltaTime);
            animator.SetTrigger("TurnPoint");
            yield return new WaitForSeconds(TimeAtTurnPoint);
            nextID += idChangeValue;
            BlinkCalled = false;
        }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(shopWave.position, waveRange);
    }
}
    
