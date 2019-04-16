using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public int nextKey = 0;
    public Dictionary<int, GameObject> objectRefs = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> dummyTypes = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> dummyRefs = new Dictionary<int, GameObject>();
    public Dictionary<int, Recordable.RecordableState> frame = new Dictionary<int, Recordable.RecordableState>();
    public List<Dictionary<int, Recordable.RecordableState>> frameList = new List<Dictionary<int, Recordable.RecordableState>>();
    bool replay = false;
    public int replayFrame;
    bool pressed = false;

    void Start()
    {
        StartCoroutine("InputCheck");
    }

    void Update()
    {
        if (replay)
        {
            if (frameList.Count - 1 <= replayFrame)
            {
                replay = false;
                foreach (KeyValuePair<int, GameObject> dict in objectRefs) {
                    Destroy(dummyRefs[dict.Key]);
                    if (dict.Value != null) {
                        dict.Value.SetActive(true);
                        dict.Value.transform.position = frameList[replayFrame - 1][dict.Key].position;
                        dict.Value.transform.eulerAngles = new Vector3(0, 0, frameList[replayFrame - 1][dict.Key].rotation);

                        if (dict.Value.GetComponent<Rigidbody2D>() != null)
                        {
                            dict.Value.GetComponent<Rigidbody2D>().velocity = frameList[replayFrame - 1][dict.Key].velocity;
                        }
                    }
                }
            }
            else
            {
                //Debug.Log(replayFrame + ", " + replay + ", " + frameStream.Count);
                foreach (KeyValuePair<int, GameObject> dict in objectRefs) {
                    if (frameList[replayFrame].ContainsKey(dict.Key)) {
                        if (!dummyRefs.ContainsKey(dict.Key))
                        {
                            dummyRefs.Add(dict.Key, Instantiate(dummyTypes[dict.Key], new Vector3(0, 0, 0), Quaternion.identity));
                        }

                        dummyRefs[dict.Key].transform.position = frameList[replayFrame][dict.Key].position;
                        dummyRefs[dict.Key].transform.eulerAngles = new Vector3(0, 0, frameList[replayFrame][dict.Key].rotation);

                        foreach (string trigger in frameList[replayFrame][dict.Key].animTriggers)
                        {
                            
                            dummyRefs[dict.Key].GetComponent<Animator>().SetTrigger(trigger);
                        }
                    }
                    else if (dummyRefs.ContainsKey(dict.Key)) {
                        Destroy(dummyRefs[dict.Key]);
                    }
                }
                replayFrame++;
            }
        }
        if (!replay)
        {
            frame = new Dictionary<int, Recordable.RecordableState>();
            frameList.Add(frame);
        }
    }

    private void Replay()
    {
        dummyRefs = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> dict in objectRefs) {
            if (dict.Value != null) {
                if (dict.Value.gameObject.transform.childCount > 0) {
                    dict.Value.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                dict.Value.SetActive(false);
            }
        }
        replay = true;
        replayFrame = 0;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown("space") && !replay)
        {
            pressed = true;
        }
    }

    IEnumerator InputCheck()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (pressed)
            {
                //Debug.Log("pressed");
                pressed = false;
                Replay();
            }
        }
    }

    public void FrameAddRecordableState(int key, Recordable.RecordableState objectState)
    {
        if (!replay)
        {
            frame.Add(key, objectState);
        }
    }

    public int GetObjectKey(GameObject dummyType, ref GameObject objectRef)
    {
        int key = nextKey;
        nextKey++;

        objectRefs.Add(key, objectRef);
        this.dummyTypes.Add(key, dummyType);
        return key;
    }
}
