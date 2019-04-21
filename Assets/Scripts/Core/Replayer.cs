using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Replayer : MonoBehaviour
    {
        [SerializeField] private GameController gameController;

        private Dictionary<int, GameObject> replayRefs;
        private Dictionary<int, Recordable.RecordableState> frame;
        private int currentFrame;
        private bool play;
        private float currentFrameAsFloat;
        private float replaySpeed;
        private bool pause;
        private bool isFirstFrame;

        private void UpdateReplayObjects()
        {
            foreach (KeyValuePair<int, Recordable.RecordableState> pair in frame)
            {
                int id = pair.Key;
                if (!replayRefs.ContainsKey(id))
                {
                    replayRefs.Add(id, Instantiate(gameController.recordableReplayTypes[id], new Vector3(0, 0, 0), Quaternion.identity));
                }

                GameObject obj = replayRefs[id];
                Recordable.RecordableState state = frame[id];

                obj.transform.position = state.position;
                obj.transform.eulerAngles = new Vector3(0, 0, state.rotation);

                foreach (Recordable.AnimationState anim in state.animations)
                {
                    Animator animator = obj.GetComponent<Animator>();
                    obj.GetComponent<Animator>().Play(anim.StateHash, anim.Layer, anim.Time);
                }
            }
        }

        private void RemoveDeadReplayObjects()
        {
            List<int> replayRefsToRemove = new List<int>();
            foreach (KeyValuePair<int, GameObject> pair in replayRefs)
            {
                int id = pair.Key;
                if (!frame.ContainsKey(id))
                {
                    GameObject obj = pair.Value;
                    Destroy(obj);
                    replayRefsToRemove.Add(id);
                }
            }
            foreach (int id in replayRefsToRemove)
            {
                replayRefs.Remove(id);
            }
        }

        private void Update()
        {
            if (play)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (pause)
                    {
                        pause = false;
                    }
                    else
                    {
                        pause = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    replaySpeed += 0.1f;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    replaySpeed -= 0.1f;
                }
                else if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    replaySpeed = 1f;
                }

                if (replaySpeed < 0)
                {
                    replaySpeed = 0;
                } 

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentFrameAsFloat -= 1f / Time.fixedDeltaTime;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentFrameAsFloat += 1f / Time.fixedDeltaTime;
                }
                else
                {
                    currentFrameAsFloat += Time.deltaTime / Time.fixedDeltaTime;
                }

                if (isFirstFrame)
                {
                    currentFrameAsFloat = 0;
                    isFirstFrame = false;
                }

                if (currentFrameAsFloat < 0)
                {
                    currentFrameAsFloat = 0;
                }
                else if (gameController.frames.Count - 1 < currentFrameAsFloat)
                {
                    currentFrameAsFloat = gameController.frames.Count - 1;
                }

                //Debug.Log(replaySpeed);
                Debug.Log(currentFrameAsFloat);

                currentFrame = (int)(Mathf.Round(currentFrameAsFloat));
                frame = gameController.frames[currentFrame];
                UpdateReplayObjects();
                RemoveDeadReplayObjects();

                if (currentFrameAsFloat >= gameController.frames.Count - 1)
                {
                    Time.timeScale = 0f;
                }
                else if (pause)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = replaySpeed;
                }
            }
        }

        public void Replay()
        {
            // init variables
            currentFrame = 0;
            replayRefs = new Dictionary<int, GameObject>();
            play = true;
            replaySpeed = 1f;
            currentFrameAsFloat = 0;
            pause = false;
            isFirstFrame = true;
        }
    }
}


