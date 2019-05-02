using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Handles the planning stage
    /// </summary>
    public class Planning : Task
    {
        /// <summary> Contains the state of every recordable in the last frame </summary>
        private Dictionary<int, RecordableState.RecordableState> LastFrame { get; set; }

        /// <summary> Id-GameObject dictionary of every object created for the planning stage </summary>
        private Dictionary<int, GameObject> PlanningRefs { get; set; }

        /// <summary>
        /// Start the planning process.
        /// </summary>
        private void OnEnable()
        {
            InitVariables();
            CreatePlanningObjects();    // create objects for the planning stage
        }

        /// <summary>
        /// Sets all variables to their default values
        /// </summary>
        private void InitVariables()
        {
            LastFrame = GameController.Frames[GameController.Frames.Count - 1]; // get the last frame from the gameControllers list of frames
            PlanningRefs = new Dictionary<int, GameObject>();   // start adding planning objects to an empty container
        }

        /// <summary>
        /// Create a planning object, that is of either planningType or replayType, of every recordable in the last frame
        /// </summary>
        private void CreatePlanningObjects()
        {
            // go through every id-state pair in the last frame
            foreach (KeyValuePair<int, RecordableState.RecordableState> pair in LastFrame) 
            {
                int id = pair.Key;
                RecordableState.RecordableState state = pair.Value;

                // create the object
                GameObject obj;
                if (GameController.RecordablePlanningTypes.ContainsKey(id)) {   // has planning type
                    obj = Instantiate(GameController.RecordablePlanningTypes[id]);  // create planning type
                }
                else {  // no planning type
                    obj = Instantiate(GameController.RecordableReplayTypes[id]);    // create replay type
                }

                state.SetToObject(obj);

                PlanningRefs.Add(id, obj);  // add a reference of the created object to the planning refs dictionary
            }
        }

        /// <summary>
        /// Check if space is pressed and end planning phase if true.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Exit();
            }
        }

        /// <summary>
        /// Sends data to the recordables, resets timescale, disables destroys planning objects, sends exit flag to gameController and etc.
        /// </summary>
        private void Exit()
        {
            SendDataToRecordables();
            DestroyPlanningObjects();
            GameController.SendMessage(GameMessage.PlanningEnd);
        }

        /// <summary>
        /// Send all data from planning objects to the recordables
        /// </summary>
        private void SendDataToRecordables()
        {
            SendPath();
        }

        /// <summary>
        /// If the planning object has a make path script, send the path to the recordable with the same id
        /// </summary>
        private void SendPath()
        {
            foreach (KeyValuePair<int, GameObject> pair in PlanningRefs) {
                int id = pair.Key;
                GameObject obj = pair.Value;
                if (obj.GetComponent<MakePath>() != null) { // if obj has makePath script
                    List<Vector3> list = obj.GetComponent<MakePath>().mousePositionList;    // get path list
                    GameController.RecordableRefs[id].GetComponent<FollowPath>().SetMousePositionList(list); // send it to the recordable
                }
            }
        }
        
        /// <summary>
        /// Destroys the planning objects
        /// </summary>
        private void DestroyPlanningObjects()
        {
            foreach (KeyValuePair<int, GameObject> pair in PlanningRefs) {
                GameObject obj = pair.Value;
                Destroy(obj);
            }
        }
    }
}