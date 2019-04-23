using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Planning : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        private GameController GameController { get { return gameController; } }

        private Dictionary<int, Recordable.RecordableState> LastFrame { get; set; }
        private List<GameObject> objects;
        private bool plan;

        public void Plan()
        {
            // init variables
            plan = true;
            objects = new List<GameObject>();
            LastFrame = GameController.Frames[GameController.Frames.Count - 1];
            CreateObjects();
            Time.timeScale = 0;
        }

        private void Update()
        {
            if (plan) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    plan = false;
                    DestroyObjects();
                    Time.timeScale = 1f;
                    gameController.Flag = GameFlag.PlanningEnd;
                }
            }
        }

        private void CreateObjects()
        {
            foreach (KeyValuePair<int, Recordable.RecordableState> pair in LastFrame) 
            {
                int id = pair.Key;
                Recordable.RecordableState state = pair.Value;
                GameObject obj;

                if (GameController.recordablePlanningTypes.ContainsKey(id)) {
                    obj = Instantiate(GameController.recordablePlanningTypes[id]);
                } else {
                    obj = Instantiate(GameController.recordableReplayTypes[id]);
                }

                obj.transform.position = state.position;
                obj.transform.rotation = new Quaternion(0, 0, state.rotation, 0);

                foreach (Recordable.AnimationState anim in state.animations) {
                    Animator animator = obj.GetComponent<Animator>();
                    obj.GetComponent<Animator>().Play(anim.StateHash, anim.Layer, anim.Time);
                }

                objects.Add(obj);
            }
        }

        private void DestroyObjects()
        {
            foreach (GameObject obj in objects) {
                Destroy(obj);
            }
        }
    }
}