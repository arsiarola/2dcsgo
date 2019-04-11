using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCR : MonoBehaviour
{
    public List<Vector3> mousePositions;
    public List<float> inputWS;
    public List<float> inputAD;
    public List<bool> attacks;
    public Vector3 originalPos;
    public int next = 0;
    public List<Vector3> enemyPositions;
    public List<GameObject> enemyObjects;
    public GameObject enemyType;

    void Start()
    {
        mousePositions = new List<Vector3>();
        inputWS = new List<float>();
        inputAD = new List<float>();
        originalPos = GameObject.Find("Player").transform.position;
        enemyPositions = new List<Vector3>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyObjects.AddRange(enemies);
        foreach (GameObject enemy in enemies)
        {
            enemyPositions.Add(enemy.transform.position);
        }
    }

    public void SaveEverything(Vector3 mousePosition, float inputWS, float inputAD, bool attack)
    {
        mousePositions.Add(mousePosition);
        this.inputWS.Add(inputWS);
        this.inputAD.Add(inputAD);
        attacks.Add(attack);
    }

    public void PrintMousePositions()
    {
        foreach (Vector3 ms in mousePositions) {
            Debug.Log(ms.x + ", " + ms.y);
        }
    }

    public bool IsNotLast()
    {
        return next < mousePositions.Count;
    }

    public void IncrementNext()
    {
        next++;
    }

    public Vector3 GetNextMousePosition()
    {
        Vector3 v = mousePositions[next];
        return v;
    }

    public float GetNextInputWS()
    {
        float f = inputWS[next];
        return f;
    }

    public float GetNextInputAD()
    {
        float f = inputAD[next];
        return f;
    }

    public bool GetNextAttack()
    {
        bool b = attacks[next];
        return b;
    }

    public void StartReplay()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        new WaitForSeconds(1);
        player.transform.position = originalPos;
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            if (enemyObjects[i] == null)
            {
                Instantiate(enemyType, enemyPositions[i], Quaternion.identity);
            }
            else
            {
                enemyObjects[i].transform.position = enemyPositions[i];
                enemyObjects[i].transform.rotation = Quaternion.identity;
            }
        }
        
    }

    public void ResetReplay()
    {
        //originalPos = GameObject.Find("Player").transform.position;
        next = 0;
    }

}
