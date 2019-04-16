using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum GamePhase
    {
        CounterTerroristOrders,
        TerroristOrders,
        CounterTerroristReplay,
        TerroristReplay
    }

    public class GameController : MonoBehaviour
    {
        GamePhase gamePhase = GamePhase.CounterTerroristOrders;


    }
}


