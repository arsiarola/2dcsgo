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
    [SerializeField]Text aliveText;

    void Start()
    {
    }

    void Update()
    {
        DisplayGameStage();
        DisplayTime();
        DisplayAlive();
    }

    void DisplayAlive() {
        int tAiId;
        int ctAiId;
        int ctAlive;
        int tAlive;

        switch (gameController.Stage) {
            case Core.GameStage.Replay:
                int currentFrame = gameController.Replayer.CurrentFrameAsInt;
                //Debug.Log(currentFrame);
                tAiId = gameController.SideAIs[Core.Side.Terrorist];
                ctAiId = gameController.SideAIs[Core.Side.CounterTerrorist];
                ctAlive = gameController.Frames[currentFrame][ctAiId].GetProperty<RecordableState.ExtendedAI>().Children.Count;
                tAlive = gameController.Frames[currentFrame][tAiId].GetProperty<RecordableState.ExtendedAI>().Children.Count;
                //aliveText.text = "CT:" + ctAlive + " | T:" + tAlive;
                break;

            case Core.GameStage.Planning:
                //Dictionary<int, RecordableState.RecordableState> lastFrame = gameController.Planning.LastFrame;
                //tAiId = gameController.SideAIs[Core.Side.Terrorist];
                //ctAiId = gameController.SideAIs[Core.Side.CounterTerrorist];
                //ctAlive = lastFrame[ctAiId].GetProperty<RecordableState.ExtendedAI>().Children.Count;
                //tAlive = lastFrame[tAiId].GetProperty<RecordableState.ExtendedAI>().Children.Count;
                //Debug.Log(ctAlive);
                //Debug.Log(tAlive);
                break;

            default:
                break;
        }
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
