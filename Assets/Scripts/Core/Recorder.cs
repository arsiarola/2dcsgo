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
        [SerializeField] private GameController gameController;

        /// <summary> Reference to the GameController Script </summary>
        private GameController GameController { get { return gameController; } }

        /// <summary> Time to be recorded in milliseconds </summary>
        private int RecordingLength { get; set; }

        /// <summary> Time in milliseconds since the recording started </summary>
        private float CurrentTime { get; set; }

        /// <summary> List of recorded frames. A frame contains a Id / state dictionary </summary>
        private List<Dictionary<int, Recordable.RecordableState>> RecordedFrames;

        /// <summary>
        /// Starts the recording/simulation process
        /// </summary>
        public void Record(int milliSeconds)
        {
            gameObject.SetActive(true); // enables the Recorder object
            InitVariables(milliSeconds);
            GameController.EnableRecordables();
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
                Dictionary<int, Recordable.RecordableState> currentFrame = new Dictionary<int, Recordable.RecordableState>();   // create a new frame where to store object states
                RecordedFrames.Add(currentFrame);   // add the frame to the list of recorded frames
                SaveRecordableStates(currentFrame); // save recordable states to that frame

                CurrentTime += Time.fixedDeltaTime * 1000;  // fixedDeltaTime, which is in seconds is converted to milliSeconds by multiplying it by 1000
                if (CurrentTime >= RecordingLength) {
                    Time.timeScale = 1.0f;
                    GameController.DisableRecordables();
                    GameController.Flag = GameFlag.RecordEnd;
                    StopCoroutine("AtFixedUpdateEnd");
                    gameObject.SetActive(false);
                }
            }
        }

        private void SaveRecordableStates(Dictionary<int, Recordable.RecordableState> frame)
        {
            foreach (KeyValuePair<int, GameObject> pair in GameController.recordableRefs)
            {
                int id = pair.Key;
                GameObject obj = pair.Value;
                if (obj != null)
                {
                    Recordable.RecordableState state = obj.GetComponent<Recordable.Recordable>().GetRecordableState();
                    frame.Add(id, state);
                }
            }
        }


        public List<Dictionary<int, Recordable.RecordableState>> GetFrames()
        {
            return RecordedFrames;
        }

        public Dictionary<int, Recordable.RecordableState> GetRecordableStates()
        {
            Dictionary<int, Recordable.RecordableState> frame = new Dictionary<int, Recordable.RecordableState>();
            SaveRecordableStates(frame);
            return frame;
        }
    }
}


