using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CTOperatorAI : OperatorAI
    {
        public override Core.Side Side { get; protected set; } = Core.Side.CounterTerrorist;
        public bool WillDefuse { get; set; } = false;
        public bool Defusing { get; set; } = false;
        public float DefusingStarted { get; set; } = 0;


        public void Defuse()
        {
            GameObject bomb = GameObject.Find(Misc.Constants.GAME_CONTROLLER_NAME).GetComponent<Core.GameController>().Bomb;
            BombScript bombScript = bomb.GetComponent<BombScript>();
            if (!(NextPointInPath < Path.Count - 1) && WillDefuse) { 
                if (!bombScript.IsTimerZero() && !bombScript.BeingDefused) {
                    Defusing = true;
                    bombScript.StartDefuse(gameObject);
                    DefusingStarted = Core.Vars.SimulationTime;
                }
            } else {
                Defusing = false;
                if (bombScript.Defuser == gameObject) {
                    bombScript.StopDefuse();
                }
            }
        }

        public void SetWillDefuse(bool b)
        {
            WillDefuse = b;
        }
    }
}

