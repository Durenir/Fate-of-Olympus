using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGrapes : MonoBehaviour
{
    public float grapeRange = 3f;
    public int healAmount = 40;
    public int powerAmount = 20;
    public LayerMask playerLayer;
    bool eaten = false;
    Transform players;

    void Start()
    {
        players = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(!eaten)
        {   
            Collider2D[] eat = Physics2D.OverlapCircleAll(transform.position, grapeRange, playerLayer);
            if (eat.Length != 0  && Input.GetKeyDown(KeyCode.E))
            {
                foreach(Transform child in transform)
                    GameObject.Destroy(child.gameObject);
                //foreach(Collider2D player in eat)
                    eat[0].GetComponent<PlayerCombat>().Heal(healAmount);
                    eat[0].GetComponent<PlayerCombat>().Power(powerAmount);
                eaten = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grapeRange);
    }
}
