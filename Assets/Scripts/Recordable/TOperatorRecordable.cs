using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class TOperatorRecordable : OperatorRecordable
    {
        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.Bomb>();
        }
    }
}

