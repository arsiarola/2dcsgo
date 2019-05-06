using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operator
{
    public class OperatorState : MonoBehaviour
    {
        public float Hp { get; private set; } = 100;
        public GameObject DeathAnimation { get { return deathAnimation; } }
        [SerializeField] private GameObject deathAnimation;

        public bool IsAlive()
        {
            return 0 < Hp;
        }

        public void Kill()
        {
            Hp = 0;
        }
    }
}

