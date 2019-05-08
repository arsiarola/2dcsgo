using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class ChangeScene : MonoBehaviour {
    public Button play;
    public Button exit;
    void Start() {
    }

    public void Play() {
        SceneManager.LoadScene("GameControl"); 
    }

    public void Exit() {
        exit.onClick.AddListener(delegate { Application.Quit(); });
    }
}
