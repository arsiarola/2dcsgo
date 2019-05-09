using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class TOperatorAI : OperatorAI {
        public override Core.Side Side { get; protected set; } = Core.Side.Terrorist;
        private float distanceToBombA;
        private float distanceToBombB;

        public void PlantBomb(Vector3 pos) {
            distanceToBombA = Vector3.Distance(pos, new Vector3(42.5f, -25f, 0f));
            distanceToBombB = Vector3.Distance(pos, new Vector3(-3.5f, 40f, 0f));
            if (Target == null && distanceToBombA < 1f) {
                Debug.Log("Planting bomb to A");
            }

            else if (Target == null && distanceToBombB < 1f) {
                Debug.Log("Planting bomb to B");
            }
        }
    }
}


