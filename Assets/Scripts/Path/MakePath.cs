using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePath : MonoBehaviour
{
    private List<Vector3> mousePositionList;
    private Vector3 mousePosition;

    private bool spacePressed;
    private bool makePath;
    private bool drawPath;
    private bool destroyPath;
    private bool inAction;

    private float overallDistance;
    private float vectorDistance;
    private float amountOfPoints;
    private float pointAccuracy;

    private LineRenderer lineRenderer;
    private GameObject gObj;

    private Core.GameController gameController;
    private void Start()
    {
        gameController = GameObject.Find(Misc.Constants.GameControllerName).GetComponent<Core.GameController>();   
        gObj = gameObject;
        StartCoroutine("InputCheck");
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
        inAction = false;
    }

    void Update()
    {
        if (drawPath)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, mousePositionList[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        mousePosition = Misc.Tools.SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (makePath)
        {
            if (destroyPath)
            {
                lineRenderer.positionCount = 0;
                lineRenderer.positionCount = 2;
            }
            CreatePath();
        }
    }

    private void CreatePath()
    {
        if (!inAction)
        {
            drawPath = true;
            destroyPath = false;
            overallDistance = 0;
            
            for(int i = 1; i < mousePositionList.Count; i++)
            {
                vectorDistance = Vector3.Distance(mousePositionList[i], mousePositionList[i - 1]);
                overallDistance += vectorDistance;
            }
            overallDistance += Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition);
            if (Input.GetMouseButton(0) && lineRenderer.positionCount < amountOfPoints && overallDistance < 100)
            {
                vectorDistance = (Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition));

                if (vectorDistance > pointAccuracy)
                {
                    lineRenderer.positionCount++;
                    mousePositionList.Add(mousePosition);
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
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
    public void SendMousePositionList() 
    {
        //int id = gObj.GetComponent<Id>().id; 
    }

}
