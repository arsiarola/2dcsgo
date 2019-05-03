using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// The stage of the game
    /// </summary>
    public enum GameStage
    {
        Record,
        Replay,
        Planning
    }

    /// <summary>
    /// Flag that represents the end of a stage
    /// </summary>
    public enum GameMessage
    {
        RecordEnd,
        ReplayEnd,
        PlanningEnd
    }

    /// <summary>
    /// Controls the game stage as well as key collections and variables
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary> Reference to the Recorder script of a Recorder object</summary>
        private Recorder Recorder { get { return recorder; } }
        [SerializeField] private Recorder recorder;

        /// <summary> Reference to the Replayer script of a Replayer object</summary>
        private Replayer Replayer { get { return replayer; } }
        [SerializeField] private Replayer replayer;

        /// <summary> Reference to the Planning script of a Planning object</summary>
        private Planning Planning { get { return planning; } }
        [SerializeField] private Planning planning;

        /// <summary> This script handles the simulation loop</summary>
        private Simulation Simulation { get { return simulation; } set { simulation = Simulation; } }
        [SerializeField] private Simulation simulation;

        public Side Side { get { return side; } private set { side = value; IsSideChanged = true; } }
        private Side side = Side.CounterTerrorist;

        private bool IsSideChanged { get; set; } = true;

        public int Turn { get; private set; } = -1;

        public int Planned { get; private set; } = 0;

        public int Replays { get; private set; } = 0;

        /// <summary> The current game stage. If the value is changed, IsStateChanged is also set to true</summary>
        public GameStage Stage { get { return stage; } private set { stage = value; IsStageChanged = true; } }
        private GameStage stage = GameStage.Planning;

        private bool IsStageChanged { get; set; } = true;   // stage has been changed because we go to the first stage

        /// <summary> What is the next available id for recordables </summary>
        private int NextId { get; set; } = 0;   // start from zero

        /// <summary> List of frames with each frame containing a dictionary of type Id-State which specifies every recordable's state at that frame. The time between consecutive frames is determined by the fixed timestep</summary>
        public List<Dictionary<int, RecordableState.RecordableState>> Frames { get; private set; } = new List<Dictionary<int, RecordableState.RecordableState>>();

        /// <summary> Contains the references to every recordable. These are contained in a dictionary of type id-recordable. Value is the reference to a recordable while the key is it's id. </summary>
        public Dictionary<int, GameObject> RecordableRefs { get; private set; } = new Dictionary<int, GameObject>();

        /// <summary> Contains the reference to every recordable's planning type prefab (if it has one). The planning type of a given recordable can be accessed with it's id. The id is this id-planningType dictionary's key. </summary>
        public Dictionary<int, GameObject> RecordablePlanningTypes { get; private set; } = new Dictionary<int, GameObject>();

        /// <summary> Contains the reference to every recordable's replay type prefab. The replay type of a given recordable can be accessed with it's id</summary>
        public Dictionary<int, GameObject> RecordableReplayTypes { get; private set; } = new Dictionary<int, GameObject>();

        

        /// <summary>
        /// Gets the starting frame from Recorder, disables every recordable, and disables the Recorder, the Replayer and the Planning objects
        /// </summary>
        /// <remarks>
        /// Before start every Recordable that has been put to scene will execute their Awake methods which send a reference of that object to the GameController
        /// </remarks>
        private void Start()
        {
            Frames.Add(Recorder.GetRecordableStates()); // get start frame. Not sure if necessary for the replay, but we do need to get the objects starting positions at least (then again these can be gained by other means)
            DisableRecordables();       // disable recordables before they can execute their FixedUpdate or update methods
        }

        /// <summary>
        /// Handles flags and stage changes
        /// </summary>
        private void Update()
        {
            if (IsSideChanged) {
                HandleSideChange();
            }

            if (IsStageChanged) {
                HandleStageChange();
            }
        }

        private void HandleSideChange()
        {
            IsSideChanged = false;
            Turn++;
        }

        /// <summary>
        /// Handles messages that have been sent to the GameController
        /// </summary>
        public void SendMessage(GameMessage message)
        {
            Time.timeScale = 1f;  // reset time scale to normal
            switch (message)
            {
                case GameMessage.RecordEnd:
                    DisableRecordables();    // disable recordables now that the simulating is complete
                    Frames.AddRange(Recorder.GetRecordedFrames());  // add the recorded frames to the frames list
                    Recorder.gameObject.SetActive(false);    // disable the Recorder gameObject for claritys sake
                    Simulation.gameObject.SetActive(false); // disables simulation
                    Stage = GameStage.Replay;   // after recording, the stage is replay
                    break;
                case GameMessage.ReplayEnd:
                    Replayer.gameObject.SetActive(false);
                    Stage = GameStage.Planning; // after replay the stage is planning
                    Replays++;
                    break;
                case GameMessage.PlanningEnd:
                    Planning.gameObject.SetActive(false);    // stops the update loop for this script
                    Planned++;
                    if (Planned == 2) {
                        Stage = GameStage.Record;   // after planning the stage is record
                        Planned = 0;
                    } else {
                        SwitchSide();
                        if (Turn == 0) {
                            Stage = GameStage.Planning;
                        } else {
                            Stage = GameStage.Replay;
                        }
                    }
                    break;
            }
        }

        private void SwitchSide()
        {
            switch (Side) {
                case Side.CounterTerrorist:
                    Side = Side.Terrorist;
                    break;
                case Side.Terrorist:
                    Side = Side.CounterTerrorist;
                    break;
            }
        }

        /// <summary>
        /// Handles a stage change
        /// </summary>
        private void HandleStageChange()
        {
            
            switch (Stage)
            {
                case GameStage.Record:
                    EnableRecordables(); // enable recordables for simulation
                    Time.timeScale = Misc.Constants.SIMULATION_SPEED;  // simulate as fast as possible
                    Recorder.gameObject.SetActive(true); // enables the Recorder object
                    Simulation.gameObject.SetActive(true);  // enables simulation
                    break;
                case GameStage.Replay:
                    Debug.Log("HERE");
                    Replayer.gameObject.SetActive(true); // activate the replayer object in order to activate the update of this script. Update is executed only if the script is enabled
                    break;
                case GameStage.Planning:
                    Time.timeScale = 0; // pause time for the duration of the planning
                    Planning.gameObject.SetActive(true); // activating the Planning object also activates this scripts Update method
                    break;
            }
            IsStageChanged = false; // stage change has been handled
        }

        

        /// <summary>
        /// Add a recordable's reference, replayType and planningType to the dictionaries. The new recordable is given its unique id
        /// </summary>
        /// <param name="reference"> Recordable object's reference </param>
        /// <param name="replayType"> Recordable's replayType prefab</param>
        /// <param name="planningType">Recordable's planningType prefab. Can be null</param>
        public void AddRecordable(ref GameObject reference, GameObject replayType, GameObject planningType)
        {
            RecordableRefs.Add(NextId, reference);
            RecordableReplayTypes.Add(NextId, replayType);
            if (planningType != null) RecordablePlanningTypes.Add(NextId, planningType);
            NextId++;   // increment NextId in preparation for the next recordable
        }

        
        /// <summary>
        /// Disables every alive recordable by first disabling it's children. Also disables the simulation object
        /// </summary>
        private void DisableRecordables()
        {
            foreach (KeyValuePair<int, GameObject> pair in RecordableRefs)
            {
                int id = pair.Key;
                GameObject obj = pair.Value;
                if (obj != null)
                {
                    DisableRecordableChildren(ref obj); // disable children first
                }
            }
            //Simulation.SetActive(false);    // disable simulation
        }

        /// <summary>
        /// Disables a recordable's children before disabling the recordable itself. This prevents some bugs
        /// </summary>
        /// <param name="obj">The Recordable/child to be disabled</param>
        private void DisableRecordableChildren(ref GameObject obj)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject child = obj.transform.GetChild(i).gameObject;
                DisableRecordableChildren(ref child);
            }
            obj.SetActive(false);
        }

        /// <summary>
        /// Enables every recordable that is alive, and enables the simulation object
        /// </summary>
        private void EnableRecordables()
        {
            foreach (KeyValuePair<int, GameObject> pair in RecordableRefs)
            {
                int id = pair.Key;
                GameObject obj = pair.Value;
                if (obj != null) {
                    obj.SetActive(true);
                    RecordableState.RecordableState lastState = Frames[Frames.Count - 1][id];
                    lastState.InitOwner();
                }
            }
            //Simulation.SetActive(true); // enable simulation
            //Init recordable state
        }
    }
}


