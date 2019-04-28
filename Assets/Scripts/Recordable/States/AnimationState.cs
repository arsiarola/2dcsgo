using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    /// <summary>
    /// Contains the state of a single animation layer of an object
    /// </summary>
    public class AnimationState
    {
        /// <summary>The current state's name as hash. We use hash because we can't use the string when playing the wanted animation.</summary>
        public int StateHash { get; }

        /// <summary>The animation layer as an integer</summary>
        public int Layer { get; }

        /// <summary>The current state's animation stage from [0, 1]. 0 is animation start, 1 is animation end.</summary>
        public float Time { get; }

        /// <summary>
        /// Creates an AnimationState. Gets the stateinfo of a given layer and initializes variables in accordance to that information
        /// </summary>
        /// <param name="animator">Reference to the animator of the object</param>
        /// <param name="layer">The animation layer</param>
        public AnimationState(Animator animator, int layer)
        {
            AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(layer);
            StateHash = animatorState.fullPathHash;     // get the state name hash (fullPathHash is the current standard)
            Layer = layer;
            Time = animatorState.normalizedTime % 1;    // get the modulus when divided by one. This gives the stage without the amount times the animation has been played
        }
    }
}