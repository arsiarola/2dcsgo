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
        public Dictionary<int, GameObject> objects;

        public void Plan()
        {
            // init variables
            gameObject.SetActive(true);
            objects = new Dictionary<int, GameObject>();
            LastFrame = GameController.Frames[GameController.Frames.Count - 1];
            CreateObjects();
            Time.timeScale = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                gameObject.SetActive(false);
                foreach (KeyValuePair<int, GameObject> pair in objects)
                {
                    int id = pair.Key;
                    GameObject obj = pair.Value;
                    if (obj.GetComponent<MakePath>() != null)
                    {
                        List<Vector3> list = obj.GetComponent<MakePath>().mousePositionList;
                        gameController.recordableRefs[id].GetComponent<FollowPath>().SetMousePositionList(list);
                    }
                }
                DestroyObjects();
                Time.timeScale = 1f;
                gameController.Flag = GameFlag.PlanningEnd;
                    
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
                obj.transform.eulerAngles = new Vector3(0, 0, state.rotation);

                foreach (Recordable.AnimationState anim in state.animations) {
                    Animator animator = obj.GetComponent<Animator>();
                    obj.GetComponent<Animator>().Play(anim.StateHash, anim.Layer, anim.Time);
                }

                objects.Add(id, obj);
            }
        }

        private void DestroyObjects()
        {
            foreach (KeyValuePair<int, GameObject> pair in objects) {
                GameObject obj = pair.Value;
                Destroy(obj);
            }
        }
    }
}