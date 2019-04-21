using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Path : MonoBehaviour
{

    public List<Vector3> mousePositionList;
    public Vector3 mousePosition;

    public bool spacePressed;
    public bool makePath;
    public bool drawPath;
    public bool destroyPath;
    public bool inAction;

    public float overallDistance;
    public float vectorDistance;
    public float amountOfPoints;
    public float pointAccuracy;

    public LineRenderer lineRenderer;
    public GameObject gObj;
    public Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        gObj = gameObject;
        StartCoroutine("InputCheck");
        StartCoroutine("DoAction");
        rb = gObj.GetComponent<Rigidbody2D>();
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
            MakePath();
        }
    }
    
    private void LateUpdate()
    {
        if (Input.GetKeyDown("space"))
        {
            spacePressed = true;
        }
    }
    
    IEnumerator InputCheck()

    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (spacePressed)
            {
                spacePressed = false;
                inAction = true;
            }
        }
    }

    IEnumerator DoAction()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (inAction)
            {
                lineRenderer.positionCount = 0;
                //while(gameState = "Simulator")
                for (int i = 1; i < mousePositionList.Count; i++)
                {
                    while (Vector3.Distance(gObj.transform.position, mousePositionList[i]) != 0)
                    {
                        yield return new WaitForSeconds(0.001f);
                        transform.position = Vector3.MoveTowards(transform.position, mousePositionList[i], Time.deltaTime * 10f);
/*                      if (enemyInFov) 
 *                      {
                           yield return new WaitForSeconds(0.5);
                           while (enemyInFov)
                           {
                                gObj.shoot();
                           }
                        }
 */                   }
                }
/*              while(inAction)
                {
                  while (enemyInFov)
                  {
                      gObj.shoot();   //shoot() would include the calculation which enemy to shoot
                  }

                }
*/                mousePositionList = new List<Vector3>();
                inAction = false;
                makePath = false;
                drawPath = false;
            }
        }
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

}
