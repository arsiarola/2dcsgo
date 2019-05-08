using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class AIRecordable : Recordable
    {
        protected override Type Type { get; set; } = Type.AI;

        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.BaseAI>();
        }
    }
}

