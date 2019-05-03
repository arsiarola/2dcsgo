using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Simulation : Task
    {
        private AI.CTAI CounterTerroristAI { get { return counterTerroristAI; } }
        [SerializeField] private AI.CTAI counterTerroristAI;

        private AI.TAI TerroristAI { get { return terroristAI; } }
        [SerializeField] private AI.TAI terroristAI;

        private int count = 0;

        private void FixedUpdate()
        {
            CounterTerroristAI.UpdateChildrenList();
            TerroristAI.UpdateChildrenList();

            //Debug.Log(CounterTerroristAI.Children.Count + ", " + TerroristAI.Children.Count);

            CounterTerroristAI.CheckVisibility(TerroristAI.Children);
            TerroristAI.CheckVisibility(CounterTerroristAI.Children);

            //if (count % 10 == 0) Debug.Log(CounterTerroristAI.VisibleEnemies.Count + ", " + TerroristAI.VisibleEnemies.Count);

            // Movement

            // Rotation

            // Shoot if possible
            count++;
        }
    }
}


