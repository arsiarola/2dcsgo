using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class AIRecordable : Recordable
    {
        public override Type Type { get; protected set; } = Type.AI;

        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.SideAI>();
        }
    }
}

