using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public bool Planted { get; private set; } = false;
    public bool BeingDefused { get; private set; } = false;
    public GameObject Defuser { get; private set; } = null;

    public float TimePlanted { get; private set; } = 0;
    public float TimeDefuseStarted { get; private set; } = 0;

    public void Plant()
    {
        Planted = true;
        TimePlanted = Core.Vars.SimulationTime;
    }

    public bool IsTimerZero()
    {
        return Core.Vars.SimulationTime > TimePlanted + 30f && Planted;
    }

    public void StartDefuse(GameObject defuser)
    {
        Defuser = defuser;
        BeingDefused = true;
        TimeDefuseStarted = Core.Vars.SimulationTime;
    }

    public bool IsDefused()
    {
        if (Defuser != null && !Defuser.GetComponent<Operator.OperatorState>().IsAlive()) {
            StopDefuse();
        }
        return Core.Vars.SimulationTime > TimeDefuseStarted + 5f && Planted && BeingDefused;
    }

    public void StopDefuse()
    {
        Defuser = null;
        BeingDefused = false;
    }
}
