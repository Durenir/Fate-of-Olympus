using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void FadeSceneTransition()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        GameManager.instance.sceneLoading = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        GameManager.instance.sceneLoading = false;
    }
}
