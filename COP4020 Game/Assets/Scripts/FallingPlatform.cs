using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("On the test platform");
            StartCoroutine(waitAndFall());
        }
    }

    private IEnumerator waitAndFall()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Dropping");
        gameObject.AddComponent<Rigidbody2D>();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}