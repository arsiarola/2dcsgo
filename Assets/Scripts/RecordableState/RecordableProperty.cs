using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordableState
{
    public abstract class RecordableProperty
    {

    }

    public class Position : RecordableProperty
    {
        public Vector3 pos;
        public Position()
        {
            this.pos = new Vector3(0, 0, 0);
        }
        public Position(Vector3 pos)
        {
            this.pos = pos;
        }
    }

    public class Velocity : RecordableProperty
    {
        public Vector3 velocity;
        public Velocity()
        {
            this.velocity = new Vector3(0, 0, 0);
        }
        public Velocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }
    }
}
