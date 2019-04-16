using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class Terrorist : Operator
    {
        protected override void Awake()
        {
            base.Awake();
            side = Side.T;
        }
    }
}
