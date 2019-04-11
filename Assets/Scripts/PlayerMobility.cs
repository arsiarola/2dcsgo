using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMobility : MonoBehaviour
{
    public float speed;
    public float strafeSpeed;
    Animator anim;

    public int replayKey;
    public SavedState savedState;
    public GameObject dummy;

    private static void SetZAxisToZero(ref Vector3 v)
    {
        v = v + new Vector3(0, 0, -v.z);
    }

    private static Vector3 SetZAxisToZero(Vector3 v)
    {
        return v + new Vector3(0, 0, -v.z);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject gObj = gameObject;
        replayKey = savedState.GetObjectKey(dummy, ref gObj);
        GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
    }

    void Update()
    {
        bool attackAnim = false;
        float nt = -1;
        if (Input.GetMouseButtonDown (0))
        {
            attackAnim = true;
            anim.SetTrigger("Attack");
            Debug.Log("jotai vaa");
        }
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("AttackAnim")) {
            //Debug.Log("Attack anim");
            nt = GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).normalizedTime;
        }
        SavedState.ObjectState os = new SavedState.ObjectState
        {
            position = transform.position,
            rotation = transform.rotation.eulerAngles.z,
            velocity = GetComponent<Rigidbody2D>().velocity,
            attack = attackAnim,
            normalizedTime = nt
        };
        savedState.UpdateFrameState(replayKey, os);
    }

    void FixedUpdate()
    {
        Vector3 mousePosition = SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        transform.up = mousePosition - transform.position;

        float inputWS = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * inputWS);

        float inputAD = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().AddForce(transform.right * strafeSpeed * inputAD);
    }

    public void UpdateTurn(Vector3 mousePosition, float inputWS, float inputAD, bool attack)
    {
        if (attack) {
            anim.SetTrigger("Attack");
        }

        transform.up = mousePosition - transform.position;

        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * inputWS);

        GetComponent<Rigidbody2D>().AddForce(transform.right * strafeSpeed * inputAD);
    }
}
