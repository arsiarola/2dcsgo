using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class BombRecordable : AnimatedRecordable
    {
        protected override Type Type { get; set; } = Type.Bomb;

        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
        }
    }
}
