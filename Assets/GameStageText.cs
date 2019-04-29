using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStageText : MonoBehaviour
{
    [SerializeField] private Core.GameController gameController;
    Text gameStageText;
    void Start()
    {
        gameStageText = (Text)GameObject.Find("DisplayGameStage").GetComponent<Text>();
    }

    void Update()
    {
        DisplayGameStage();
    }

    void DisplayGameStage() {
        switch(gameController.Stage) {
            case Core.GameStage.CTPlanning:
                gameStageText.text = "CT Orders";
                break;

            case Core.GameStage.TPlanning:
                gameStageText.text = "T Orders";
                break;

            case Core.GameStage.CTReplay:
                gameStageText.text = "CT Replay";
                break;

            case Core.GameStage.TReplay:
                gameStageText.text = "T Replay";
                break;

            case Core.GameStage.Record:
                gameStageText.text = "Simualtion in progress...";
                break;

            case Core.GameStage.Planning:
                gameStageText.text = "Test Planning";
                break;

            case Core.GameStage.Replay:
                gameStageText.text = "Test Replay";
                break;
            default:
                gameStageText.text = "";
                break;
        }
    }
}
