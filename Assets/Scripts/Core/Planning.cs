using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Handles the planning stage
    /// </summary>
    public class Planning : MonoBehaviour
    {
        [SerializeField] private GameController gameController;

        /// <summary> Reference to the GameController Script </summary>
        private GameController GameController { get { return gameController; } }

        /// <summary> Contains the state of every recordable in the last frame </summary>
        private Dictionary<int, Recordable.RecordableState> LastFrame { get; set; }

        /// <summary> Id-GameObject dictionary of every object created for the planning stage </summary>
        private Dictionary<int, GameObject> PlanningRefs { get; set; }

        /// <summary>
        /// Start the planning process.
        /// </summary>
        public void Plan()
        {
            gameObject.SetActive(true); // activating the Planning object also activates this scripts Update method
            InitVariables();
            CreatePlanningObjects();    // create objects for the planning stage
            Time.timeScale = 0; // pause time for the duration of the planning
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
            foreach (KeyValuePair<int, Recordable.RecordableState> pair in LastFrame) 
            {
                int id = pair.Key;
                Recordable.RecordableState state = pair.Value;

                // create the object
                GameObject obj;
                if (GameController.recordablePlanningTypes.ContainsKey(id)) {   // has planning type
                    obj = Instantiate(GameController.recordablePlanningTypes[id]);  // create planning type
                }
                else {  // no planning type
                    obj = Instantiate(GameController.recordableReplayTypes[id]);    // create replay type
                }

                // set rotation and position for object
                obj.transform.position = state.position;
                obj.transform.eulerAngles = new Vector3(0, 0, state.rotation);

                // set animations
                foreach (Recordable.AnimationState anim in state.animations) {
                    Animator animator = obj.GetComponent<Animator>();
                    obj.GetComponent<Animator>().Play(anim.StateHash, anim.Layer, anim.Time);
                }

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
            Time.timeScale = 1f;    // set time scale to normal
            gameController.Flag = GameFlag.PlanningEnd;
            gameObject.SetActive(false);    // stops the update loop for this script
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
                    gameController.recordableRefs[id].GetComponent<FollowPath>().SetMousePositionList(list); // send it to the recordable
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