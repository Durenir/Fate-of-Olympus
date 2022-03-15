using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public class LoaderMB : MonoBehaviour { }
    private static LoaderMB loaderMB;

    private static void init()
    {
        if(loaderMB == null)
        {
            GameObject gameObject = new GameObject("Loader");
            loaderMB = gameObject.AddComponent<LoaderMB>();
        }
    }
    public enum Scene{
        splash, mainMenu, demo, hub, hermes, ares, hades
    }
    public static void Load(Scene scene)
    {
        if(SceneManager.GetActiveScene().name != "Splash")
        {
            init();
            GameObject go = GameObject.Find("CrossFade");
            SceneTransition st = go.GetComponent<SceneTransition>();
            AudioManager.instance.Stop(GameManager.instance.activeSceneMusic);
            st.FadeSceneTransition();
            loaderMB.StartCoroutine(loadWithTransition(scene));
        } else {
            AudioManager.instance.Stop(GameManager.instance.activeSceneMusic);
            SceneManager.LoadScene(scene.ToString());
        }
    }

    public static void Load(string scene)
    {
        if(SceneManager.GetActiveScene().name != "Splash")
        {
            init();
            GameObject go = GameObject.Find("CrossFade");
            SceneTransition st = go.GetComponent<SceneTransition>();
            AudioManager.instance.Stop(GameManager.instance.activeSceneMusic);
            st.FadeSceneTransition();
            scene = scene.ToLower();
            Scene nextScene = (Scene)System.Enum.Parse(typeof(Scene), scene);
            loaderMB.StartCoroutine(loadWithTransition(nextScene));
        } else {
            AudioManager.instance.Stop(GameManager.instance.activeSceneMusic);
            SceneManager.LoadScene(scene);
        }
    }

    private static IEnumerator loadWithTransition(Scene scene)
    {
        while(GameManager.instance.sceneLoading)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene(scene.ToString());
    }
}
