using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Handles the Recording/Simulation stage
    /// </summary>
    public class Recorder : MonoBehaviour
    {
        /// <summary> Reference to the GameController Script </summary>
        private GameController GameController { get { return gameController; } }
        [SerializeField] private GameController gameController;

        /// <summary> Time to be recorded in milliseconds </summary>
        private int RecordingLength { get; set; }

        /// <summary> Time in milliseconds since the recording started </summary>
        private float CurrentTime { get; set; }

        /// <summary> List of recorded frames. A frame contains a Id / state dictionary </summary>
        private List<Dictionary<int, Recordable.RecordableState>> RecordedFrames;

        /// <summary>
        /// Starts the recording/simulation process
        /// </summary>
        /// <param name="milliSeconds">The amount of time in milliseconds to be recorded</param>
        public void Record(int milliSeconds)
        {
            gameObject.SetActive(true); // enables the Recorder object
            InitVariables(milliSeconds);
            GameController.EnableRecordables(); // enable recordables for simulation
            Time.timeScale = 100f;  // simulate as fast as possible
            StartCoroutine("AtFixedUpdateEnd"); // start coroutine that runs at the end of every fixed update
        }

        /// <summary>
        /// Sets all variables to their default values
        /// </summary>
        private void InitVariables(int milliSeconds)
        {
            CurrentTime = 0;
            RecordingLength = milliSeconds;
            RecordedFrames = new List<Dictionary<int, Recordable.RecordableState>>();
        }

        /// <summary>
        /// Saves states to a new frame at the end of every fixedUpdate loop
        /// </summary>
        IEnumerator AtFixedUpdateEnd()
        {
            while (true) {
                yield return new WaitForFixedUpdate();
                RecordedFrames.Add(GetRecordableStates());   // get states and add the created dictionary of id-state to the list of frames

                CurrentTime += Misc.Tools.SecondsToMilliSeconds(Time.fixedDeltaTime);  // expired time is the same as the fixed time step
                if (CurrentTime >= RecordingLength) {   // if we have recorded for the given time
                    Exit();
                }
            }
        }

        /// <summary>
        /// Get the current state of every recordable
        /// </summary>
        /// <returns>The current state of every recordable stored in a Id-State dictionary </returns>
        public Dictionary<int, Recordable.RecordableState> GetRecordableStates()
        {
            Dictionary<int, Recordable.RecordableState> frame = new Dictionary<int, Recordable.RecordableState>();  // create a new frame where to store object states
            // goes through the all of the recordables and saves their state
            foreach (KeyValuePair<int, GameObject> pair in GameController.recordableRefs)
            {
                int id = pair.Key;
                GameObject obj = pair.Value;
                // if the recordable has not been destroyed, get its state and add it to the frame
                if (obj != null)
                {
                    Recordable.RecordableState state = obj.GetComponent<Recordable.Recordable>().GetRecordableState();  // gets the state by calling its getRecordableState method from the Recordable script
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
            Time.timeScale = 1.0f;  // reset time scale to normal
            GameController.DisableRecordables();    // disable recordables now that the simulating is complete
            GameController.Flag = GameFlag.RecordEnd;   // change the game controller flag
            StopCoroutine("AtFixedUpdateEnd");  // important to stop the coroutine so it doesn't keep running in the background
            gameObject.SetActive(false);    // disable the Recorder gameObject for claritys sake
        }

        /// <summary>
        /// Get the list of recorded frames.
        /// </summary>
        /// <returns>List of dictionaries with a key-value pair of ID-RecordableState.</returns>
        public List<Dictionary<int, Recordable.RecordableState>> GetRecordedFrames()
        {
            return RecordedFrames;
        }
    }
}


