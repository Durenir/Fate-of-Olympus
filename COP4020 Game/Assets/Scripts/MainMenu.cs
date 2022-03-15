using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    Player player;
    public GameObject warningUI;
    public GameObject mainMenuUI;
    public Button continueButton;
    public TMP_Text textComponent;

    void Start()
    {
        warningUI = GameObject.Find("WarningScreen");
        warningUI.SetActive(false);
        if(!SaveSystem.SaveExists("Player") && GameManager.instance.createTempSave)
        {
            continueButton.interactable = false;
            Debug.Log(textComponent.text);
            textComponent.color = new Color32(84,84,84,200);
            textComponent.enableVertexGradient = false;
        }
        AudioManager.instance.Play("Music");
        GameManager.instance.activeSceneMusic = "Music";
        GameManager.instance.activeScene = SceneManager.GetActiveScene().name;
    }

    public void NewGame()
    {
        if(SaveSystem.SaveExists("Player") || SaveSystem.TempExists("Player"))
        {
            mainMenuUI = GameObject.Find("MainMenu");
            warningUI.SetActive(true);
            mainMenuUI.SetActive(false);
        } else {
            FindObjectOfType<GameManager>().GetComponent<CollectibleItemSet>().CollectedItems.Clear();
            Loader.Load(Loader.Scene.demo);
        }
    }

    public void EraseGame()
    {
        SaveSystem.SeriouslyDeleteAllSaveFiles();
        FindObjectOfType<GameManager>().GetComponent<CollectibleItemSet>().CollectedItems.Clear();
        NewGame();
    }

    public void Continue()
    {
        if(SaveSystem.SaveExists("Player") && GameManager.instance.createTempSave)
        {
            //Set useGameSave bool to true to load GameSave file and not temp file.
            GameManager.instance.useMainSave = true;
            if(SaveSystem.SaveExists("CollectedItems"))
            {
                FindObjectOfType<GameManager>().GetComponent<CollectibleItemSet>().CollectedItems = SaveSystem.Load<HashSet<string>>("CollectedItems");
                Debug.Log("We have loaded " + FindObjectOfType<GameManager>().GetComponent<CollectibleItemSet>().CollectedItems.Count + " coins");
            }
            GameManager.instance.useMainSave = true;
            Loader.Load(Loader.Scene.hub);
        } else {
            // SceneManager.LoadScene(GameManager.instance.sceneToLoad);//Need to use loader by converting string to enum.
            Loader.Load(GameManager.instance.sceneToLoad);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
