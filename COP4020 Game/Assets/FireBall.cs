using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float dieTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDownTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerCombat>().TakeDamage(10);
        }
        Destroy(gameObject);
    }

    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(dieTime);
        Destroy(gameObject);
    }
}
