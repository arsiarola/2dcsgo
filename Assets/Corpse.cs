using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Animator>().ResetTrigger("Hit");
        GetComponent<Animator>().SetTrigger("Hit");
    }
}
