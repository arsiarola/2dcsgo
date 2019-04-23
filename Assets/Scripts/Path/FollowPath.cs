using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public List<Vector3> refMousePositionList = new List<Vector3>();
    public int nextPoint = 1;
    void Start()
    {
        //StartCoroutine("DoAction");
    }


    private void FixedUpdate()
    {
        if (nextPoint < refMousePositionList.Count)
        {
            //transform.position = Vector3.MoveTowards(gameObject.transform.position, refMousePositionList[nextPoint], 10f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * speed);
            //Vector3 newDir = Vector3.RotateTowards(transform.up, relativePos, step, 2f);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
            float speed = 2.5f * Time.fixedDeltaTime;
            float distance;
            float t;
            distance = Vector3.Distance(transform.position, refMousePositionList[nextPoint]);
            
            while (distance < speed && nextPoint + 1 < refMousePositionList.Count) {
                Debug.Log("True");
                nextPoint++;
                distance = Vector3.Distance(transform.position, refMousePositionList[nextPoint]);
            }
            Debug.Log(nextPoint + ", " + refMousePositionList.Count);
            t = speed / distance;
            /*do {
                if (nextPoint + 1 < refMousePositionList.Count) nextPoint++;
                distance = Vector3.Distance(transform.position, refMousePositionList[nextPoint]);
                t = speed / distance;
            } while (t > 1 && nextPoint + 1 < refMousePositionList.Count);*/


            int lookTowardsPoint = nextPoint;
            while (lookTowardsPoint < refMousePositionList.Count && lookTowardsPoint < nextPoint + 2) {
                lookTowardsPoint++;
            }
            Vector3 relativePos = refMousePositionList[nextPoint] - transform.position;
            Quaternion target = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = target;
            transform.position = Vector3.Lerp(transform.position, refMousePositionList[nextPoint], t);
        }
    }

    public void SetMousePositionList(List<Vector3> list)
    {
        //Debug.Log("in fixed update");
        refMousePositionList = list;
        nextPoint = 1;
    }
}
