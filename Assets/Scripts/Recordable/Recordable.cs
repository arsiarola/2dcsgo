using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    /// <summary>
    ///     Communicates with GameController/Simulator/Recorder.
    /// </summary>
    /// <remarks>
    ///     Initially it sends the gameObject's reference and possible replayType and/or
    ///     planningType. During the simulation/recording process and at the end of every FixedUpdate cycle, 
    ///     the Recorder calls some function(s) from this script to get a recordable object's state for that
    ///     cycle/frame. Even though the recordable gameObject may have many scripts, everything that is recorded
    ///     must be called by the recorder from this class or it's childs (depending on what type of recordable is in
    ///     question). This means that, for example, an instance variable that is saved in a different script, but 
    ///     should be recorded every frame, must be called by the Recordable script when it is asked to give the object's
    ///     state to the Recorder.
    /// </remarks>
    public class Recordable : MonoBehaviour
    {
        /// <summary>Reference to the GameController script of the GameController object</summary>
        protected Core.GameController GameController { get { return gameController; } set { gameController = value; } }
        [SerializeField] private Core.GameController gameController;

        /// <summary>Reference to the recordable's replay type prefab</summary>
        protected GameObject ReplayType { get { return replayType; } set { replayType = value; } }
        [SerializeField] private GameObject replayType;

        /// <summary>Reference to the recordable's planning type prefab</summary>
        protected GameObject PlanningType { get { return planningType; } set { planningType = value; } }
        [SerializeField] private GameObject planningType;

        /// <summary>
        /// Add a reference of the Recordable to the GameController
        /// </summary>
        protected virtual void Awake()
        {
            if (GameController == null) // find the gameController if it isn't initialized using the unity inspector
            {
                GameController = GameObject.Find(Misc.Constants.GAME_CONTROLLER_NAME).GetComponent<Core.GameController>();
            }
            GameObject gObj = gameObject;   // the gameObject property must be put to a variable so that we can send a reference of the object
            GameController.AddRecordable(ref gObj, ReplayType, PlanningType);
        }

        /// <summary>
        /// Initializes the recordable state variables
        /// </summary>
        /// <param name="state">Reference of the state we are modifying</param>
        protected virtual void AddProperties(RecordableState.RecordableState state)
        {
            
        }

        /// <summary>
        /// Get the recordable's current state
        /// </summary>
        /// <returns>Returns the recordable's state</returns>
        public RecordableState.RecordableState GetState()
        {
            RecordableState.RecordableState state = new RecordableState.RecordableState(gameObject);
            AddProperties(state);
            return state;
        }
    }
}




