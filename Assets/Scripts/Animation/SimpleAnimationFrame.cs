using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    /* This works for frame updated animators only. Use the physics updated version if your animator updates
     * in the physics cycle. */

    public class SimpleAnimationFrame : SimpleAnimation
    {
        private void Update()
        {
            DestroyOnAnimationEnd();
        }
    }
}
