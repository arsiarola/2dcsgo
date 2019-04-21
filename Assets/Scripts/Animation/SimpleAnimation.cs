using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    /* An object that plays a single animation and then disappears. Use either the frame updated or physics
     * updated child class. This is just an abstract class. */

    public abstract class SimpleAnimation : Animated
    {
        [SerializeField] private string animationName;  // the name of the animation state

        protected void DestroyOnAnimationEnd()  // destroy the object once the animation has played
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                Destroy(gameObject);
            }
        }
    }
}

