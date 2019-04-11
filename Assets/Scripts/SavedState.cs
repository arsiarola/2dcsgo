using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedState : MonoBehaviour
{
    public int frame = 0;
    public int nextKey = 0;
    public Dictionary<int, ObjectReferences> references = new Dictionary<int, ObjectReferences>();
    public Dictionary<int, GameObject> dummyRef = new Dictionary<int, GameObject>();
    public List<Dictionary<int, ObjectState>> frameStream = new List<Dictionary<int, ObjectState>>();
    public Dictionary<int, ObjectState> frameState;
    bool replay = false;
    public int replayFrame;
    public GameObject explosion;

    public struct ObjectReferences
    {
        public GameObject dummy;
        public GameObject me;
    }

    public struct ObjectState
    {
        public Vector3 position;
        public float rotation;
        public Vector2 velocity;
        public bool attack;
    }

    private void AddFrameState()
    {
        frameState = new Dictionary<int, ObjectState>();
        frameStream.Add(frameState);
    }

    void Start()
    {
        AddFrameState();
    }

    void Update()
    {
        if (replay)
        {
            if (frameStream.Count - 1 <= replayFrame)
            {
                replay = false;
                foreach (KeyValuePair<int, ObjectReferences> dict in references) {
                    Destroy(dummyRef[dict.Key]);
                    GameObject me = dict.Value.me;
                    if (me != null) {
                        me.SetActive(true);
                        me.transform.position = frameStream[replayFrame - 1][dict.Key].position;
                        me.transform.eulerAngles = new Vector3(0, 0, frameStream[replayFrame - 1][dict.Key].rotation);
                        me.GetComponent<Rigidbody2D>().velocity = frameStream[replayFrame - 1][dict.Key].velocity;
                    }
                }
            }
            else
            {
                Debug.Log(replayFrame + ", " + replay + ", " + frameStream.Count);
                foreach (KeyValuePair<int, ObjectReferences> dict in references) {
                    if (frameStream[replayFrame].ContainsKey(dict.Key)) {
                        dummyRef[dict.Key].transform.position = frameStream[replayFrame][dict.Key].position;
                        dummyRef[dict.Key].transform.eulerAngles = new Vector3(0, 0, frameStream[replayFrame][dict.Key].rotation);
                        if (frameStream[replayFrame][dict.Key].attack == true) {
                            dummyRef[dict.Key].GetComponent<Animator>().SetTrigger("Attack");
                        }
                    }
                    else if (dummyRef[dict.Key] != null) {
                        Instantiate(explosion, frameStream[replayFrame - 1][dict.Key].position, Quaternion.identity);
                        Destroy(dummyRef[dict.Key]);
                    }
                }
                replayFrame++;
            }
        }
    }

    private void Replay()
    {
        dummyRef = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, ObjectReferences> dict in references) {
            if (dict.Value.me != null) {
                dict.Value.me.SetActive(false);
            }
            dummyRef.Add(dict.Key, Instantiate(references[dict.Key].dummy, new Vector3(0, 0, 0), Quaternion.identity));
        }
        replay = true;
        replayFrame = 0;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown("space"))
        {
            Replay();
        }
        if (!replay)
        {
            frame++;
            AddFrameState();
        } 
    }

    public void UpdateFrameState(int key, ObjectState objectState)
    {
        if (!replay)
        {
            frameState.Add(key, objectState);
        }
    }

    public int GetObjectKey(GameObject d, ref GameObject m)
    {
        int key = nextKey;
        nextKey++;

        ObjectReferences of = new ObjectReferences
        {
            dummy = d,
            me = m
        };
        references.Add(key, of);
        return key;
    }
}
