using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePath : MonoBehaviour
{
    public List<Vector3> mousePositionList;
    public Vector3 mousePosition;

    public bool spacePressed;
    public bool createPath;
    public bool drawPath;
    public bool enableDrag = true;
    public bool mouseInWall = false;

    public float vectorDistance;
    public float amountOfPoints;
    public float pointAccuracy;

    private LineRenderer lineRenderer;
    private GameObject gObj;

    private Core.GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find(Misc.Constants.GameControllerName).GetComponent<Core.GameController>();   
        gObj = gameObject;

        pointAccuracy = 0.1f;
        amountOfPoints = 1000;
        mousePositionList = new List<Vector3>();

        //basic settings for linderenderer, important one is positionCount = 0, so we are not drawing anything at the beginning
        lineRenderer = gObj.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = false;

        createPath = false;
        drawPath = false;
        spacePressed = false;
    }

    //Adds a point to mousePostionList if conditions are right and after that draw the path
    private void Update()
    {
        mousePosition = Misc.Tools.SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        MouseOver();
        if (createPath)
        {
            CreatePath(); //includes  the point adding
        }
        if (drawPath)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++) //draw path from the beginning to the last point
            {
                lineRenderer.SetPosition(i, mousePositionList[i]);
            }
        }
    }

    private void CreatePath()
    {

        drawPath = true; // now that we have starterd creating the path, afterward its okay to draw the path

        // if mouse down 
        if (Input.GetMouseButton(0) && lineRenderer.positionCount < amountOfPoints) 
        {
            if (Physics2D.OverlapPoint(mousePosition, 1 << 8)) //check if mouse is on wall layermask set mouseInWall to true
                mouseInWall = true;
            
            if (mouseInWall == true && Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition) < pointAccuracy) //if mouse was inside wall check if it has come back
                mouseInWall = false;

            //if mouse is not in wall and the distance from last point to current mousePosition is  far enough to add vector point to mousePositionList
            if (mouseInWall == false) 
            {
                vectorDistance = (Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition));
                if (vectorDistance > pointAccuracy)
                {
                    lineRenderer.positionCount++; // the length of the path can now also increase since we have one more point to draw
                    mousePositionList.Add(mousePosition);
                }
            }
        
        }
        //if mouse is not held down stop creating and drawing the path and next time we try to create path destroy the old path
        else 
        {       
            createPath = false;
            drawPath = false;
            for (int i = 0; i < lineRenderer.positionCount; i++) //draw the line for the last time just incase
            {
                lineRenderer.SetPosition(i, mousePositionList[i]);
            }
        }
    } 
    


    private void MouseOver() //when mouse is moved close enough to the player and held down reset the mousePositionList and the drawn path
    {
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(mousePosition, gObj.transform.position) < 0.5) 
        {
            mousePositionList = new List<Vector3>();
            mousePositionList.Add(gObj.transform.position);
            mousePositionList.Add(mousePosition);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, gObj.transform.position);
            lineRenderer.SetPosition(1, mousePosition);
            createPath = true;
            drawPath = false;

        }
    }

}
