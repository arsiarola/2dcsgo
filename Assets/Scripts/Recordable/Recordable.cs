using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class Recordable : MonoBehaviour
    {
        [SerializeField] protected Core.GameController gameController;
        [SerializeField] protected GameObject dummyType;

        protected virtual void Awake()
        {
            if (gameController == null)
            {
                gameController = GameObject.Find(Misc.Constants.GameControllerName).GetComponent<Core.GameController>();
            }
            GameObject gObj = gameObject;
            SendRecordable(ref gObj);
        }

        protected virtual void SendRecordable(ref GameObject gObj)
        {
            gameController.AddRecordable(ref gObj, dummyType);
        }

        protected virtual void InitRecordableState(RecordableState recordableState)
        {
            recordableState.position = transform.position;
            recordableState.rotation = transform.rotation.eulerAngles.z;
        }

        public RecordableState GetRecordableState()
        {
            RecordableState recordableState = new RecordableState();
            InitRecordableState(recordableState);
            return recordableState;
        }
    }
}




