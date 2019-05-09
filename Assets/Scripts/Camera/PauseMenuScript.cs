using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] Core.GameController gameController;

    [SerializeField] bool isGamePaused = false;
    [SerializeField] bool isAreYouSure = false;
    [SerializeField] bool quitMenu;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button resume;
    [SerializeField] Button menu;
    [SerializeField] Button quit;

    [SerializeField] GameObject areYouSureMenu;
    [SerializeField] Button menuYes;
    [SerializeField] Button menuNo;

    [SerializeField] GameObject areYouSureQuit;
    [SerializeField] Button quitYes;
    [SerializeField] Button quitNo;

    [SerializeField] GameObject turnChange;
    [SerializeField] Button turnChangeOk;

    [SerializeField] GameObject endScreen;
    [SerializeField] Button endScreenOk;
    [SerializeField] Text endScreenText;


    /// <summary>
    ///     apply on click listeners to all different pausemenu buttons
    /// </summary>
    private void Start()
    {
        resume.onClick.AddListener(delegate { pauseMenu.SetActive(false); isGamePaused = false; });
        menu.onClick.AddListener(delegate { BringAreYouSureMenu(); });
        quit.onClick.AddListener(delegate { BringAreYouSureQuit(); });

        menuYes.onClick.AddListener(delegate { SceneManager.LoadScene("MainMenu"); });
        menuNo.onClick.AddListener(delegate { BringPauseMenu(); });

        quitYes.onClick.AddListener(delegate { Application.Quit(); });
        quitNo.onClick.AddListener(delegate { BringPauseMenu(); });

        turnChangeOk.onClick.AddListener(delegate { OkClicked(); });

        endScreenOk.onClick.AddListener(delegate { EndScreenClicked(); });


    }
    private void Update() {
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

    /// <summary>
    /// brings up the endscreen, and is called from gamecontroller. Side parameter tells who won and is applied to endscreen text
    /// </summary>
    /// <param name="side"></param>
    public void BringEndScreen(Core.Side side) {
        string s = "";
        if (side == Core.Side.Terrorist) {
            s = "Terrorists";
        }
        else if (side == Core.Side.CounterTerrorist) {
            s = "Counter-Terrorists";
        }
        endScreen.SetActive(true);
        endScreenText.text = s + " win, Press ok to continue";

    }

    /// <summary>
    /// when user has clicked ok button in endscreen message is sent to gamecontroller and endscreen is taken off off the screen
    /// </summary>
    public void EndScreenClicked() {
        endScreen.SetActive(false);
        gameController.SendMessage(Core.GameMessage.EndScreenClicked);
    }
}


