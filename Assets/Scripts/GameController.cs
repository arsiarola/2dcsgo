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

    public class GameController : MonoBehaviour
    {
        private GameStage gameStage = GameStage.Record;
        private int NextId { get; set; }

        [SerializeField] private Recorder recorder;
        private Dictionary<int, GameObject> objectRefs = new Dictionary<int, GameObject>();
        private Dictionary<int, GameObject> planningTypes = new Dictionary<int, GameObject>();
        private Dictionary<int, GameObject> dummyTypes = new Dictionary<int, GameObject>();
        private List<Dictionary<int, Recordable.RecordableState>> frames = new List<Dictionary<int, Recordable.RecordableState>>();

        private void Start()
        {
            Time.timeScale = 100.0f;
            UpdateStage();
        }

        private void Update()
        {

        }

        private void UpdateStage()
        {
            switch (gameStage) {
                case GameStage.CTOrders:
                    foreach (KeyValuePair<int, GameObject> pair in objectRefs) {
                        int id = pair.Key;
                        GameObject gObj = pair.Value;
                        gObj.SetActive(false);
                        /*if (gObj.GetComponent<Recordable.Operator>() != null)
                        {
                            if (gObj.GetComponent<Recordable.Operator>().side == Recordable.Side.CT)
                            {
                                Instantiate(ordersTypes[id]); 
                            }
                            else
                            {
                                //Instantiate(dummyTypes[id]);
                            }

                        }*/
                    }
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

        public void AddRecordable(ref GameObject objectRef, GameObject dummyType)
        {
            objectRefs.Add(NextId, objectRef);
            dummyTypes.Add(NextId, dummyType);
            NextId++;
        }
        public void AddRecordable(ref GameObject objectRef, GameObject dummyType, GameObject planningType)
        {
            planningTypes.Add(NextId, planningType);
            AddRecordable(ref objectRef, dummyType);
        }
    }
}


