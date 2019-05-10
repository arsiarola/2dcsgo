using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class TOperatorAI : OperatorAI {
        public override Core.Side Side { get; protected set; } = Core.Side.Terrorist;
        public bool WillPlantBomb { get; protected set; } = false;
        public bool Planting { get; protected set; } = false;
        public float PlantingStarted { get; protected set; } = 0;


        public void SetWillPlant(bool b)
        {
            WillPlantBomb = b;
        }

        public void PlantBomb()
        {
            if (!(NextPointInPath < Path.Count - 1) && WillPlantBomb) {
                if (!Planting) {
                    Planting = true;
                    PlantingStarted = Core.Vars.SimulationTime;
                }
            } else {
                Planting = false;
            }
            if (Planting) {
                if (Core.Vars.SimulationTime - PlantingStarted > 3f) {
                    Planting = false;
                    WillPlantBomb = false;
                    GetComponent<Operator.TOperatorState>().SetBomb(false);
                    GameObject bomb = GameObject.Find(Misc.Constants.GAME_CONTROLLER_NAME).GetComponent<Core.GameController>().Bomb;
                    bomb.transform.position = transform.position;
                    bomb.GetComponent<BombScript>().Plant();
                }
            }
        }

        public void PickUpBomb()
        {
            GameObject bomb = GameObject.Find(Misc.Constants.GAME_CONTROLLER_NAME).GetComponent<Core.GameController>().Bomb;
            if (Vector3.Distance(bomb.transform.position, transform.position) < 3f && !bomb.GetComponent<BombScript>().Planted) {
                GetComponent<Operator.TOperatorState>().SetBomb(true);
                bomb.transform.position = new Vector3(400, 400);
            }
        }
       

    }
}


