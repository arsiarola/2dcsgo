using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class ChangeScene : MonoBehaviour {
    private Button play;
    private Button exit;
    void Start() {
        play = GameObject.Find("Play").GetComponent<Button>();
        exit = GameObject.Find("Exit").GetComponent<Button>();
        play.onClick.AddListener(delegate {SceneManager.LoadScene("GameControl"); });
        exit.onClick.AddListener(delegate { Application.Quit(); });
    }

    // Update is called once per frame
    void Update() {

    }
}
