using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     create the path and draw it
/// </summary>
public class MakePath : MonoBehaviour
{
    public List<Vector3> mousePositionList;
    public bool willPlant = false;
    public bool willDefuse = false;
    private Vector3 mousePosition;

    private bool spacePressed;
    private bool createPath;
    private bool drawPath;
    private bool enableDrag = true;
    private bool mouseInWall = false;
    private bool setLookDirection = false;

    private float vectorDistance;
    private float amountOfPoints;
    private float pointAccuracy;

    private LineRenderer lineRenderer;
    private GameObject gObj;
    private Core.GameController gameController;
    private GameObject circle;
    private Rigidbody2D rbCircle;
    private GameObject pathEndMark;

    private void Awake()
    {

        gameController = GameObject.Find(Misc.Constants.GAME_CONTROLLER_NAME).GetComponent<Core.GameController>();   
        gObj = gameObject;
        circle = gObj.transform.Find("Sphere").gameObject;
        rbCircle = circle.GetComponent<Rigidbody2D>();
        circle.SetActive(false);
        pathEndMark = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pathEndMark.transform.parent = transform;
        pathEndMark.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

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
        lineRenderer.sortingOrder = 10;

        createPath = false;
        drawPath = false;
        spacePressed = false;
    }

    ///<summary>Adds a point to mousePostionList if conditions are right and after that draw the path</summary>
    private void Update()
    {
        mousePosition = Misc.Tools.SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        MouseOver();
        if (createPath)
        {
            CreatePath(); //includes the point adding to the pointList
        }
        if (drawPath)
        {
            DrawPath();
        }

        if(setLookDirection) {
            SetLookDirection();
        }

        if(mousePositionList.Count > 1)
            pathEndMark.SetActive(true);
        else
            pathEndMark.SetActive(false);

        if(mousePositionList.Count > 1) {
            pathEndMark.transform.position = mousePositionList[mousePositionList.Count - 1];
        }
    }

    public void DrawPath()
    {
        lineRenderer.positionCount = mousePositionList.Count;
        for (int i = 0; i < mousePositionList.Count; i++) //draw path from the beginning to the last point
        {
            lineRenderer.SetPosition(i, mousePositionList[i]);
        }
    }

    private void CalculatePosition(Vector3 lastPos)
    {
        bool bothHit;
        float raycastCircleRadius = 0.4f;
        float offset = 0.025f;
        Vector3 direction = mousePosition - rbCircle.transform.position;
        float distance = Vector3.Distance(rbCircle.transform.position, mousePosition);
        Vector3 originalPos = rbCircle.transform.position;
        Vector3 raycastOrigin = rbCircle.transform.position + direction.normalized * offset;
        RaycastHit2D hit = Physics2D.CircleCast(raycastOrigin, raycastCircleRadius, direction, distance, 1 << 8);

        //If something was hit.
        if (hit.collider != null)
        {

            Vector3 newPos = rbCircle.transform.position;

            Vector3 yComponent = new Vector3(0, direction.y);
            Vector3 xComponent = new Vector3(direction.x, 0);

            Vector3 yRaycastOrigin = rbCircle.transform.position + yComponent.normalized * offset;
            Vector3 xRaycastOrigin = rbCircle.transform.position + xComponent.normalized * offset;

            RaycastHit2D yHit = Physics2D.CircleCast(yRaycastOrigin, raycastCircleRadius, yComponent, yComponent.magnitude, 1 << 8);
            RaycastHit2D xHit = Physics2D.CircleCast(xRaycastOrigin, raycastCircleRadius, xComponent, xComponent.magnitude, 1 << 8);

            if (yHit.collider != null)
            {
                newPos += yComponent.normalized * (yHit.distance - offset);
            }
            else
            {
                newPos += yComponent;
            }
            if (xHit.collider != null)
            {
                newPos += xComponent.normalized * (xHit.distance - offset);
            }
            else
            {
                newPos += xComponent;
            }
            rbCircle.transform.position = newPos;

            if (yHit.collider != null && xHit.collider != null)
            {
                bothHit = true;
            }
            else
            {
                bothHit = false;
            }

            /*if (bothHit && lastPos != rbCircle.transform.position)
            {
                Debug.Log("Diagonal slide");
                CalculatePosition(rbCircle.transform.position);
            }*/

            RaycastHit2D lateHit = Physics2D.CircleCast(rbCircle.transform.position, raycastCircleRadius, direction, 0, 1 << 8);
            while (lateHit.collider != null)
            {
                lateHit = Physics2D.CircleCast(rbCircle.transform.position + direction.normalized * offset, raycastCircleRadius, direction, 0, 1 << 8);
                rbCircle.transform.position -= direction.normalized * offset;
            }

            Vector3 changeVector = -originalPos + rbCircle.transform.position;
            RaycastHit2D testCast = Physics2D.Raycast(originalPos, changeVector, changeVector.magnitude, 1 << 8);
            if (testCast.collider != null)
            {
                rbCircle.transform.position = originalPos + changeVector.normalized * (testCast.distance - raycastCircleRadius);
            }
        }
        else
        {
            rbCircle.transform.position = mousePosition;
        }
    }

