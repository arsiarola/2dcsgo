using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class RigidAnimated : Animated
    {
        protected override void InitRecordableState(RecordableState recordableState)
        {
            base.InitRecordableState(recordableState);
            recordableState.velocity = GetComponent<Rigidbody2D>().velocity;
        }
    }
}

