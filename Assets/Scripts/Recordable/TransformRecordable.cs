using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Recordable
{
    /// <summary>
    /// Inherits recordable and extends it to include the position and rotation
    /// </summary>
    public class TransformRecordable : Recordable
    {
        /// <summary>
        /// Base method + adds the transform component to the param recordableState.
        /// </summary>
        /// <param name="state">RecordableState which needs Transform data added to it.</param>
        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.Transform>();
        }
    }
}


