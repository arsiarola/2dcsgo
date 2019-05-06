using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operator
{
    public class Weapon
    {
        public float FireRate { get; private set; } = 600;    // rpm
        public float Stage { get; private set; } = 0;
        public float Damage { get; private set; } = 36;
        public float HitDifficulty { get; private set; } = 0.8f;

        public Weapon()
        {

        }

        public void FireAt(GameObject target)
        {
            if (Stage == 0) {
                if (Random.Range(0.0f, 1.0f) < HitDifficulty) target.GetComponent<OperatorState>().Damage(Damage);
            }
            Stage += FireRate / 60 * Time.fixedDeltaTime;
            while (1 < Stage) {
                if (Random.Range(0.0f, 1.0f) < HitDifficulty) target.GetComponent<OperatorState>().Damage(Damage);
                Stage -= 1;
            }
        }
    }
}