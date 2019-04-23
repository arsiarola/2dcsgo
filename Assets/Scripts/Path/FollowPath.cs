﻿using System.Collections;
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
            if (gameObject.transform.position != refMousePositionList[nextPoint])
            {
                transform.position = Vector3.MoveTowards(gameObject.transform.position, refMousePositionList[nextPoint], 10f);
            }
            else
            {
                nextPoint++;
            }
        }
    }

    public void SetMousePositionList(List<Vector3> list)
    {
        Debug.Log("in fixed update");
        refMousePositionList = list;
    }
}
