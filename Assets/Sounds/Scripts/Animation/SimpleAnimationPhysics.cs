using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    /// <summary>
    /// This is a PHYSICS UPDATED single animation object. If your animator isn't physics updated, but updated every frame instead, use the frame updated version
    /// </summary>
    public class SimpleAnimationPhysics : SimpleAnimation
    {
        /// <summary>
        /// Destroy the object if the animation has played
        /// </summary>
        private void FixedUpdate()
        { 
            DestroyOnAnimationEnd();
        }
    }
}


