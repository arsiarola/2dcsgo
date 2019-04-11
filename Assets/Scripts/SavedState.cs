using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedState : MonoBehaviour
{
    public int nextKey = 0;
    public Dictionary<int, GameObject> objectRefs = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> dummyTypes = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> dummyRefs = new Dictionary<int, GameObject>();
    public Dictionary<int, ObjectState> frame = new Dictionary<int, ObjectState>();
    public List<Dictionary<int, ObjectState>> frameList = new List<Dictionary<int, ObjectState>>();
    bool replay = false;
    public int replayFrame;
    public GameObject explosion;

    public struct ObjectState
    {
        public Vector3 position;
        public float rotation;
        public Vector2 velocity;
        public bool attack;
    }

    void Start()
    {
        frameList.Add(frame);
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
                        dict.Value.GetComponent<Rigidbody2D>().velocity = frameList[replayFrame - 1][dict.Key].velocity;
                    }
                }
            }
            else
            {
                //Debug.Log(replayFrame + ", " + replay + ", " + frameStream.Count);
                foreach (KeyValuePair<int, GameObject> dict in objectRefs) {
                    if (frameList[replayFrame].ContainsKey(dict.Key)) {
                        dummyRefs[dict.Key].transform.position = frameList[replayFrame][dict.Key].position;
                        dummyRefs[dict.Key].transform.eulerAngles = new Vector3(0, 0, frameList[replayFrame][dict.Key].rotation);
                        if (frameList[replayFrame][dict.Key].attack == true) {
                            dummyRefs[dict.Key].GetComponent<Animator>().SetTrigger("Attack");
                        }
                    }
                    else if (dummyRefs[dict.Key] != null) {
                        Instantiate(explosion, frameList[replayFrame - 1][dict.Key].position, Quaternion.identity);
                        Destroy(dummyRefs[dict.Key]);
                    }
                }
                replayFrame++;
            }
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
            dummyRefs.Add(dict.Key, Instantiate(dummyTypes[dict.Key], new Vector3(0, 0, 0), Quaternion.identity));
        }
        replay = true;
        replayFrame = 0;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown("space") && !replay)
        {
            Replay();
        }
        if (!replay)
        {
            frame = new Dictionary<int, ObjectState>();
            frameList.Add(frame);
        } 
    }

    public void UpdateFrameState(int key, ObjectState objectState)
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
