using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class Recordable : MonoBehaviour
    {
        protected int recordingId;
        public Recorder recorder;
        public GameObject dummyType;

        protected virtual void Start()
        {
            GameObject gObj = gameObject;
            recordingId = recorder.GetObjectKey(dummyType, ref gObj);
        }
    }

    public class AnimatedRecordable : Recordable
    {
        protected Animator animator;

        protected override void Start()
        {
            base.Start();
            animator = GetComponent<Animator>();
            GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
        }

    }
}


