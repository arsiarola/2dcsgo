using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// The stage of the game
    /// </summary>
    public enum GameStage
    {
        CTPlanning,
        TPlanning,
        CTReplay,
        TReplay,
        Record,
        Replay,
        Planning
    }

    /// <summary>
    /// Flag that represents the end of a stage
    /// </summary>
    public enum GameFlag
    {
        RecordEnd,
        ReplayEnd,
        PlanningEnd
    }

    /// <summary>
    /// Controls the game stage as well as key collections and variables
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>  </summary>
        private Recorder Recorder { get { return recorder; } }
        [SerializeField] private Recorder recorder;

        private Replayer Replayer { get { return replayer; } }
        [SerializeField] private Replayer replayer;

        private Planning Planning { get { return planning; } }
        [SerializeField] public Planning planning;

        /// <summary> The current game stage. If the value is changed, IsStateChanged is also set to true</summary>
        public GameStage Stage { get { return stage; } set { stage = value; IsStageChanged = true; } }
        private GameStage stage = GameStage.Planning;

        /// <summary> Has the stage been changed, and not handled yet </summary>
        private bool IsStageChanged { get; set; } = true;   // stage has been changed because we go to the first stage

        /// <summary> What is the next available id for recordables </summary>
        private int NextId { get; set; } = 0;   // start from zero

        /// <summary> Current flag that needs to be handled </summary>
        public GameFlag? Flag { get; set; } = null; // the question mark means that it can be null

        /// <summary> Current flag that needs to be handled </summary>
        public List<Dictionary<int, Recordable.RecordableState>> Frames { get; private set; } = new List<Dictionary<int, Recordable.RecordableState>>();

        
        
        public Dictionary<int, GameObject> recordableRefs = new Dictionary<int, GameObject>();
        public Dictionary<int, GameObject> recordablePlanningTypes = new Dictionary<int, GameObject>();
        public Dictionary<int, GameObject> recordableReplayTypes = new Dictionary<int, GameObject>();
        
        // Before start every Recordable that has been put to scene, execute their Awake method. These send a reference to the GameController
        private void Start()
        {
            Frames.Add(Recorder.GetRecordableStates()); // get start frame. Not sure if necessary for the replay, but we do need to get the objects starting positions at least (then again these can be gained by other means)
            DisableRecordables();
            Recorder.gameObject.SetActive(false);
            Replayer.gameObject.SetActive(false);
            planning.gameObject.SetActive(false);
        }

        private void HandleFlags()
        {
            switch (Flag)
            {
                case GameFlag.RecordEnd:
                    Frames.AddRange(Recorder.GetRecordedFrames());
                    Stage = GameStage.Replay;
                    break;
                case GameFlag.ReplayEnd:
                    Stage = GameStage.Planning;
                    break;
                case GameFlag.PlanningEnd:
                    Stage = GameStage.Record;
                    break;
            }
            Flag = null;
        }

        private void HandleStages()
        {
            switch (Stage)
            {
                case GameStage.Record:
                    Record();
                    break;
                case GameStage.Replay:
                    Replay();
                    break;
                case GameStage.Planning:
                    Plan();
                    break;
            }
            IsStageChanged = false;
        }

        private void Update()
        {
            if (Flag != null)
            {
                HandleFlags();
            }
            if (IsStageChanged)
            {
                HandleStages();
            }
            
        }

        private void Record()
        {
            Recorder.Record(5000);
        }

        private void Replay()
        {
            Replayer.Replay();
        }

        private void Plan()
        {
            Planning.Plan();
        }

        public void AddRecordable(ref GameObject reference, GameObject replayType, GameObject planningType)
        {
            recordableRefs.Add(NextId, reference);
            recordableReplayTypes.Add(NextId, replayType);
            if (planningType != null) recordablePlanningTypes.Add(NextId, planningType);
            NextId++;
        }

        private void DisableRecordableChildren(ref GameObject obj)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject child = obj.transform.GetChild(0).gameObject;
                DisableRecordableChildren(ref child);
            }
        }

        public void DisableRecordables()
        {
            List<int> removeIds = new List<int>();
            foreach (KeyValuePair<int, GameObject> pair in recordableRefs)
            {
                int id = pair.Key;
                GameObject obj = pair.Value;
                if (obj != null)
                {
                    DisableRecordableChildren(ref obj);
                    obj.SetActive(false);
                } else {
                    removeIds.Add(id);
                }
            }
            foreach (int id in removeIds) {
                recordableRefs.Remove(id);
            }
        }

        public void EnableRecordables()
        {
            foreach (KeyValuePair<int, GameObject> pair in recordableRefs)
            {
                GameObject obj = pair.Value;
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}


