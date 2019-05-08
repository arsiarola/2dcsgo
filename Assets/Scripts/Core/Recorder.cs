using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Handles the Recording/Simulation stage
    /// </summary>
    public class Recorder : Task
    {
        /// <summary> Time to be recorded in milliseconds </summary>
        public int RecordingLength { get; set; } = Misc.Constants.RECORD_LENGTH;

        /// <summary> Time in milliseconds since the recording started </summary>
        private float time { get; set; } = 0;

        public float CurrentTime { get; set; } = 0;

        /// <summary> List of recorded frames. A frame contains a Id / state dictionary </summary>
        private List<Dictionary<int, RecordableState.RecordableState>> RecordedFrames;

        /// <summary>
        /// Starts the recording/simulation process
        /// </summary>
        /// <param name="milliSeconds">The amount of time in milliseconds to be recorded</param>
        private void OnEnable()
        {
            InitVariables();
            StartCoroutine("AtFixedUpdateEnd"); // start coroutine that runs at the end of every fixed update
        }

        /// <summary>
        /// Sets all variables to their default values
        /// </summary>
        private void InitVariables()
        {
            time = 0;
            RecordedFrames = new List<Dictionary<int, RecordableState.RecordableState>>();
        }

        /// <summary>
        /// Saves states to a new frame at the end of every fixedUpdate loop
        /// </summary>
        IEnumerator AtFixedUpdateEnd()
        {
            while (true) {
                yield return new WaitForFixedUpdate();
                RecordedFrames.Add(GetRecordableStates());   // get states and add the created dictionary of id-state to the list of frames

                time += Misc.Tools.SecondsToMilliSeconds(Time.fixedDeltaTime);  // expired time is the same as the fixed time step
                if (time >= RecordingLength) {   // if we have recorded for the given time
                    Exit();
                }
                CurrentTime = 60 - (time / 1000);
            }
        }

        /// <summary>
        /// Get the current state of every recordable
        /// </summary>
        /// <returns>The current state of every recordable stored in a Id-State dictionary </returns>
        public Dictionary<int, RecordableState.RecordableState> GetRecordableStates()
        {
            Dictionary<int, RecordableState.RecordableState> frame = new Dictionary<int, RecordableState.RecordableState>();  // create a new frame where to store object states
            // goes through the all of the recordables and saves their state
            foreach (KeyValuePair<int, GameObject> pair in GameController.RecordableRefs)
            {
                int id = pair.Key;
                GameObject obj = pair.Value;
                // if the recordable has not been destroyed, get its state and add it to the frame
                if (obj != null)
                {
                    RecordableState.RecordableState state = obj.GetComponent<Recordable.Recordable>().GetState();  // gets the state by calling its getRecordableState method from the Recordable script
                    frame.Add(id, state);
                }
            }
            return frame;
        }

        /// <summary>
        /// Resets timescale, disables simulation objects, sends exit flag to gameController and etc.
        /// </summary>
        private void Exit()
        {
            GameController.SendMessage(GameMessage.RecordEnd);   // change the game controller flag
            StopCoroutine("AtFixedUpdateEnd");  // important to stop the coroutine so it doesn't keep running in the background
        }

        /// <summary>
        /// Get the list of recorded frames.
        /// </summary>
        /// <returns>List of dictionaries with a key-value pair of ID-RecordableState.</returns>
        public List<Dictionary<int, RecordableState.RecordableState>> GetRecordedFrames()
        {
            return RecordedFrames;
        }
    }
}


