using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    /// <summary>
    /// Inherits animated recordable class and extends it to include the Rigidbody2D component.
    /// </summary>
    public class DynamicRecordable : AnimatedRecordable
    {
        /// <summary>
        /// Base method + adds the velocity to the recordableState
        /// </summary>
        /// <param name="state">RecordableState which needs velocity added to it</param>
        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.Dynamics>();
        }
    }
}

