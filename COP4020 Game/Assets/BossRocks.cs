using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRocks : MonoBehaviour
{
    public Player player;
    bool falling;
    Vector3 location;
    Vector3 scale;
    Quaternion rotation;
    bool roundOne = true;
    bool drop = false;
    int health = 100;
    bool destroy;
    bool reaperHit;
    ReaperController reaperController;

    void Start()
    {
        location = transform.position;
        scale = transform.localScale;
        rotation = transform.rotation;
        reaperController = FindObjectOfType<ReaperController>();
        while(reaperController == null)
        {
            Debug.Log("Fuck!!!!!!!!!");
        }
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
        if(drop && !falling)
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
        Debug.Log("Collided with: " + other.gameObject.name);
        if(!reaperHit)
        {
            if (other.gameObject.tag == "ReaperBoss")
            {
                Debug.Log("Hit the reaper!");
                other.gameObject.GetComponent<ReaperController>().staggered = true;
                other.gameObject.GetComponent<ReaperController>().rocks.Remove(gameObject);
                destroy = true;
                reaperHit = true;
            } else if(other.gameObject.tag != "Untagged" && other.gameObject.tag != "Player" && other.gameObject.tag != "Enemy") {
                reaperController.rocks.Remove(gameObject);
                destroy = true;
            }
        }
    }

    private IEnumerator waitAndDestroy()
    {
        destroy = false;
        yield return new WaitForSeconds(1.5f);
        GameObject newRock = GameObject.Instantiate(gameObject);
        FindObjectOfType<ReaperController>().rocks.Add(newRock);
        newRock.transform.position = location;
        newRock.transform.localScale = scale;
        newRock.transform.rotation = rotation;
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Here");
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(WaitForFall());
            drop = true;
        }
    }

    private IEnumerator WaitForFall()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
