using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    /// <summary>
    /// This works for frame updated animators only. Use the physics updated version if your animator updates in the physics cycle.
    /// </summary>
    public class SimpleAnimationFrame : SimpleAnimation
    {
        /// <summary>
        /// Destroy the object if the animation has played
        /// </summary>
        private void Update()
        {
            DestroyOnAnimationEnd();
        }
    }
}
