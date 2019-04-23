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
            float speed = 20f;
            Vector3 relativePos = refMousePositionList[nextPoint] - transform.position;
            Quaternion target = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * speed);
            GetComponent<Rigidbody2D>().AddForce(transform.up * speed);

            if (Vector3.Distance(gameObject.transform.position, refMousePositionList[nextPoint]) < 0.5f)
            {
                nextPoint++;
            }
        }
    }

    public void SetMousePositionList(List<Vector3> list)
    {
        //Debug.Log("in fixed update");
        refMousePositionList = list;
    }
}
