using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Planning : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        private GameController GameController { get { return gameController; } }

        private Dictionary<int, Recordable.RecordableState> LastFrame { get; set; }

        public void Plan()
        {
            LastFrame = GameController.Frames[GameController.Frames.Count - 1];
        }
    }
}