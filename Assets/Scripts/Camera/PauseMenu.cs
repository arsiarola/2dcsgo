using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Core.GameController gameController;
    public Button okButton;

    public bool isGamePaused = false;
    public bool isAreYouSure = false;
    public bool quitMenu;

    public GameObject pauseMenu;
    public GameObject areYouSureMenu;
    public GameObject areYouSureQuit;
    public GameObject turnChange;

    public Button resume;
    public Button menu;
    public Button quit;

    public Button menuYes;
    public Button menuNo;
    public Button quitYes;
    public Button quitNo;

    private void Start() {
        resume.onClick.AddListener(delegate { pauseMenu.SetActive(false); isGamePaused = false; });
        menu.onClick.AddListener(delegate { BringAreYouSureMenu(); });
        quit.onClick.AddListener(delegate { BringAreYouSureQuit(); });

        menuYes.onClick.AddListener(delegate { SceneManager.LoadScene("MainMenu"); });
        menuNo.onClick.AddListener(delegate { BringPauseMenu(); });
        quitYes.onClick.AddListener(delegate { Application.Quit(); });
        quitNo.onClick.AddListener(delegate { BringPauseMenu(); });

        okButton.onClick.AddListener(delegate { OkClicked(); });

        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (isGamePaused && !isAreYouSure) {
                Resume();
            }
            else {
                Pause();
            }
        } 
    }

     void Pause() {
        pauseMenu.SetActive(true);
        areYouSureMenu.SetActive(false);
        areYouSureQuit.SetActive(false);
        isAreYouSure = false;
        isGamePaused = true;
    }

     void Resume() {
        pauseMenu.SetActive(false);
        isGamePaused = false;
    }

    private void BringAreYouSureMenu() {
        isAreYouSure = true;
        pauseMenu.SetActive(false);
        areYouSureMenu.SetActive(true);
    }

    private void BringAreYouSureQuit() {
        isAreYouSure = true;
        pauseMenu.SetActive(false);
        areYouSureQuit.SetActive(true);
    }

    private void BringPauseMenu() {
        areYouSureMenu.SetActive(false);
        areYouSureQuit.SetActive(false);

        isAreYouSure = false;
        pauseMenu.SetActive(true);
    }

    public void OkClicked() {
        turnChange.SetActive(false);
        gameController.SendMessage(Core.GameMessage.OkClicked);
    }

    public void BringTurnChange() {
        turnChange.SetActive(true);
    }
     
}
