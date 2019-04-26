using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    public bool isGamePaused = false;
    public bool quitMenu;
    public GameObject pauseMenu;
    public GameObject areYouSureMenu;
    public GameObject areYouSureQuit;

    public Button resume;
    public Button menu;
    public Button quit;

    public Button menuYes;
    public Button menuNo;
    public Button quitYes;
    public Button quitNo;

    public Text areYouSureText;

    private void Start() {
        resume.onClick.AddListener(delegate { pauseMenu.SetActive(false); isGamePaused = false; });
        menu.onClick.AddListener(delegate { BringAreYouSureMenu(); });
        quit.onClick.AddListener(delegate { BringAreYouSureQuit(); });

        menuYes.onClick.AddListener(delegate { SceneManager.LoadScene("MainMenu"); });
        menuNo.onClick.AddListener(delegate { BringPauseMenu(); });
        quitYes.onClick.AddListener(delegate { Application.Quit(); });
        quitNo.onClick.AddListener(delegate { BringPauseMenu(); });

        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (isGamePaused) {
                Resume();
            }else {
                Pause();
            }
        } 
    }

     void Pause() {
        pauseMenu.SetActive(true);
        isGamePaused = true;
    }

     void Resume() {
        pauseMenu.SetActive(false);
        isGamePaused = false;
    }

    private void SwitchScene (string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    private void BringAreYouSureMenu() { 
        pauseMenu.SetActive(false);
        areYouSureMenu.SetActive(true);
    }

    private void BringAreYouSureQuit() {
        pauseMenu.SetActive(false);
        areYouSureQuit.SetActive(true);
    }

    private void BringPauseMenu() {
        areYouSureMenu.SetActive(false);
        areYouSureQuit.SetActive(false);

        pauseMenu.SetActive(true);
    }
}
