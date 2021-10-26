using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

public class RotateBehaviour : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    private Rigidbody2D RB;

    [SerializeField] private const float CONSTANT_rotateValue = 90f;         // How much the character turns when using rotation
    [SerializeField] private const float CONSTANT_rotateSpeed = 20f;         // How much the character rotates every frame
    [SerializeField] private const float CONSTANT_rotateCooldown = 0.5f;     // How much time to wait after rotating to be able to rotate again
    [SerializeField] private const float CONSTANT_rotateIncrement = 5f;
    [SerializeField] private const float CONSTANT_rotateMaximum = 1000f;

    private int rotateDir = 0;                              // Variable containing the trigonometric direction (1 for left, -1 for right)
    private float rotateValue = 0;                          // Variable containing the value of rotation to apply left

    private bool isRotating = false;                        // Boolean indicating whether or not the player is rotating right now
    private bool onCooldown = false;                        // Boolean indicating whether or not the rotation is on cooldown (if so, player can't rotate)
    private bool cooldownRoutineRunning = false;            // Boolean indicating whether or not the cooldown routine is on
    private bool isHolding = false;                         // Boolean indicating whether or not a rotate button is being held

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
        if (isHolding)
        {
            if (rotateValue < CONSTANT_rotateMaximum)
            {
                rotateValue += CONSTANT_rotateIncrement;
            }
            else
            {
                rotateValue = CONSTANT_rotateMaximum;
            }
        }
        else if (isRotating && rotateValue > 0)
        {
            rotateValue = Mathf.Clamp(rotateValue, CONSTANT_rotateSpeed, CONSTANT_rotateMaximum);
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



    // #region =================== CONTROLS FUNCTIONS ==================

    public void Input_RotateRight(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            if (_context.interaction is HoldInteraction)
            {
                RotateHold(-1);
            }
            else if (_context.interaction is TapInteraction)
            {
                RotateTap(-1);
            }
        }
        else if (_context.canceled)
        {
            if (_context.interaction is HoldInteraction)
            {
                RotateRelease(-1);
            }
        }
    }


    public void Input_RotateLeft(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            if (_context.interaction is HoldInteraction)
            {
                RotateHold(1);
            }
            else if (_context.interaction is TapInteraction)
            {
                RotateTap(1);
            }
        }
        else if (_context.canceled)
        {
            if (_context.interaction is HoldInteraction)
            {
                RotateRelease(1);
            }
        }
    }

    // #endregion



    // #region ==================== ROTATE FUNCTIONS ===================

    /// <summary>
    ///     When rotating left, set rotateDir to left and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateTap(int _rotateDir)
    {
        if (CanRotate())
        {
            rotateDir = _rotateDir;
            rotateValue = CONSTANT_rotateValue;
            isRotating = true;
            onCooldown = true;
        }
    }


    /// <summary>
    ///     When rotating right, set rotateDir to right and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateHold(int _rotateDir)
    {
        if (CanRotate())
        {
            isHolding = true;
            rotateDir = _rotateDir;
            rotateValue = CONSTANT_rotateValue;
        }
    }


    /// <summary>
    ///     When rotating right, set rotateDir to right and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateRelease(int _rotateDir)
    {
        if (CanRotate() && rotateDir == _rotateDir)
        {
            isHolding = false;
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
