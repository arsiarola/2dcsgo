using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class PlayerMobility : AnimatedRecordable
    {
        public float speed;
        public float strafeSpeed;

        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            bool attackAnim = false;
            if (Input.GetMouseButtonDown(0)) {
                attackAnim = true;
                animator.SetTrigger("Attack");
            }
            Recorder.ObjectState os = new Recorder.ObjectState
            {
                position = transform.position,
                rotation = transform.rotation.eulerAngles.z,
                velocity = GetComponent<Rigidbody2D>().velocity,
                attack = attackAnim,
            };
            recorder.UpdateFrameState(recordingId, os);
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
}

