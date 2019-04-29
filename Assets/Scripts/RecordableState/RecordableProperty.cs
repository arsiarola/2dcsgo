using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordableState
{
    interface IRecordableProperty<T>
    {
        T GetValue();
    }

    public abstract class RecordableProperty
    {

    }

    public class Position : RecordableProperty, IRecordableProperty<Vector3>
    {
        Vector3 pos;
        public Position(Vector3 pos)
        {
            this.pos = pos;
        }

        public Vector3 GetValue()
        {
            return pos;
        }
    }
}
