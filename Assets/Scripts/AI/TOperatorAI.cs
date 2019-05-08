using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TOperatorAI : OperatorAI
    {
        public override Core.Side Side { get; protected set; } = Core.Side.Terrorist;

        private void PlantBomb() {
            RaycastHit2D Hit = Physics2D.CircleCast(transform.position, 0.3f, transform.position, 1 << 9);
            Debug.Log(Hit);
            if(Target != null && Hit.collider != null) {

            }
        }
    }
}