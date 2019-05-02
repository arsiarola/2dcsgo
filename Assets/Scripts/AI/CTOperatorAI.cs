using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CTOperatorAI : OperatorAI
    {
        public override Side Side { get; protected set; } = Side.CounterTerrorist;
    }
}

