using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class RotateBehaviour : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    private Rigidbody2D RB;

    private const float CONSTANT_rotateValue = 90f;         // How much the character turns when using rotation
    private const float CONSTANT_rotateSpeed = 20f;         // How much the character rotates every frame
    private const float CONSTANT_rotateCooldown = 0.5f;     // How much time to wait after rotating to be able to rotate again

    private int rotateDir = 0;                              // Variable containing the trigonometric direction (1 for left, -1 for right)
    private float rotateValue = 0;                          // Variable containing the value of rotation to apply left

    private bool isRotating = false;                        // Boolean indicating whether or not the player is rotating right now
    private bool onCooldown = false;                        // Boolean indicating whether or not the rotation is on cooldown (if so, player can't rotate)
    private bool cooldownRoutineRunning = false;            // Boolean indicating whether or not the cooldown routine is on

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Get initial references
    /// </summary>
    private void Awake()
    {
        RB = this.GetComponent<Rigidbody2D>();
    }


    /// <summary>
    ///     Every frame, the player will rotate if rotateValue is not equal to zero. Otherwise, if rotate should be on cooldown, start the cooldown coroutine
    /// </summary>
    private void FixedUpdate()
    {
        if (rotateValue > 0)
        {
            rotateValue = Mathf.Clamp(rotateValue, CONSTANT_rotateSpeed, CONSTANT_rotateValue);
            RB.rotation += rotateDir * CONSTANT_rotateSpeed;
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

    // #endregion



    // #region ==================== ROTATE FUNCTIONS ===================


    /// <summary>
    ///     When rotating right, set rotateDir to right and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateRight()
    {
        if (CanRotate())
        {
            rotateDir = -1;
            rotateValue = CONSTANT_rotateValue;
            isRotating = true;
            onCooldown = true;
        }
    }


    /// <summary>
    ///     When rotating left, set rotateDir to left and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateLeft()
    {
        if (CanRotate())
        {
            rotateDir = 1;
            rotateValue = CONSTANT_rotateValue;
            isRotating = true;
            onCooldown = true;
        }
    }


    /// <summary>
    ///     The rotate cooldown set the corresponding booleans at start and at the end of the cooldown
    /// </summary>
    private IEnumerator RotateCooldown()
    {
        cooldownRoutineRunning = true;

        yield return new WaitForSeconds(CONSTANT_rotateCooldown);

        cooldownRoutineRunning = false;
        onCooldown = false;
    }


    /// <summary>
    ///     Shortcut for !onCooldown and !isRotating, indicating if the player can rotate
    /// </summary>
    private bool CanRotate()
    {
        return !onCooldown && !isRotating;

    }

    // #endregion
}
