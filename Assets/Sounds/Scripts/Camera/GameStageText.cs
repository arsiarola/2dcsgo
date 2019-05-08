using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStageText : MonoBehaviour
{
    /*
    public class MyComponent<T>
    {
        public T obj;

        public MyComponent(T t)
        {
            obj = t;
        }
    }

    List<MyComponent<string>> listOfComponents = new List<MyComponent<string>>();
    */
    [SerializeField] private Core.GameController gameController;
    Text gameStageText;
    void Start()
    {
        gameStageText = (Text)GameObject.Find("DisplayGameStage").GetComponent<Text>();
        /*string s = "asfdsf";
        listOfComponents.Add(new MyComponent<string>(s));*/
    }

    void Update()
    {
        DisplayGameStage();
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
