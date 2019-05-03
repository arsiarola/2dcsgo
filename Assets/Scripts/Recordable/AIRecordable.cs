using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class AIRecordable : Recordable
    {
        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.ArtificialIntelligence>();
        }
    }
}

