using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class AI : MonoBehaviour
    {
        public abstract Core.Side Side { get; protected set; }
        public abstract List<GameObject> VisibleEnemies { get; protected set; }
    }
}
