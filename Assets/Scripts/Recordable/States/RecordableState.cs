using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    /// <summary>
    /// Stores information about a game object during the recording phase. A RecordableState stores the state of a game object in a single frame.
    /// </summary>
    /// <remarks>
    /// Vector3 position, float rotation, Vector2 velocity, List(AnimationState) animations.
    /// </remarks>
    public class RecordableState
    {
        public Vector3 position;
        public float rotation;
        public Vector2 velocity;
        public List<AnimationState> animations;

        /// <summary>
        /// Constructor: zeroes on all the values and creates an empty List(AnimationState)
        /// </summary>
        public RecordableState()
        {
            position = new Vector3(0, 0, 0);
            rotation = 0;
            velocity = new Vector2(0, 0);
            animations = new List<AnimationState>();
        }
    }
}

