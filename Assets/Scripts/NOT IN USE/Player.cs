using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
namespace Recordable
{
    public class Player : DynamicRecordable
    {
        public float speed;
        public float strafeSpeed;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Animator>().SetTrigger("Attack");
            }
        }

        private void FixedUpdate()
        {
            Vector3 mousePosition = Misc.Tools.SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            transform.up = mousePosition - transform.position;

            float inputWS = Input.GetAxis("Vertical");
            GetComponent<Rigidbody2D>().AddForce(transform.up * speed * inputWS);

            float inputAD = Input.GetAxis("Horizontal");
            GetComponent<Rigidbody2D>().AddForce(transform.right * strafeSpeed * inputAD);
        }
    }
}*/

