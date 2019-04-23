﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");   // TESTING
        Instantiate(explosion, collision.transform.position, Quaternion.identity);
        Destroy(collision.gameObject);
    }
}
