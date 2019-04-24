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
        private float currentFrameAsFloat;
        private float replaySpeed;
        private bool pause;
        private bool isFirstFrame;

        /// <summary>
        /// Updates and creates replay objects to match the frame state.
        /// </summary>
        private void UpdateReplayObjects()
        {
            foreach (KeyValuePair<int, Recordable.RecordableState> pair in frame)
            {
                int id = pair.Key;

                // if replay object doesn't exist: create one
                if (!replayRefs.ContainsKey(id))
                {
                    replayRefs.Add(id, Instantiate(gameController.recordableReplayTypes[id]));
                }

                // init reference variables
                GameObject obj = replayRefs[id];
                Recordable.RecordableState state = frame[id];

                // update position and rotation
                obj.transform.position = state.position;
                obj.transform.eulerAngles = new Vector3(0, 0, state.rotation);

                // update animations
                foreach (Recordable.AnimationState anim in state.animations)
                {
                    Animator animator = obj.GetComponent<Animator>();
                    obj.GetComponent<Animator>().Play(anim.StateHash, anim.Layer, anim.Time);
                }
            }
        }

        /// <summary>
        /// Removes and destroys replay objects that don't exist in the current frame
        /// </summary>
        private void RemoveReplayObjects()
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
            else if (gameController.Frames.Count - 1 < currentFrameAsFloat)
            {
                currentFrameAsFloat = gameController.Frames.Count - 1;
            }

            currentFrame = (int)(Mathf.Round(currentFrameAsFloat));
            frame = gameController.Frames[currentFrame];
            UpdateReplayObjects();
            RemoveReplayObjects();

            if (currentFrameAsFloat >= gameController.Frames.Count - 1)
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

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Time.timeScale = 1f;
                DestroyReplayObjects();
                gameController.Flag = GameFlag.ReplayEnd;
                gameObject.SetActive(false);
            }
        }

        public void DestroyReplayObjects()
        {
            foreach (KeyValuePair<int, GameObject> pair in replayRefs)
            {
                GameObject obj = pair.Value;
                Destroy(obj);
            }
        }

        public void Replay()
        {
            // init variables
            gameObject.SetActive(true);
            currentFrame = 0;
            replayRefs = new Dictionary<int, GameObject>();
            replaySpeed = 1f;
            currentFrameAsFloat = 0;
            pause = false;
            isFirstFrame = true;
        }
    }
}


