using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Handles the replaying stage
    /// </summary>
    public class Replayer : MonoBehaviour
    {
        /// <summary> Reference to the GameController Script </summary>
        private GameController GameController { get { return gameController; } }
        [SerializeField] private GameController gameController;

        /// <summary> Id / replayObject ref. Contains the objects that have been created for the replay </summary>
        private Dictionary<int, GameObject> ReplayRefs { get; set; }

        /// <summary> Id / state. Contains the state of the recordables in the current frame </summary>
        private Dictionary<int, Recordable.RecordableState> CurrentFrame { get; set; }

        /// <summary> The value of the current frame as a float </summary>
        private float CurrentFrameAsFloat { get; set; }

        /// <summary> Is used to modify the timeScale. Is between [0, inf[. 1 is normal and 0 is paused </summary>
        private float ReplaySpeed { get; set; }

        /// <summary> Has pause key been pressed </summary>
        private bool IsPause { get; set; }

        /// <summary> Is this the first frame to be shown </summary>
        private bool IsFirstFrame { get; set; }

        /// <summary> Has exit key been pressed </summary>
        private bool IsExit { get; set; }

        /// <summary>
        /// Starts the Replay process
        /// </summary>
        public void Replay()
        {
            gameObject.SetActive(true); // activate the replayer object in order to activate the update of this script. Update is executed only if the script is enabled
            InitVariables();
        }

        /// <summary>
        /// Sets all variables to their default values
        /// </summary>
        private void InitVariables()
        {
            ReplayRefs = new Dictionary<int, GameObject>(); // we will create the replay objects from scratch
            ReplaySpeed = 1f;   // set speed to normal
            CurrentFrameAsFloat = 0;    // we start with frame zero (placeholder)
            IsPause = false;
            IsExit = false;
            IsFirstFrame = true;
        }

        /// <summary>
        /// Handles input as well as updating the replay objects every frame
        /// </summary>
        private void Update()
        {
            HandleInput();

            // change the current frame
            UpdateCurrentFrameAsFloat();
            int currentFrameAsInt = (int)(Mathf.Round(CurrentFrameAsFloat));    // round the float value, to get a specific frame
            CurrentFrame = GameController.Frames[currentFrameAsInt];

            // update and remove replay objects
            UpdateReplayObjects();
            RemoveReplayObjects();

            UpdateReplaySpeed();

            // exit the replayer
            if (IsExit) {
                Exit();
            }
        }

        /// <summary>
        /// Handles all input that is specific to the replayer
        /// </summary>
        private void HandleInput()
        {
            // pause
            if (Input.GetKeyDown(KeyCode.Space)) {
                IsPause = !IsPause;
            }

            // replay speed
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                ReplaySpeed += 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                ReplaySpeed -= 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace)) {
                ReplaySpeed = 1f;
            }

            // rewind/fastforward
            // it's rewind time! t. will smith
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                CurrentFrameAsFloat -= 1f / Time.fixedDeltaTime;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                CurrentFrameAsFloat += 1f / Time.fixedDeltaTime;
            }

            // exit
            if (Input.GetKeyDown(KeyCode.Escape)) {
                IsExit = true;
            }
        }

        /// <summary>
        /// Updates the current frames float value
        /// </summary>
        private void UpdateCurrentFrameAsFloat()
        {
            // update current frame's float value
            if (IsFirstFrame)   // if first frame make sure the float is 0. Can cause bugs if not
            {
                CurrentFrameAsFloat = 0;
                IsFirstFrame = false;
            }
            else {
                CurrentFrameAsFloat += Time.deltaTime / Time.fixedDeltaTime;
            }

            // dont let current frame go below 0 or over the last frame element
            if (CurrentFrameAsFloat < 0) {  // below
                CurrentFrameAsFloat = 0;
            }
            else if (GameController.Frames.Count - 1 < CurrentFrameAsFloat) {   // going over
                CurrentFrameAsFloat = GameController.Frames.Count - 1;
            }
        }

        /// <summary>
        /// Updates and creates replay objects to match the frame state.
        /// </summary>
        private void UpdateReplayObjects()
        {
            foreach (KeyValuePair<int, Recordable.RecordableState> pair in CurrentFrame)
            {
                int id = pair.Key;

                // if replay object doesn't exist: create one
                if (!ReplayRefs.ContainsKey(id))
                {
                    ReplayRefs.Add(id, Instantiate(GameController.RecordableReplayTypes[id]));
                }

                // init reference variables
                GameObject obj = ReplayRefs[id];
                Recordable.RecordableState state = CurrentFrame[id];

                // update position and rotation
                obj.transform.position = state.Position;
                obj.transform.eulerAngles = new Vector3(0, 0, state.Rotation);

                // update animations
                foreach (Recordable.AnimationState anim in state.AnimationLayers)
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
            // destroy objects that dont exist in the frame and add their id to a list
            List<int> replayRefsToRemove = new List<int>(); 
            foreach (KeyValuePair<int, GameObject> pair in ReplayRefs)
            {
                int id = pair.Key;
                if (!CurrentFrame.ContainsKey(id))
                {
                    GameObject obj = pair.Value;
                    Destroy(obj);
                    replayRefsToRemove.Add(id);
                }
            }
            // using the created list remove all elements from replayRefs that point to a null object
            foreach (int id in replayRefsToRemove)
            {
                ReplayRefs.Remove(id);
            }
        }

        /// <summary>
        /// Update the replay speed, which is the same as timeScale
        /// </summary>
        private void UpdateReplaySpeed()
        {
            // dont let replay speed go below 0
            if (ReplaySpeed< 0) {
                ReplaySpeed = 0;
            }

            // pausing
            if (CurrentFrameAsFloat >= GameController.Frames.Count - 1) {
                Time.timeScale = 0f;
            }
            else if (IsPause) {
                Time.timeScale = 0f;
            }
            else {
                Time.timeScale = ReplaySpeed;
            }
        }

        /// <summary>
        /// Resets global values, destroys replay objects, sends exit flag to gameController and etc.
        /// </summary>
        private void Exit()
        {
            Time.timeScale = 1f;    // reset timescale
            DestroyReplayObjects();
            GameController.Flag = GameFlag.ReplayEnd;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Destroys every replay object
        /// </summary>
        public void DestroyReplayObjects()
        {
            foreach (KeyValuePair<int, GameObject> pair in ReplayRefs)
            {
                GameObject obj = pair.Value;
                Destroy(obj);
            }
        }

        

        
    }
}


