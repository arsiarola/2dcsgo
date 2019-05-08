using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Task : MonoBehaviour
    {
        /// <summary> Reference to the GameController Script </summary>
        protected GameController GameController { get { return gameController; } }
        [SerializeField] private GameController gameController;

        protected virtual void Awake()
        {
            gameObject.SetActive(false);    // disable the task at start. This prevents it from executing its update methods and also helps in clarity
        }
    }
}


