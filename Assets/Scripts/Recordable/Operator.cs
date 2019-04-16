using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public enum Side
    {
        T,
        CT
    }

    public abstract class Operator : RigidAnimated
    {
        public Side side; 
        [SerializeField] protected GameObject ordersType;

        protected override int GetObjectId(ref GameObject gObj)
        {
            return gameController.GetObjectId(ref gObj, dummyType, ordersType);

        }
    }
}