    public void ReceiveDefuse(bool b)
    {
        willDefuse = b;
        gameController.Planning.IsPaused = false;
    }

    public void ReceivePlant(bool b)
    {
        willPlant = b;
        gameController.Planning.IsPaused = false;
    }

    private void CreatePath()
    {

                /*if (normal == new Vector3(1, 0, 0) || normal == new Vector3(-1, 0, 0)) {

                }*/
        drawPath = true; // now that we have starterd creating the path, afterward its okay to draw the path
        willPlant = false;
        willDefuse = false;
        if (Input.GetMouseButton(0) && lineRenderer.positionCount < amountOfPoints) {
            CalculatePosition(rbCircle.transform.position);
                
            vectorDistance = (Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition));
            if (vectorDistance > pointAccuracy) // checking if the mousePositon is far enough to add the point
            {
                lineRenderer.positionCount++; // the length of the drawn path can now also increase since we have one more point to draw
                mousePositionList.Add(rbCircle.transform.position);
            } 
            
        }
        else 
        {       
            circle.SetActive(false);
            createPath = false;
            drawPath = false;
            for (int i = 0; i < lineRenderer.positionCount; i++) //draw the line for the last time just incase
            {
                lineRenderer.SetPosition(i, mousePositionList[i]);
            }

            int id = gameController.Planning.PlanningRefs[gameObject];
            RecordableState.RecordableState state = gameController.Planning.LastFrame[id];

            if (Vector3.Distance(gameController.Bomb.transform.position, mousePositionList[mousePositionList.Count - 1]) < 0.9f && gameController.Bomb.GetComponent<BombScript>().Planted) {
                if (state.GetProperty<RecordableState.BaseAI>().Side == Core.Side.CounterTerrorist) {
                    gameController.PauseMenu.BringDefuse(gameObject);
                    gameController.Planning.IsPaused = true;
                }
            }

            Collider2D collision = Physics2D.OverlapPoint(mousePositionList[mousePositionList.Count - 1], 1 << 9);
            if (collision != null) {
                if (state.GetProperty<RecordableState.BaseAI>().Side == Core.Side.Terrorist) {
                    if (state.GetProperty<RecordableState.Bomb>().HasBomb == true) {
                        gameController.PauseMenu.BringPlant(gameObject);
                        gameController.Planning.IsPaused = true;
                    }
                }
            }
        }
    }

    ///<summary>when mouse is moved close enough to the player and held down reset the mousePositionList and the previously drawn path, enable creating path</summary>
    private void MouseOver()
    {
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(mousePosition, gObj.transform.position) < 0.5) {
            mousePositionList = new List<Vector3>();
            mousePositionList.Add(gObj.transform.position);
            mousePositionList.Add(mousePosition);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, gObj.transform.position);
            lineRenderer.SetPosition(1, mousePosition);
            createPath = true;
            drawPath = false;
            circle.SetActive(true);
            circle.transform.position = gObj.transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && Vector3.Distance(mousePosition, gObj.transform.position) < 0.5) {
            setLookDirection = true;
        }
        else if (mousePositionList.Count > 0 && createPath == false) {
            if (Input.GetMouseButtonDown(0) && Vector3.Distance(mousePosition, mousePositionList[mousePositionList.Count - 1]) < 0.5) {
                rbCircle.transform.position = mousePositionList[mousePositionList.Count - 1];
                circle.SetActive(true);
                createPath = true;
            }
        }
    }

    private void SetLookDirection() {
        if (Input.GetKey(KeyCode.Mouse1) ) { 

            Vector3 vectorToTarget = mousePosition - gObj.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.5f);
        } else {
            setLookDirection = false;
        }

    }
}
