using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class CounterTerrorist : Operator
    {
        protected override void Awake()
        {
            base.Awake();
            side = Side.CT;
        }
        
    }
}