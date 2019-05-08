using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiScript : MonoBehaviour
{
    [SerializeField] private Core.GameController gameController;
    [SerializeField]Text gameStageText;
    [SerializeField]Text scoreText;
    [SerializeField]Text timeText;
    void Start()
    {
    }

    void Update()
    {
        DisplayGameStage();
        DisplayTime();
    }

    void DisplayScore() {

    }

    void DisplayTime() {
        switch (gameController.Stage) {
            case Core.GameStage.Planning:
                timeText.text = gameController.Planning.CurrentTime.ToString("0.00");
                break;
            case Core.GameStage.Replay:
                timeText.text = gameController.Replayer.CurrentTime.ToString("0.00");
                break;
            case Core.GameStage.Record:
                timeText.text = gameController.Recorder.CurrentTime.ToString("0.00");
                break;
            default:
                break;
        }
    }

    void DisplayGameStage() {
        string side = "";

        switch (gameController.Side) {
            case Core.Side.CounterTerrorist:
                side = "CT";
                break;
            case Core.Side.Terrorist:
                side = "T";
                break;
            default:
                break;
        }

        switch(gameController.Stage) {
            case Core.GameStage.Record:
                gameStageText.text = "Simulation in progress...";
                break;

            case Core.GameStage.Planning:
                gameStageText.text = side + " Planning";
                break;

            case Core.GameStage.Replay:
                gameStageText.text = side + " Replay";
                break;
            default:
                gameStageText.text = "";
                break;
        }
    }
}
