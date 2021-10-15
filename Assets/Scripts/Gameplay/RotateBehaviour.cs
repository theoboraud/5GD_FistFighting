using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class RotateBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;

    private const float CONSTANT_rotateValue = 90f;      // How much the character turns when using rotation
    private const float CONSTANT_rotateSpeed = 20f;

    private int rotateDir = 0;                          // Variable containing the trigonometric direction (1 for left, -1 for right)
    private float rotateValue = 0;                      // Variable containing the value of rotation to apply left

    private bool isRotating = false;
    private bool onCooldown = false;
    private bool cooldownRoutineRunning = false;
    private float rotateCooldown = 0.5f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rotateValue > 0)
        {
            rotateValue = Mathf.Clamp(rotateValue, CONSTANT_rotateSpeed, CONSTANT_rotateValue);
            rb.rotation += rotateDir * CONSTANT_rotateSpeed;
            rotateValue -= CONSTANT_rotateSpeed;
        }

        if (isRotating && rotateValue <= 0)
        {
            isRotating = false;
            rotateDir = 0;
            rotateValue = 0;
        }

        if (onCooldown && !isRotating && !cooldownRoutineRunning)
        {
            StartCoroutine(RotateCooldown());
        }
    }

    public void RotateRight(InputAction.CallbackContext _value)
    {
        if (CanRotate())
        {
            rotateDir = -1;
            rotateValue = CONSTANT_rotateValue;
            isRotating = true;
            onCooldown = true;
        }
    }

    public void RotateLeft(InputAction.CallbackContext _value)
    {
        if (CanRotate())
        {
            rotateDir = 1;
            rotateValue = CONSTANT_rotateValue;
            isRotating = true;
            onCooldown = true;
        }
    }

    private IEnumerator RotateCooldown()
    {
        print("RotateCooldown started");
        cooldownRoutineRunning = true;

        yield return new WaitForSeconds(rotateCooldown);

        cooldownRoutineRunning = false;
        onCooldown = false;
        print("RotateCooldown over");
    }

    private bool CanRotate()
    {
        return !onCooldown && !isRotating;
    }
}
