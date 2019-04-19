using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static bool startCounting = false;
}


public class Recorder : MonoBehaviour
{
    float timeTracker = 0;
    int count_2 = 0;
    int milliSeconds = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator Test()
    {
        int count = 0;
        while (true) {
            yield return new WaitForFixedUpdate();
            count++;
            Debug.Log("End: " + count);
            timeTracker += Time.fixedDeltaTime * 1000;
            /* Get ObjectStates using foreach */
            if (timeTracker >= milliSeconds) {
                StopCoroutine("Test");
                Globals.startCounting = false;
                Debug.Log("END");
            }
            
        }
    }


    private void FixedUpdate()
    {
        if (Globals.startCounting) {
            count_2++;
            Debug.Log("Fixed: " + count_2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.fixedDeltaTime);
    }

    public void Record(int milliSeconds)
    {
        this.milliSeconds = milliSeconds;
        Globals.startCounting = true;
        StartCoroutine("Test");
    }
}

