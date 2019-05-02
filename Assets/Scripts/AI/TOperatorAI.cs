using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TOperatorAI : OperatorAI
    {
        public override Side Side { get; protected set; } = Side.Terrorist;
    }
}