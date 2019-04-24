using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    /// <summary>
    /// Inherits recordable class and extends it to include the Animator component.
    /// </summary>
    /// <remarks>
    /// protected animator = getComponent(animator)
    /// overrides InitRecordableState() to allow the addition of animationstates.
    /// </remarks>
    public class AnimatedRecordable : Recordable
    {
        protected Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }
        /// <summary>
        /// Base method + adds the animations to the param recordableState.
        /// </summary>
        /// <param name="recordableState">RecordableState which needs AnimationStates added to it.</param>
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
