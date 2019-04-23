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
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("Awake: " + gameObject.transform.position); // TESTING
        }

        private void Start()
        {
            Debug.Log("Start: " + gameObject.transform.position); // TESTING
        }

        private void FixedUpdate()
        { 
            Debug.Log("FixedUpdate: " + gameObject.transform.position);   // TESTING
            DestroyOnAnimationEnd();
        }
    }
}


