using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class RotateBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;

    private const float CONSTANT_rotateValue = 90f;      // How much the character turns when using rotation
    private const float CONSTANT_rotateSpeed = 30f;                     //

    private int rotateDir = 0;                          // Variable containing the trigonometric direction (1 for left, -1 for right)
    private float rotateValue = 0;                      // Variable containing the value of rotation to apply left

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(rotateValue > 0)
        {
            rb.rotation += rotateDir * CONSTANT_rotateSpeed;
            rotateValue -= CONSTANT_rotateSpeed;
        }
        else
        {
            rotateDir = 0;
            rotateValue = 0;
        }
    }

    public void RotateRight(InputAction.CallbackContext _value)
    {
        if(rotateValue == 0)
        {
            rotateDir = -1;
            rotateValue = CONSTANT_rotateValue;
        }
    }

    public void RotateLeft(InputAction.CallbackContext _value)
    {
        if(rotateValue == 0)
        {
            rotateDir = 1;
            rotateValue = CONSTANT_rotateValue;
        }
    }
}
