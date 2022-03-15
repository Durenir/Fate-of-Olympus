/* PauseMenu.cs
 * 
 * Credit to Brackeys
 * "PAUSE MENU in Unity"
 * https://www.youtube.com/watch?v=JivuXdrIHK0
 * 2021-09-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        if (GameIsPaused)
	        {
		        Resume();
	        }
	        else
	        {
		        Pause();
	        }
	    }
    }

    public void Resume()
    {
    GameManager.instance.gameIsPaused = false;
	pauseMenuUI.SetActive(false);
	Time.timeScale = 1f;
	GameIsPaused = false;
    }

    void Pause ()
    {
    GameManager.instance.gameIsPaused = true;
	pauseMenuUI.SetActive(true);
	Time.timeScale = 0f;
	GameIsPaused = true;
    }

    public void RestartLevel()
    {
        Resume();
        GameManager.instance.createTempSave = false;
        // SceneManager.LoadScene(GameManager.instance.activeScene);//Need to use loader. Convert string to enum
        Loader.Load(GameManager.instance.activeScene);
    }

    public void OptionsMenu()
    {

    }

    public void LoadMenu()
    {
        GameManager.instance.createTempSave = false;
        Resume();
        GameManager.instance.sceneToLoad = GameManager.instance.activeScene;
        Loader.Load(Loader.Scene.mainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
