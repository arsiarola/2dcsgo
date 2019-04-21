using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class AnimationState
    {
        public int StateHash { get; }
        public int Layer { get; }
        public float Time { get; }

        public AnimationState(Animator animator, int layer)
        {
            AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(layer);
            StateHash = animatorState.fullPathHash;
            Layer = layer;
            Time = animatorState.normalizedTime % 1;
        }
    }
}