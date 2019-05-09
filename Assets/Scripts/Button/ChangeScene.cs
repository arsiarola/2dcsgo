using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class ChangeScene : MonoBehaviour {
    [SerializeField] private Button play;
    [SerializeField] private Button exit;
    void Start() {
    }

    public void Play() {
        SceneManager.LoadScene(1); 
    }

    public void Exit() {
        exit.onClick.AddListener(delegate { Application.Quit(); });
    }
}
