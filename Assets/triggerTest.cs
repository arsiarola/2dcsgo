using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerTest : MonoBehaviour
{
    private int count = 0;
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Globals.startCounting)
        {
            count++;
            if (count == 10)
            {
                Debug.Log("Trigger set");
                animator.SetTrigger("Attack");
                foreach (AnimatorControllerParameter parameter in animator.parameters)
                {
                    if (parameter.type == AnimatorControllerParameterType.Trigger)
                    {
                        Debug.Log(parameter.name + ": " + animator.GetBool(parameter.name));
                    }

                }
            }
            GetComponent<Rigidbody2D>().AddForce(transform.up * 5 * 1);
        }
        
    }
}
