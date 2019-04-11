using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(explosion, collision.transform.position, Quaternion.identity);
        Destroy(collision.gameObject);
    }
}
