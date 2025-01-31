﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    /// <summary>
    ///     Inherits transform recordable class and extends it to include the Animator component.
    /// </summary>
    /// <remarks>
    ///      protected animator = getComponent(animator)
    ///      overrides InitRecordableState() to allow the addition of animationstates.
    /// </remarks>
    public class AnimatedRecordable : TransformRecordable
    {
        /// <summary>
        /// Base method + adds the animations to the param recordableState.
        /// </summary>
        /// <param name="state">RecordableState which needs AnimationStates added to it.</param>
        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.Animations>();
        }
    }
}
