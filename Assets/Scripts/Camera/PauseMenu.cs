using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraText : MonoBehaviour {
    private Text text;
    [SerializeField] private Core.GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        WriteGameState();

    }

    private void WriteGameState() {

        switch (gameController.Stage) {

            case Core.GameStage.Planning:
                text.text = "Test Planning";
                break;

            case Core.GameStage.Replay:
                text.text = "Test Replay";
                break;
            case Core.GameStage.Record:
                text.text = "Test Record";
                break;
        }

    }
}
