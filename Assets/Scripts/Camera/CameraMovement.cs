using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float spacing = 20f;
    public Vector3 pos;
    void Start()
    {
        Camera.main.orthographic = true;
        pos = transform.position;
 }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(); 
        
    }
    private void MoveCamera() {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            spacing = 60f;
        else
            spacing = 20f;
        if (Input.GetKey(KeyCode.W))
            pos.y += spacing * Time.unscaledDeltaTime;
        else if (Input.GetKey(KeyCode.A))
            pos.x -= spacing * Time.unscaledDeltaTime;
        else if (Input.GetKey(KeyCode.S))
            pos.y -= spacing * Time.unscaledDeltaTime;
        else if (Input.GetKey(KeyCode.D))
            pos.x += spacing * Time.unscaledDeltaTime;

         if (Input.GetKey(KeyCode.Comma) &&  Camera.main.orthographicSize + spacing*Time.unscaledDeltaTime < 20) 
            Camera.main.orthographicSize += spacing * Time.unscaledDeltaTime;
         else if (Input.GetKey(KeyCode.Period) && Camera.main.orthographicSize - spacing*Time.unscaledDeltaTime > 5 )
            Camera.main.orthographicSize -= spacing * Time.unscaledDeltaTime;

    transform.position = pos;
    }
}


