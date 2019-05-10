using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Simulation : Task
    {
        private AI.CTAI CounterTerroristAI { get; set; }

        private AI.TAI TerroristAI { get; set; }

        protected override void Awake()
        {
            base.Awake();
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

        public void StepThrough()
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

            Vars.SimulationTime += Time.fixedDeltaTime;
            //Debug.Log(Vars.SimulationTime);

            BombScript bomb = GameController.Bomb.GetComponent<BombScript>();
            if (GameController.Winner == null) {
                if (CounterTerroristAI.Children.Count == 0 || bomb.IsTimerZero()) {
                    GameController.Winner = Side.Terrorist;
                    Debug.Log("T WIN");
                }
                else if ((TerroristAI.Children.Count == 0 && !bomb.Planted) || bomb.IsDefused()) {
                    GameController.Winner = Side.CounterTerrorist;
                    Debug.Log("CT WIN");
                }
            }
        }

        private void FixedUpdate()
        {
            StepThrough();
        }

        private void SimulationStep(AI.SideAI ai)
        {
            foreach (GameObject child in ai.Children) {
                child.GetComponent<AI.OperatorAI>().FindTarget();   // Target
                child.GetComponent<AI.OperatorAI>().Rotate();   // rotation
                child.GetComponent<AI.OperatorAI>().Shoot();    // shoot if possible
                child.GetComponent<AI.OperatorAI>().FollowPath();   // movement
                if (ai.Side == Side.Terrorist) {
                    child.GetComponent<AI.TOperatorAI>().PlantBomb();   // movement
                    child.GetComponent<AI.TOperatorAI>().PickUpBomb();
                }
                if (ai.Side == Side.CounterTerrorist) {
                    child.GetComponent<AI.CTOperatorAI>().Defuse();
                }
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
                    if (child.GetComponent<AI.OperatorAI>().Side == Side.Terrorist && child.GetComponent<Operator.TOperatorState>().HasBomb) {
                        Debug.Log("HERE");
                        GameController.Bomb.transform.position = child.transform.position;
                    }
                }
            }
        }
    }
}


