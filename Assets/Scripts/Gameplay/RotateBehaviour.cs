using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class RotateBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private float rotateDir = 0;
    private float rotateSpeed = 3f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(rotateDir != 0)
        {
            rb.rotation += rotateDir * rotateSpeed;
        }
    }

    public void RotateRight(InputAction.CallbackContext _value)
    {
        rotateDir = -1 * _value.ReadValue<float>();
    }

    public void RotateLeft(InputAction.CallbackContext _value)
    {
        rotateDir = _value.ReadValue<float>();
    }
}
