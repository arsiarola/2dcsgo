using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private float spacing = 20f;
    private Vector3 pos;
    private float mouseWheel;
    private float dragSpeed = 3f;
    private Vector3 dragOrigin;
    private bool usingWasd;
    void Start() {
        Camera.main.orthographic = true;
    }

    void Update() {
        pos = transform.position;
        mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        MoveCamera();
    }

    /// <summary>
    ///     includes all possible camera movements with either keyboard or mouse
    /// </summary>
    private void MoveCamera() {
        ///if shift is held every camera movement faster
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            spacing = 60f;
        else
            spacing = 20f;
        ///check if using wasd keys
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.D)) {
            usingWasd = true;
        }
        else usingWasd = false;

        /// wasd movement for the camera
        if (usingWasd) {
            if (Input.GetKey(KeyCode.W) && pos.y + spacing * Time.unscaledDeltaTime < 64)
                pos.y += spacing * Time.unscaledDeltaTime;
            else if (Input.GetKey(KeyCode.S) && pos.y - spacing * Time.unscaledDeltaTime > -64)
                pos.y -= spacing * Time.unscaledDeltaTime;
            if (Input.GetKey(KeyCode.A) && pos.x - spacing * Time.unscaledDeltaTime > -64)
                pos.x -= spacing * Time.unscaledDeltaTime;
            else if (Input.GetKey(KeyCode.D) && pos.x + spacing * Time.unscaledDeltaTime < 64)
                pos.x += spacing * Time.unscaledDeltaTime;
            transform.position = pos;
        }

         /// move camera farther and closer with comma and period
        if (Input.GetKey(KeyCode.Comma)  && Camera.main.orthographicSize + spacing * Time.unscaledDeltaTime < 40)
            Camera.main.orthographicSize += spacing * Time.unscaledDeltaTime;
        else if (Input.GetKey(KeyCode.Period) && Camera.main.orthographicSize - spacing * Time.unscaledDeltaTime > 5)
            Camera.main.orthographicSize -= spacing * Time.unscaledDeltaTime;

        /// move camera farther and closer with mousewheel
        else if (mouseWheel > 0 && (Camera.main.orthographicSize - (spacing * Time.unscaledDeltaTime * 5))  > 5)
            Camera.main.orthographicSize -= spacing * Time.unscaledDeltaTime * 5;
        else if (mouseWheel < 0 && (Camera.main.orthographicSize + (spacing * Time.unscaledDeltaTime * 5)) < 40)
            Camera.main.orthographicSize += spacing * Time.unscaledDeltaTime * 5;

        /// detect if middle mouse button was pressed and mark the place where it was pressed
        if (Input.GetMouseButtonDown(2)) {
            dragOrigin = Input.mousePosition;
            return;
        }
        if (!Input.GetMouseButton(2)) {
            return;
        }

        /// allow camera movement with mouse if wasd keys arent used, while holding middle mouse button drag the camera towards it
        if (!usingWasd) {
            Vector3 position = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(position.x * dragSpeed, position.y * dragSpeed, 0);
            Vector3 moveX = new Vector3(move.x, Camera.main.ScreenToViewportPoint(position).y, 0);
            Vector3 moveY = new Vector3(Camera.main.ScreenToViewportPoint(position).x, move.y, 0);
            /// if in boundaries move freely
            if (transform.position.x + move.x < 64 && transform.position.x + move.x > -64 && transform.position.y + move.y < 64 && transform.position.y + move.y > -64) {
                transform.Translate(move, Space.World);
            }
            /// if either upper or lower boundarie is hit move only in x-axis
            else if (transform.position.x + move.x < 64 && transform.position.x + move.x > -64 && ((transform.position.y + move.y < 64) || (transform.position.y + move.y > -64))) {
                transform.Translate(moveX, Space.World);
            }
            /// if either left or right boundarie is hit move only in y-axis
            else if (((transform.position.x + move.x < 64 || transform.position.x + move.x > -64)) && (transform.position.y + move.y < 64 && transform.position.y + move.y > -64)) {
                transform.Translate(moveY, Space.World);
            }
        }

    }

    /// <summary>
    ///     center the camera angle, used after every turn change so enemy cannot get hints from previous camera angle
    /// </summary>
    public void CenterCamera() {
        transform.position = new Vector3(6, 6, -10);
        Camera.main.orthographicSize = 25;
    }
}



