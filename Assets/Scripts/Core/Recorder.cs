using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static bool startCounting = false;
}

namespace Core
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField] private GameController gameController;

        private int recordingLength;    // in milliseconds
        private float currentTime;  // time in milliseconds since the recording started
        private List<Dictionary<int, Recordable.RecordableState>> frames;
        private Dictionary<int, Recordable.RecordableState> frame = new Dictionary<int, Recordable.RecordableState>();

        private void SaveRecordableStates()
        {
            foreach (KeyValuePair<int, GameObject> pair in gameController.recordableRefs)
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

        IEnumerator AtFixedUpdateEnd()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                frame = new Dictionary<int, Recordable.RecordableState>();
                frames.Add(frame);

                SaveRecordableStates();
                currentTime += Time.fixedDeltaTime * 1000;
                if (currentTime >= recordingLength)
                {
                    Time.timeScale = 1.0f;
                    Globals.startCounting = false;
                    gameController.DisableRecordables();
                    gameController.SetFlag(GameFlag.RecordEnd);
                    StopCoroutine("AtFixedUpdateEnd");
                }
            }
        }

        public void Record(int milliSeconds)
        {
            // initialize variables
            currentTime = 0;
            recordingLength = milliSeconds;
            frames = new List<Dictionary<int, Recordable.RecordableState>>();

            Globals.startCounting = true;
            gameController.EnableRecordables();
            Time.timeScale = 100f;
            StartCoroutine("AtFixedUpdateEnd");
        }

        public List<Dictionary<int, Recordable.RecordableState>> GetFrames()
        {
            return frames;
        }
    }
}


