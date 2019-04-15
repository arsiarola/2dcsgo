using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMobility : AnimatedRecordable
{
    public float speed;
    public float strafeSpeed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            ActivateAnimTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        Vector3 mousePosition = Tools.SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        transform.up = mousePosition - transform.position;

        float inputWS = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed * inputWS);

        float inputAD = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().AddForce(transform.right * strafeSpeed * inputAD);
    }
}
