using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TOperatorAI : OperatorAI
    {
        public override Core.Side Side { get; protected set; } = Core.Side.Terrorist;
    }
}