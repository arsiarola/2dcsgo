using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Handles the replaying stage
    /// </summary>
    public class Replayer : Task
    {
        /// <summary> Id / replayObject ref. Contains the objects that have been created for the replay </summary>
        private Dictionary<int, GameObject> ReplayRefs { get; set; }

        /// <summary> Id / state. Contains the state of the recordables in the current frame </summary>
        private Dictionary<int, RecordableState.RecordableState> CurrentFrame { get; set; }

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
        private void OnEnable()
        { 
            InitVariables();
        }

        /// <summary>
        /// Sets all variables to their default values
        /// </summary>
        private void InitVariables()
        {
            ReplayRefs = new Dictionary<int, GameObject>(); // we will create the replay objects from scratch
            ReplaySpeed = 1f;   // set speed to normal
            CurrentFrameAsFloat = (GameController.Replays / 2) * (Misc.Tools.MilliSecondsToSeconds(Misc.Constants.RECORD_LENGTH) / Time.fixedDeltaTime);   // we start with frame zero (placeholder)
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
            if (Input.GetKeyDown(KeyCode.Minus)) {
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
            if (Input.GetKeyDown(KeyCode.Space)) {
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
            int AIid = GameController.SideAIs[GameController.Side];
            List<GameObject> visibleEnemies = CurrentFrame[AIid].GetProperty<RecordableState.BaseAI>().VisibleEnemies;

            foreach (KeyValuePair<int, RecordableState.RecordableState> pair in CurrentFrame)
            {
                int id = pair.Key;
                RecordableState.RecordableState state = CurrentFrame[id];

                bool hasReplayType = GameController.RecordableReplayTypes.ContainsKey(id);

                if (hasReplayType) {
                    GameObject obj = null;
                    bool isVisible = visibleEnemies.Contains(GameController.RecordableRefs[id]);
                    // if replay object doesn't exist: create one
                    if (!ReplayRefs.ContainsKey(id)) {
                        if (state.GetProperty<RecordableState.BaseAI>() != null) {
                            Side side = state.GetProperty<RecordableState.BaseAI>().Side;
                            RecordableState.OperatorState operatorState = state.GetProperty<RecordableState.OperatorState>();
                            if (side == GameController.Side && (operatorState == null || operatorState.IsAlive == true)) {
                                obj = Instantiate(GameController.RecordableReplayTypes[id]);
                                ReplayRefs.Add(id, obj);
                            }
                            else if (side != GameController.Side && isVisible) {
                                obj = Instantiate(GameController.RecordableReplayTypes[id]);
                                ReplayRefs.Add(id, obj);
                            }
                        }
                        else {
                            obj = Instantiate(GameController.RecordableReplayTypes[id]);
                            ReplayRefs.Add(id, obj);
                        }
                    } else {
                        obj = ReplayRefs[id];
                    }
                    // update state
                    if (obj != null) {
                        state.SetToObject(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Removes and destroys replay objects that don't exist in the current frame 
        /// </summary>
        private void RemoveReplayObjects()
        {
            int AIid = GameController.SideAIs[GameController.Side];
            List<GameObject> visibleEnemies = CurrentFrame[AIid].GetProperty<RecordableState.BaseAI>().VisibleEnemies;

            // destroy objects that dont exist in the frame and add their id to a list
            List<int> replayRefsToRemove = new List<int>(); 
            foreach (KeyValuePair<int, GameObject> pair in ReplayRefs)
            {
                int id = pair.Key;
                bool isVisible = visibleEnemies.Contains(GameController.RecordableRefs[id]);

                if (CurrentFrame.ContainsKey(id)) {
                    RecordableState.RecordableState state = CurrentFrame[id];
                    Side? side = null;
                    if (state.GetProperty<RecordableState.BaseAI>() != null) side = state.GetProperty<RecordableState.BaseAI>().Side;
                    RecordableState.OperatorState operatorState = state.GetProperty<RecordableState.OperatorState>();

                    if ((side != null && side != GameController.Side && !isVisible) || (side == GameController.Side && !operatorState.IsAlive)) {
                        GameObject obj = pair.Value;
                        Destroy(obj);
                        replayRefsToRemove.Add(id);
                    }
                } else {
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
            if (CurrentFrameAsFloat >= GameController.Frames.Count - 1 || IsPause) {
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
            DestroyReplayObjects();
            GameController.SendMessage(GameMessage.ReplayEnd);
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


