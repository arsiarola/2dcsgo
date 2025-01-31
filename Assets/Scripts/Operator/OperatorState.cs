﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operator {
    public class OperatorState : MonoBehaviour {
        public float Hp { get; private set; } = 100;
        public GameObject DeathAnimation { get { return deathAnimation; } }
        [SerializeField] private GameObject deathAnimation;
        [SerializeField] private GameObject bombSprite;

        public GameObject Weapon { get { return weapon; } private set { weapon = value; } }
        [SerializeField] private GameObject weapon;

        public bool IsAlive() {
            return 0 < Hp;
        }

        public void Kill() {
            Hp = 0;
        }

        public void Damage(float damage) {
            GetComponent<Animator>().ResetTrigger("Hit");
            GetComponent<Animator>().SetTrigger("Hit");
            Hp -= damage;
        }
    }
}

