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
        /// <summary>The position</summary>
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
        
        /// <summary>The Z-Axis rotation</summary>
        public float Rotation { get; set; } = 0;

        /// <summary>The Velocity</summary>
        public Vector2 Velocity { get; set; } = new Vector2(0, 0);

        /// <summary>The state of each animation layer</summary>
        public List<AnimationState> AnimationLayers { get; set; } = new List<AnimationState>();

        /// <summary>
        /// Create a new recordable state. All values are default
        /// </summary>
        public RecordableState()
        {
        }
    }
}

