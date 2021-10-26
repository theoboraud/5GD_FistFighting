using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlateform : MonoBehaviour
{
    public float force;
    public Vector2 dir;

    private void Start()
    {
        if (force == 0) force = 10;
        if (dir == Vector2.zero) dir = transform.up.normalized;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.rigidbody.AddForce(dir*force,ForceMode2D.Impulse);
    }
}
