using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class AnimatedRecordable : Recordable
    {
        protected Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected override void InitRecordableState(RecordableState recordableState)
        {
            base.InitRecordableState(recordableState);
            for (int i = 0; i < animator.layerCount; i++)
            {
                recordableState.animations.Add(new AnimationState(animator, i));
            }
        }
    }
}
