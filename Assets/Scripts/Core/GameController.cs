using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum GameStage
    {
        CTOrders,
        TOrders,
        CTReplay,
        TReplay,
        Record
    }

    public enum GameFlag
    {
        RecordEnd,
        Null
    }

    public class GameController : MonoBehaviour
    {
        private GameStage gameStage = GameStage.Record;
        private int NextId { get; set; }
        private GameFlag flag = GameFlag.Null;

        [SerializeField] private Recorder recorder;
        [SerializeField] private Replayer replayer;
        public Dictionary<int, GameObject> recordableRefs = new Dictionary<int, GameObject>();
        private Dictionary<int, GameObject> recordablePlanningTypes = new Dictionary<int, GameObject>();
        public Dictionary<int, GameObject> recordableReplayTypes = new Dictionary<int, GameObject>();
        public List<Dictionary<int, Recordable.RecordableState>> frames = new List<Dictionary<int, Recordable.RecordableState>>();

        private void Start()
        {
            Time.timeScale = 1f;
            DisableRecordables();
            Record();
        }

        public void SetFlag(GameFlag flag)
        {
            this.flag = flag;
        }

        private void HandleFlags()
        {
            switch (flag)
            {
                case GameFlag.RecordEnd:
                    frames.AddRange(recorder.GetFrames());
                    Replay();
                    break;
            }
            flag = GameFlag.Null;
        }

        private void Replay()
        {
            replayer.Replay();
        }

        private void Update()
        {
            if (flag != GameFlag.Null)
            {
                HandleFlags();
            }
            
        }

        private void UpdateStage()
        {
            switch (gameStage) {
                case GameStage.CTOrders:
                    break;
                case GameStage.TOrders:
                    break;
                case GameStage.CTReplay:
                    break;
                case GameStage.TReplay:
                    break;
                case GameStage.Record:
                    Record();
                    break;
            }
        }

        private void Record()
        {
            recorder.Record(5000);
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


