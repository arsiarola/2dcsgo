﻿using System.Collections;
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

        public void UpdateVisibility()
        {
            CounterTerroristAI.UpdateChildrenList();
            TerroristAI.UpdateChildrenList();
            CounterTerroristAI.CheckVisibility(TerroristAI.Children);
            TerroristAI.CheckVisibility(CounterTerroristAI.Children);
        }

        private void FixedUpdate()
        {
            // ct
            CounterTerroristAI.UpdateChildrenList();
            TerroristAI.UpdateChildrenList();
            CounterTerroristAI.CheckVisibility(TerroristAI.Children);
            SimulationStep(CounterTerroristAI);
            HideDead();

            // tero
            CounterTerroristAI.UpdateChildrenList();
            TerroristAI.UpdateChildrenList();
            TerroristAI.CheckVisibility(CounterTerroristAI.Children);
            SimulationStep(TerroristAI);
            HideDead();

            // end visibility check for recording
            UpdateVisibility();

            count++;
        }

        private void SimulationStep(AI.SideAI ai)
        {
            foreach (GameObject child in ai.Children) {
                child.GetComponent<AI.OperatorAI>().FindTarget();   // Target
                child.GetComponent<AI.OperatorAI>().Rotate();   // rotation
                child.GetComponent<AI.OperatorAI>().Shoot();    // shoot if possible
                child.GetComponent<AI.OperatorAI>().FollowPath();   // movement
            }
        }

        private void HideDead()
        {
            List<GameObject> bothChildren = new List<GameObject>();
            bothChildren.AddRange(CounterTerroristAI.Children);
            bothChildren.AddRange(TerroristAI.Children);
            foreach (GameObject child in bothChildren) {
                Operator.OperatorState operatorState = child.GetComponent<Operator.OperatorState>();
                if (!operatorState.IsAlive()) {
                    child.SetActive(false);
                    Instantiate(operatorState.DeathAnimation, child.transform.position, Quaternion.identity);
                }
            }
        }
    }
}


