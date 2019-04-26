using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     create the path and draw it
/// </summary>
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
    private GameObject circle;
    private Rigidbody2D rbCircle;

    private void Start()
    {
        gameController = GameObject.Find(Misc.Constants.GameControllerName).GetComponent<Core.GameController>();   
        gObj = gameObject;
        circle = gObj.transform.Find("Sphere").gameObject;
        rbCircle = circle.GetComponent<Rigidbody2D>();
        circle.SetActive(false);

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
            for (int i = 0; i < lineRenderer.positionCount; i++) //draw path from the beginning to the last point
            {
                lineRenderer.SetPosition(i, mousePositionList[i]);
            }
        }
    }

    private void CreatePath()
    {

                /*if (normal == new Vector3(1, 0, 0) || normal == new Vector3(-1, 0, 0)) {

                }*/
        drawPath = true; // now that we have starterd creating the path, afterward its okay to draw the path

        ///<summary> 
        ///     if mouse is held down, mouseposition is far enough and the mouseposition is not inside a wall add a point to mousePositionList
        /// </summary>
        if (Input.GetMouseButton(0) && lineRenderer.positionCount < amountOfPoints) {
            float raycastCircleRadius = 0.4f;
            float offset = 0.025f;
            Vector3 direction = mousePosition - rbCircle.transform.position;
            float distance = Vector3.Distance(rbCircle.transform.position, mousePosition);
            Vector3 originalPos = rbCircle.transform.position;
            Vector3 raycastOrigin = rbCircle.transform.position + direction.normalized * offset;
            RaycastHit2D hit = Physics2D.CircleCast(raycastOrigin, raycastCircleRadius , direction, distance, 1<<8);

            //If something was hit.
            if (hit.collider != null) {
                Vector3 normal = hit.normal;
                
                Vector3 newPos = rbCircle.transform.position;
                /*if (hit.normal.x == 0f || hit.normal.y == 0) {
                    
                } else {
                    newPos += direction.normalized * hit.distance;
                }*/

                Vector3 yComponent = new Vector3(0, direction.y);
                Vector3 xComponent = new Vector3(direction.x, 0);

                Vector3 yRaycastOrigin = rbCircle.transform.position + yComponent.normalized * offset;
                Vector3 xRaycastOrigin = rbCircle.transform.position + xComponent.normalized * offset;

                RaycastHit2D yHit = Physics2D.CircleCast(yRaycastOrigin, raycastCircleRadius, yComponent, yComponent.magnitude, 1 << 8);
                RaycastHit2D xHit = Physics2D.CircleCast(xRaycastOrigin, raycastCircleRadius, xComponent, xComponent.magnitude, 1 << 8);

                if (yHit.collider != null) {
                    newPos += yComponent.normalized * (yHit.distance - offset);
                }
                else {
                    newPos += yComponent;
                }
                if (xHit.collider != null) {
                    newPos += xComponent.normalized * (xHit.distance - offset);
                }
                else {
                    newPos += xComponent;
                }
                rbCircle.transform.position = newPos;

                RaycastHit2D lateHit = Physics2D.CircleCast(rbCircle.transform.position, raycastCircleRadius, direction, 0, 1 << 8);
                while (lateHit.collider != null) {
                    lateHit = Physics2D.CircleCast(rbCircle.transform.position + direction.normalized * offset, raycastCircleRadius, direction, 0, 1 << 8);
                    rbCircle.transform.position -= direction.normalized * offset;
                }

                Vector3 changeVector = -originalPos + rbCircle.transform.position;
                RaycastHit2D testCast = Physics2D.Raycast(originalPos, changeVector, changeVector.magnitude, 1 << 8);
                if (testCast.collider != null) {
                    Debug.Log("Error");
                    rbCircle.transform.position = originalPos + changeVector.normalized * (testCast.distance - raycastCircleRadius);
                }

            }  else {
                rbCircle.transform.position = mousePosition;
            }
                
            vectorDistance = (Vector3.Distance(mousePositionList[mousePositionList.Count - 1], mousePosition));
            if (vectorDistance > pointAccuracy) // checking if the mousePositon is far enough to add the point
            {
                lineRenderer.positionCount++; // the length of the drawn path can now also increase since we have one more point to draw
                mousePositionList.Add(rbCircle.transform.position);
            } 
            
        }
        ///<summary>
        ///     if mouse is not held down stop creating and drawing the path and 
        /// </summary>
        else 
        {       
            circle.SetActive(false);
            createPath = false;
            drawPath = false;
            for (int i = 0; i < lineRenderer.positionCount; i++) //draw the line for the last time just incase
            {
                lineRenderer.SetPosition(i, mousePositionList[i]);
            }
        }
    }

    ///<summary>when mouse is moved close enough to the player and held down reset the mousePositionList and the previously drawn path, enable creating path</summary>
    private void MouseOver() 
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
            circle.SetActive(true);
            circle.transform.position = gObj.transform.position;
        }
    }
}
