using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordableState
{
    /// <summary>
    /// Contains the state of a single animation layer
    /// </summary>
    public class AnimationState
    {
        /// <summary>The current state's name as a hash. </summary>
        /// <remarks>We cannot use string because we can only get the hash from the animator.</remarks>
        public int StateNameHash { get; }

        /// <summary>The animation layer as an integer</summary>
        public int Layer { get; }

        /// <summary>The current state's animation stage from [0, 1]. 0 is animation start, 1 is animation end.</summary>
        public float Stage { get; }

        /// <summary>
        /// Creates the AnimationState of a single layer. Gets the animatorStateInfo of a given layer and initializes variables in accordance to that information
        /// </summary>
        /// <param name="animator">Reference to the animator of the object</param>
        /// <param name="layer">The animation layer</param>
        public AnimationState(Animator animator, int layer)
        {
            AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            StateNameHash = animatorStateInfo.fullPathHash;     // fullPathHash is the preferred hash
            Layer = layer;
            Stage = animatorStateInfo.normalizedTime % 1;    // get the remainder when divided by one. This gives the stage without the amount times the animation has been played
        }
    }
}
