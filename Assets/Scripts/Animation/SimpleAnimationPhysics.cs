using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    /* This is a PHYSICS UPDATED single animation object. This is not updated during the Update loop but instead
     * in the FixedUpdate loop. This is not guaranteed to work if the animator is NOT physics updated. If your
     * animator isn't physics updated, but updated every frame instead, use the frame updated version */

    public class SimpleAnimationPhysics : SimpleAnimation
    {
        private void FixedUpdate()
        {
            DestroyOnAnimationEnd();
        }
    }
}


