using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operator {
    public class OperatorState : MonoBehaviour {
        public float Hp { get; private set; } = 100;
        public GameObject DeathAnimation { get { return deathAnimation; } }
        [SerializeField] private GameObject deathAnimation;
        [SerializeField] private GameObject bombSprite;


        public Weapon Weapon { get; private set; } = new Weapon();

        public bool IsAlive() {
            return 0 < Hp;
        }

        public void Kill() {
            Hp = 0;
        }

        public void Damage(float damage) {
            Hp -= damage;
        }
    }
}

