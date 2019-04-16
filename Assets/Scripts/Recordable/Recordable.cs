using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class RecordableState
    {
        public Vector3 position;
        public float rotation;
        public Vector2 velocity;
        public List<string> animTriggers;

        public RecordableState()
        {
            position = new Vector3(0, 0, 0);
            rotation = 0;
            velocity = new Vector2(0, 0);
            animTriggers = new List<string>();

        }
    }

    public abstract class Recordable : MonoBehaviour
    {
        private int objectId;
        [SerializeField] protected Core.GameController gameController;
        [SerializeField] protected GameObject dummyType;

        protected virtual void Awake()
        {
            if (gameController == null)
            {
                gameController = GameObject.Find(Misc.Constants.GameControllerName).GetComponent<Core.GameController>();
            }
            GameObject gObj = gameObject;
            objectId = GetObjectId(ref gObj);
            //objectId = gameController.GetObjectId(ref gObj, dummyType, ordersType);
        }
        protected virtual int GetObjectId(ref GameObject gObj)
        {
            return gameController.GetObjectId(ref gObj, dummyType);

        }

        protected virtual void InitRecordableState(RecordableState recordableState)
        {
            recordableState.position = transform.position;
            recordableState.rotation = transform.rotation.eulerAngles.z;
        }

        protected virtual void LateUpdate()
        {
            RecordableState recordableState = new RecordableState();
            InitRecordableState(recordableState);
//            gameController.FrameAddRecordableState(objectId, recordableState);
        }
    }

    public abstract class Animated : Recordable
    {
        protected Animator animator;
        private List<string> animTriggers = new List<string>();

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
        }

        protected override void InitRecordableState(RecordableState recordableState)
        {
            base.InitRecordableState(recordableState);
            recordableState.animTriggers = new List<string>(animTriggers);
            animTriggers.Clear();
        }

        protected void ActivateAnimTrigger(string trigger)
        {
            animTriggers.Add(trigger);
            animator.SetTrigger(trigger);
        }
    }

    public abstract class RigidAnimated : Animated
    {
        protected override void InitRecordableState(RecordableState recordableState)
        {
            base.InitRecordableState(recordableState);
            recordableState.velocity = GetComponent<Rigidbody2D>().velocity;
        }
    }

    public abstract class SimpleAnimation : Animated
    {
        [SerializeField] private string animationName;

        protected virtual void Update()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                Destroy(gameObject);
            }
        }
    }
}




