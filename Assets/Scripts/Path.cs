using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Path : MonoBehaviour
{

    public List<Vector3> mousePositionList;
    public Vector3 mousePosition;

    public bool pressed;
    public bool makePath;
    public bool drawPath;
    public bool destroyPath;
    public bool inAction;

    public float overallDistance;
    public float distanceToPoint = 0;
    public float vectorDistance;
    public float distanceBetweenPoints;
    public float amountOfPoints;
    public Vector3 point;

    public LineRenderer lineRenderer;
    public GameObject gObj;
    public IEnumerator coroutine;


    // Start is called before the first frame update
    void Start()
    {
        gObj = gameObject;
        coroutine = WaitAndMove(gObj.transform.position, 0.1f);
        StartCoroutine("InputCheck");
        distanceBetweenPoints = 0.0001f;
        amountOfPoints = 100000;
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
        pressed = false;
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
            MakePath();
        }
    }
/*    private void AsOftenAsPossible()
    {
       mousePosition = Misc.Tools.SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (makePath)
        {
            if (destroyPath)
            {
                lineRenderer.positionCount = 0;
                lineRenderer.positionCount = 2;
            }
            MakePath();
        } else
        {
            CancelInvoke();
        }
    }
*/
    private void LateUpdate()
    {
        if (Input.GetKeyDown("space"))
        {
            pressed = true;
        }
    }
    IEnumerator InputCheck()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (pressed)
            {
                pressed = false;
                //DoAction();
            }
        }
    }

    IEnumerator WaitAndMove(Vector3 position, float seconds)
    {
        Debug.Log(mousePositionList.Count);
        foreach (Vector3 point in mousePositionList)
        {
            yield return new WaitForSeconds(seconds);
            gObj.transform.localPosition = point;
        }
    } 
    private void DoAction()
    {
        inAction = true;
        lineRenderer.positionCount = 0;
        /*        if (mousePositionList.Count > 0)
                {
                        coroutine = WaitAndMove(gObj.transform.position, 0.1f);
                        StartCoroutine(coroutine);
               }
        */
        for(int i = 1; i < mousePositionList.Count; i++)
        {
            distanceToPoint = Vector3.Distance(gObj.transform.position, mousePositionList[i]);
            while(distanceToPoint > 0)
            {
                //liikuta gObj mousePositionList[i] suuntaan
                distanceToPoint = Vector3.Distance(gObj.transform.position, mousePositionList[i]);
            }

        }
        
        inAction = false;
        makePath = false;
        drawPath = false;
    }

    private void MakePath()
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
            if (Input.GetMouseButton(0) && lineRenderer.positionCount < amountOfPoints && overallDistance < 10)
            {
                Debug.Log(overallDistance);
                vectorDistance = (Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition));

                if (vectorDistance > distanceBetweenPoints)
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
            //InvokeRepeating("AsOftenAsPossible", 0, 0.02f);

        }
    }

}
