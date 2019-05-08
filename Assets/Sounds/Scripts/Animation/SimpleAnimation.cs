using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    /// <summary>
    /// Plays a single animation and then disappear. Use either the frame updated or physics updated child class because is just an abstract class
    /// </summary>
    public abstract class SimpleAnimation : Animated
    {
        /// <summary>
        /// The name of the animation state to be played
        /// </summary>
        private string AnimationName { get { return animationName; } set { animationName = value; } }
        [SerializeField] private string animationName; 
       

        /// <summary>
        /// Destroy the object once the animation has played
        /// </summary>
        protected void DestroyOnAnimationEnd()
        {
            if (!Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) // current animator state is not the one specified
            {
                Destroy(gameObject);
            }
        }
    }
}

