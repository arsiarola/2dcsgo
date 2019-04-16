using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recordable
{
    public class Enemy : RigidAnimated
    {
        public float speed;
        public Transform player;

        protected override void Awake()
        {
            base.Awake();
            player = GameObject.Find("Player").transform;
        }

        void FixedUpdate()
        {
            float z = Mathf.Atan2((player.transform.position.y - transform.position.y),
                (player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, z);

            GetComponent<Rigidbody2D>().AddForce(transform.up * speed);

        }
    }
}

