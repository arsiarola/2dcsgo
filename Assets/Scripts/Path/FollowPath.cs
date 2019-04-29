using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public List<Vector3> refMousePositionList = new List<Vector3>();
    public int nextPoint = 1;
    private float rot = 0;

    //shooting
    bool isMoving;
    bool enemySpotted;
    public System.Random random = new System.Random();
    int reactiontime;
    int fov;
    int segments = 10;


    private void FixedUpdate()
    {
        if (nextPoint < refMousePositionList.Count)
        {
            // calculate position
            float speed = 2.5f * Time.fixedDeltaTime;
            float distance = Vector3.Distance(transform.position, refMousePositionList[nextPoint]);
            while (distance < speed && nextPoint + 1 < refMousePositionList.Count) {
                nextPoint++;
                distance = Vector3.Distance(transform.position, refMousePositionList[nextPoint]);
            }
            float t = speed / distance;

            // calculate rotation
            int lookTowardsPoint = nextPoint;
            while (lookTowardsPoint + 1 < refMousePositionList.Count && lookTowardsPoint < nextPoint + 2) {
                lookTowardsPoint++;
                rot = 0;
            }  
            Vector3 vectorToTarget = refMousePositionList[lookTowardsPoint] - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            if (rot < 0.75f)
            {
                rot += 0.25f;
            }
            else if (rot < 1f)
            {
                rot += 0.25f / 4;
            }
            else
            {
                rot = 0;
            }

            // transform
            transform.rotation = Quaternion.Slerp(transform.rotation, q, rot);
            transform.position = Vector3.Lerp(transform.position, refMousePositionList[nextPoint], t);
            CheckFov();
        }
    }

    public void SetMousePositionList(List<Vector3> list)
    {
        //Debug.Log("in fixed update");
        refMousePositionList = list;
        nextPoint = 1;
        rot = 0;
    }

    public void Shoot() {
        if (isMoving) {
            reactiontime = random.Next(300, 400);
        } else {
            reactiontime = random.Next(200, 300);
        }
    }

    public void CheckFov() {
        Vector3 startPos = transform.position;
        Vector3 endPos;

        int startAngle = -fov/2;
        int finishAngle = fov / 2;

        int increment = fov / segments;

        RaycastHit hit;

        for(int i = startAngle; i < finishAngle; i+= increment) {
            endPos = (Quaternion.Euler(0, i, 0) * transform.forward).normalized * 100;

            if(Physics.Linecast(startPos, endPos, out hit)) {
                Debug.Log(hit.point);
            }
            Debug.DrawLine(startPos, endPos, Color.blue);
        }

    }
}

