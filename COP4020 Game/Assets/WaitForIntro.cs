using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WaitForIntro : MonoBehaviour
{
    public Animator transition;

    public float waitTime = 10f;
    public float transitionTime = 1f;

    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(Loader.Scene.mainMenu.ToString());
    }
}
