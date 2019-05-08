using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class OperatorRecordable : AnimatedRecordable
    {
        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.BaseAI>();
            state.AddProperty<RecordableState.OperatorState>();
        }
    }
}


