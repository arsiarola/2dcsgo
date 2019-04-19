using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class Operator : RigidAnimated
    {
        public GameObject test;
        [SerializeField] protected GameObject planningType;
        int count = 0;

        protected override void SendRecordable(ref GameObject gObj)
        {
            gameController.AddRecordable(ref gObj, dummyType, planningType);
        }

        protected void FixedUpdate()
        {
            if (Globals.startCounting) {
                count++;
                Debug.Log("Other Fixed: " + count);
                if (count == 245) {
                    Instantiate(test);
                }
            }
        }
    }
}


