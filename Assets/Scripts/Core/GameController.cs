﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
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

    public enum GameFlag
    {
        RecordEnd,
        ReplayEnd,
        PlanningEnd
    }

    public class GameController : MonoBehaviour
    {
        private GameStage stage = GameStage.Record;
        private GameStage Stage { get { return stage; } set { stage = value; IsStageChanged = true; } }
        private bool IsStageChanged { get; set; } = true;
        private int NextId { get; set; } = 0;
        public GameFlag? Flag { get; set; } = null;
        public List<Dictionary<int, Recordable.RecordableState>> Frames { get; private set; } = new List<Dictionary<int, Recordable.RecordableState>>();


        [SerializeField] private Recorder recorder;
        [SerializeField] private Replayer replayer;
        [SerializeField] private Planning planning;
        private Planning Planning { get { return planning; } }
        public Dictionary<int, GameObject> recordableRefs = new Dictionary<int, GameObject>();
        private Dictionary<int, GameObject> recordablePlanningTypes = new Dictionary<int, GameObject>();
        public Dictionary<int, GameObject> recordableReplayTypes = new Dictionary<int, GameObject>();
        

        private void Start()
        {
            Time.timeScale = 1f;
            Frames.Add(recorder.GetRecordableStates()); // get start frame. Not sure if necessary for the replay, but we do need to get the objects starting positions at least (then again these can be gained by other means)
            DisableRecordables();
        }

        private void HandleFlags()
        {
            switch (Flag)
            {
                case GameFlag.RecordEnd:
                    Frames.AddRange(recorder.GetFrames());
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
            recorder.Record(5000);
        }

        private void Replay()
        {
            replayer.Replay();
        }

        private void Plan()
        {
            Planning.Plan();
        }

        public void AddRecordable(ref GameObject reference, GameObject replayType, GameObject planningType)
        {
            recordableRefs.Add(NextId, reference);
            recordableReplayTypes.Add(NextId, replayType);
            recordablePlanningTypes.Add(NextId, planningType);
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
            foreach (KeyValuePair<int, GameObject> pair in recordableRefs)
            {
                GameObject obj = pair.Value;
                if (obj != null)
                {
                    DisableRecordableChildren(ref obj);
                    obj.SetActive(false);
                }
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


