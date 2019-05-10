using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operator {
    public class TOperatorState : OperatorState {
        public bool HasBomb { get; private set; } = false;

        public void SetBomb(bool b)
        {
            if (b) {
                GetComponent<Animator>().SetBool("Bomb", true);
                HasBomb = true;
            } else {
                GetComponent<Animator>().SetBool("Bomb", false);
                HasBomb = false;
            }
        }
    }
}