using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class RecordableState
    {
        public Vector3 position;
        public float rotation;
        public Vector2 velocity;
        public List<string> animTriggers;

        public RecordableState()
        {
            position = new Vector3(0, 0, 0);
            rotation = 0;
            velocity = new Vector2(0, 0);
            animTriggers = new List<string>();

        }
    }
}

