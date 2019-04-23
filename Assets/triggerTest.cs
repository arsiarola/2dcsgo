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
            float smooth = 5.0f;
            Vector3 relativePos = new Vector3(0,0,0) - transform.position;
            Quaternion target = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * smooth);
            GetComponent<Rigidbody2D>().AddForce(transform.up * 50);
        }
        
    }
}
