using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public List<Vector3> refMousePositionList = new List<Vector3>();
    public int nextPoint = 1;
    private float rot = 0;
    void Start()
    {
        //StartCoroutine("DoAction");
    }


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
        }
    }

    public void SetMousePositionList(List<Vector3> list)
    {
        //Debug.Log("in fixed update");
        refMousePositionList = list;
        nextPoint = 1;
        rot = 0;
    }
}
