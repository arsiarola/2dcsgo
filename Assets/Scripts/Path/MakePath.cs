using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePath : MonoBehaviour
{
    public List<Vector3> mousePositionList;
    public Vector3 mousePosition;

    public bool spacePressed;
    public bool makePath;
    public bool drawPath;
    public bool destroyPath;
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

        lineRenderer = gObj.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = false;

        makePath = false;
        drawPath = false;
        destroyPath = true;
        spacePressed = false;
    }

    private void Update()
    {
        mousePosition = Misc.Tools.SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        MouseOver();
        if (makePath)
        {
            if (destroyPath)
            {
                lineRenderer.positionCount = 0;
                lineRenderer.positionCount = 2;
            }
            CreatePath();
        }
        if (drawPath)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, mousePositionList[i]);
            }
        }
    }

    private void CreatePath()
    {

        drawPath = true;
        destroyPath = false;

        if (Input.GetMouseButton(0) && lineRenderer.positionCount < amountOfPoints)
        {
            if (Physics2D.OverlapPoint(mousePosition, 1 << 8))
                mouseInWall = true;
            if (mouseInWall == true && Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition) < pointAccuracy)
            {
                mouseInWall = false;
            }

            if (mouseInWall == false)
            {
                vectorDistance = (Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition));
                if (vectorDistance > pointAccuracy)
                {
                    lineRenderer.positionCount++;
                    mousePositionList.Add(mousePosition);
                }
            }
        
        }
        else
        {
            makePath = false;
            drawPath = false;
            destroyPath = true;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, mousePositionList[i]);
            }
        }
    } 
    


    private void MouseOver()
    {
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(mousePosition, gObj.transform.position) < 0.5) 
        {
            Debug.Log("in if");
            mousePositionList = new List<Vector3>();
            mousePositionList.Add(gObj.transform.position);
            mousePositionList.Add(mousePosition);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, gObj.transform.position);
            lineRenderer.SetPosition(1, mousePosition);
            makePath = true;
            drawPath = false;

        }
    }

}
