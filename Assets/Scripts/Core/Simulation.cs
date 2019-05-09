using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Simulation : Task
    {
        private AI.CTAI CounterTerroristAI { get; set; }

        private AI.TAI TerroristAI { get; set; }

        private int count = 0;

        protected override void Awake()
        {
            CounterTerroristAI = GameController.CounterTerrorists;
            TerroristAI = GameController.Terrorists;
        }

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
                if (ai.Equals(TerroristAI) && child.GetComponent<Operator.TOperatorState>().Bomb == true){
                    child.GetComponent<AI.OperatorAI>().GetComponent<AI.TOperatorAI>().PlantBomb(child.transform.position);
                }
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


