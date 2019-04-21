using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    /* The task of the Recordable class and its child classes is to communicate with the GameController/
     * Simulator/Recorder. Initially it sends the gameObject's reference and possible replayType and/or
     * planningType. During the simulation/recording process and at the end of every FixedUpdate cycle, 
     * the Recorder calls some function(s) from this script to get a recordable object's state for that
     * cycle/frame. Even though the recordable gameObject may have many scripts, everything that is recorded
     * must be called by the recorder from this class or it's childs (depending on what type of recordable is in
     * question). This means that, for example, an instance variable that is saved in a different script, but 
     * should be recorded every frame, must be called by the Recordable script when it is asked to give the object's
     * state to the Recorder.*/

    public class Recordable : MonoBehaviour
    {
        [SerializeField] protected Core.GameController gameController;
        [SerializeField] protected GameObject replayType;
        [SerializeField] protected GameObject planningType;

        protected virtual void Awake()
        {
            if (gameController == null) // find the gameController if it isn't initialized using the unity inspector
            {
                gameController = GameObject.Find(Misc.Constants.GameControllerName).GetComponent<Core.GameController>();
            }
            GameObject gObj = gameObject;   // the gameObject property must be put to a variable so that we can send a reference of the object
            gameController.AddRecordable(ref gObj, replayType, planningType);
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




