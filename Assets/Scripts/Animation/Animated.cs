using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    /// <summary>
    /// Handles stuff that is related to the animations/animator. Every object that possesses an animator component should contain this script or any of its child classes.
    /// </summary>
    public class Animated : MonoBehaviour
    {
        /// <summary> Reference to animator component of the object</summary>
        protected Animator Animator { get; set; }

        /// <summary>
        /// Gets the animator component reference and sets the animator so that it keeps it's state on disable.
        /// </summary>
        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Animator.keepAnimatorControllerStateOnDisable = true;   // the animator state is saved on object/animator disable, so that we can continue from that state when the object/animator is enabled again
        }
    }
}

