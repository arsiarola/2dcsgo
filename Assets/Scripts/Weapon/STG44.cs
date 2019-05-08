using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class STG44 : Weapon
    {
        public override float FireRate { get; protected set; } = 600;
        public override float Damage { get; protected set; } = 0;
        public override float HitDifficulty { get; protected set; } = 0f;
    }
}


