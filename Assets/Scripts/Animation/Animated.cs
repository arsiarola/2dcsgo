using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    /* The Animated class' job is to handle stuff that is related to animations/animator. Every object that
     * possesses an animator component should contain this Animated script or any of its child classes. */

    public class Animated : MonoBehaviour
    {
        protected Animator animator;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            animator.keepAnimatorControllerStateOnDisable = true;   // the animator state is saved on object/animator disable, so that we can continue from that state when the object/animator is enabled again
        }
    }
}


