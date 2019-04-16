using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public abstract class Recordable : MonoBehaviour
    {
        private int recordingId;
        [SerializeField] private Recorder recorder;
        [SerializeField] private GameObject dummyType;

        protected virtual void Start()
        {
            if (recorder == null)
            {
                recorder = GameObject.Find(Misc.Constants.RecorderName).GetComponent<Recorder>();
            }
            GameObject gObj = gameObject;
            recordingId = recorder.GetObjectKey(dummyType, ref gObj);
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
            recorder.FrameAddRecordableState(recordingId, recordableState);
        }
    }

    public abstract class Animated : Recordable
    {
        protected Animator animator;
        private List<string> animTriggers = new List<string>();

        protected override void Start()
        {
            base.Start();
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
}




