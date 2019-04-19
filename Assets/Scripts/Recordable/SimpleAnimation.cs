using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class SimpleAnimation : Animated
    {
        [SerializeField] private string animationName;

        protected virtual void Update()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                Destroy(gameObject);
            }


        }
    }
}

