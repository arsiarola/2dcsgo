using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class Animated : Recordable
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
}
