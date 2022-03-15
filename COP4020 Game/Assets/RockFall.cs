using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    public Player player;
    bool falling;
    Vector3 location;
    Vector3 scale;
    Quaternion rotation;
    bool roundOne = true;
    bool destroy;

    void Start()
    {
        location = transform.position;
        scale = transform.localScale;
        rotation = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody2D>() != null && roundOne)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        if(GetComponent<BoxCollider2D>().enabled == false && roundOne)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        roundOne = false;
        if((Mathf.Abs(player.transform.position.x - transform.position.x) < 5) && !falling)
        {
            //Fall
            falling = true;
            if(GetComponent<Rigidbody2D>() == null)
            {
                this.gameObject.AddComponent<Rigidbody2D>();
            } else {
                GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
        if(destroy)
        {
            StartCoroutine(waitAndDestroy());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!destroy)
        {
            if (other.gameObject.tag == "Player")
            {
                player.GetComponent<PlayerCombat>().TakeDamage(20);
            }
            GetComponent<BoxCollider2D>().enabled = false;
            destroy = true;
        }
    }

    private IEnumerator waitAndDestroy()
    {
        destroy = false;
        yield return new WaitForSeconds(2f);
        GameObject newRock = GameObject.Instantiate(gameObject);
        newRock.transform.position = location;
        newRock.transform.localScale = scale;
        newRock.transform.rotation = rotation;
        Destroy(gameObject);
    }
}
