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
        public Dictionary<int, RecordableState.RecordableState> LastFrame { get; set; }

        /// <summary> Id-GameObject dictionary of every object created for the planning stage </summary>
        private Dictionary<int, GameObject> PlanningRefs { get; set; }

        public float CurrentTime { get; set; } = 0;

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
            CurrentTime = 60 - (GameController.Frames.Count - 1) * Time.fixedDeltaTime;
        }

        /// <summary>
        /// Create a planning object, that is of either planningType or replayType, of every recordable in the last frame
        /// </summary>
        private void CreatePlanningObjects()
        {
            int AIid = GameController.SideAIs[GameController.Side];
            List<GameObject> visibleEnemies = LastFrame[AIid].GetProperty<RecordableState.ExtendedAI>().VisibleEnemies;
            Dictionary<GameObject, Vector3> lastPosition = LastFrame[AIid].GetProperty<RecordableState.ExtendedAI>().LastEnemyPositions;

            // go through every id-state pair in the last frame
            foreach (KeyValuePair<int, RecordableState.RecordableState> pair in LastFrame) 
            {
                int id = pair.Key;
                RecordableState.RecordableState state = pair.Value;

                bool hasPlanningType = GameController.RecordablePlanningTypes.ContainsKey(id);
                bool hasReplayType = GameController.RecordableReplayTypes.ContainsKey(id);

                // create the object
                if (hasPlanningType || hasReplayType) {
                    GameObject obj = null;
                    if (state.GetProperty<RecordableState.BaseAI>() != null) {
                        Side side = state.GetProperty<RecordableState.BaseAI>().Side;
                        bool isVisible = visibleEnemies.Contains(GameController.RecordableRefs[id]);
                        RecordableState.OperatorState operatorState = state.GetProperty<RecordableState.OperatorState>();
                        if (side == GameController.Side && (operatorState == null || operatorState.IsAlive)) {   // has planning type
                            if (hasPlanningType) {
                                obj = Instantiate(GameController.RecordablePlanningTypes[id]);  // create planning type
                                if (obj.GetComponent<MakePath>() != null) {
                                    AI.OperatorAI operatorAI = GameController.RecordableRefs[id].GetComponent<AI.OperatorAI>();
                                    if (0 < operatorAI.Path.Count) {
                                        operatorAI.Path.RemoveRange(0, operatorAI.NextPointInPath);
                                    }
                                    obj.GetComponent<MakePath>().mousePositionList = operatorAI.Path;
                                    obj.GetComponent<MakePath>().DrawPath();
                                }
                            } else {
                                obj = Instantiate(GameController.RecordableReplayTypes[id]);  // create replay type
                            }
                        }
                        else if (side != GameController.Side) {
                            if (isVisible) {
                                obj = Instantiate(GameController.RecordableReplayTypes[id]);    // create replay type
                            }
                            else if (!isVisible && lastPosition.ContainsKey(GameController.RecordableRefs[id])) {
                                PlanningRefs.Add(id, Instantiate(GameController.Unknown, lastPosition[GameController.RecordableRefs[id]], Quaternion.identity));
                            }
                        }
                    }
                    else {
                        obj = Instantiate(GameController.RecordableReplayTypes[id]);
                    }

                    if (obj != null) {
                        state.SetToObject(obj);
                        PlanningRefs.Add(id, obj);  // add a reference of the created object to the planning refs dictionary
                    }            
                }
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
            SendRotation();
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
                    GameController.RecordableRefs[id].GetComponent<AI.OperatorAI>().SetPath(list); // send it to the recordable
                }
            }
        }

        private void SendRotation()
        {
            foreach (KeyValuePair<int, GameObject> pair in PlanningRefs) {
                int id = pair.Key;
                GameObject obj = pair.Value;
                if (obj.GetComponent<MakePath>() != null) { // if obj has makePath script
                    Quaternion q = obj.transform.rotation;    // get rotation
                    GameController.RecordableRefs[id].GetComponent<AI.OperatorAI>().SetRotation(q); // send it to the recordable
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