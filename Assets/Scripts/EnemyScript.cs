using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public Transform player;

    public int replayKey;
    public SavedState savedState;
    public GameObject dummy;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        GameObject gObj = gameObject;
        replayKey = savedState.GetObjectKey(dummy, ref gObj);
    }

    private void Update()
    {
        SavedState.ObjectState os = new SavedState.ObjectState
        {
            position = transform.position,
            rotation = transform.rotation.eulerAngles.z,
            velocity = GetComponent<Rigidbody2D>().velocity,
            attack = false
        };
        savedState.UpdateFrameState(replayKey, os);
    }

    void FixedUpdate()
    {
        float z = Mathf.Atan2((player.transform.position.y - transform.position.y),
            (player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0, 0, z);

        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);

    }
}
