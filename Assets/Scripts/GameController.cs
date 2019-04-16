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
        TReplay
    }

    public class GameController : MonoBehaviour
    {
        private GameStage gameStage = GameStage.CTOrders;


        
        [SerializeField] private Recorder recorder;
        private Dictionary<int, GameObject> ordersTypes = new Dictionary<int, GameObject>();
        private int nextId = 0;
        private Dictionary<int, GameObject> objectRefs = new Dictionary<int, GameObject>();
        private Dictionary<int, GameObject> dummyTypes = new Dictionary<int, GameObject>();
        private List<Dictionary<int, Recordable.RecordableState>> frameList = new List<Dictionary<int, Recordable.RecordableState>>();
        private void Start()
        {
            UpdateStage(); 
        }

        private void Update()
        {
            
        }

        private void UpdateStage ()
        {
            switch (gameStage)
            {
                case GameStage.CTOrders:
                    foreach(KeyValuePair<int, GameObject> pair in objectRefs)
                    {
                        int id = pair.Key;
                        GameObject gObj = pair.Value;
                        gObj.SetActive(false);
                        if (gObj.GetComponent<Recordable.Operator>() != null)
                        {
                            if (gObj.GetComponent<Recordable.Operator>().side == Recordable.Side.CT)
                            {
                                Instantiate(ordersTypes[id]); 
                            }
                            else
                            {
                                //Instantiate(dummyTypes[id]);
                            }

                        }

                    }
               
                    break;
                case GameStage.TOrders:
                    break;
                case GameStage.CTReplay:
                    break;
                case GameStage.TReplay:
                    break;
                    
                    
                    
            }
        }
        public int GetObjectId(ref GameObject objectRef, GameObject dummyType)
        {
            int id = nextId;
            nextId++;

            objectRefs.Add(id, objectRef);
            dummyTypes.Add(id, dummyType);
            return id;
        } 
        public int GetObjectId(ref GameObject objectRef, GameObject dummyType, GameObject ordersType)
        {
            int id = nextId;
            nextId++;

            objectRefs.Add(id, objectRef);
            dummyTypes.Add(id, dummyType);
            ordersTypes.Add(id, ordersType);
            return id;
        } 
    }
}


