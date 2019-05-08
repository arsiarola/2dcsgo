using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class TOperatorAI : OperatorAI {
        public override Core.Side Side { get; protected set; } = Core.Side.Terrorist;

        public void PlantBomb() {
            bool Collider = Physics2D.OverlapCircle(transform.position, 0.3f, 1 << 9);
            if (Target == null && Collider)
                Debug.Log(Collider);
        }
    }
}  
