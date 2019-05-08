using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TOperatorAI : OperatorAI
    {
        public override Core.Side Side { get; protected set; } = Core.Side.Terrorist;

        public void PlantBomb() {
            Collider2D [] Collider = Physics2D.OverlapCircleAll(transform.position, 0.3f, 1 << 9);
            if(Target != null && Collider.Length > 0) {
                Debug.Log(Collider);
            }
        }
    }
}