﻿using System.Collections;
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

    /// <summary>
    ///     Display how many CT's and T's are alive in given moment
    /// </summary>
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
                aliveText.text = "CT:" + ctAlive + " | T:" + tAlive;
                break;

            case Core.GameStage.Planning:
                Dictionary<int, RecordableState.RecordableState> lastFrame = gameController.Planning.LastFrame;
                tAiId = gameController.SideAIs[Core.Side.Terrorist];
                ctAiId = gameController.SideAIs[Core.Side.CounterTerrorist];
                if (lastFrame != null && lastFrame.ContainsKey(ctAiId) && lastFrame.ContainsKey(tAiId)) {
                    ctAlive = lastFrame[ctAiId].GetProperty<RecordableState.ExtendedAI>().Children.Count;
                    tAlive = lastFrame[tAiId].GetProperty<RecordableState.ExtendedAI>().Children.Count;
                } else {
                    ctAlive = 5;
                    tAlive = 5;
                }
                aliveText.text = "CT:" + ctAlive + " | T:" + tAlive;
                break;

            default:
                break;
        }
    }

    /// <summary>
    ///     display time in all modes and update it correctly in replay
    /// </summary>
    void DisplayTime() {
        RecordableState.BombStuff bombState;
        switch (gameController.Stage) {
            case Core.GameStage.Planning:
                bombState = gameController.Planning.LastFrame[gameController.BombId].GetProperty<RecordableState.BombStuff>();
                if (bombState.Planted) {
                    timeText.text = bombState.BombTimer.ToString("0.00");
                    timeText.color = Color.red;
                } else {
                    timeText.text = gameController.Planning.CurrentTime.ToString("0.00");
                    timeText.color = Color.green;
                }
                
                break;
            case Core.GameStage.Replay:
                bombState = gameController.Frames[gameController.Replayer.CurrentFrameAsInt][gameController.BombId].GetProperty<RecordableState.BombStuff>();
                if (bombState.Planted) {
                    timeText.text = bombState.BombTimer.ToString("0.00");
                    timeText.color = Color.red;
                }
                else {
                    timeText.text = gameController.Replayer.CurrentTime.ToString("0.00");
                    timeText.color = Color.green;
                }
                break;
            case Core.GameStage.Record:
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///     determine whose turn it is and use it in displaying it
    /// </summary>
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

        ///<summary>
        /// check what stage is on and update the stagetext accordingly
        /// </summary>
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
