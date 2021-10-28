using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;
using Enums;

public class RotateBehaviourOld : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    private Rigidbody2D RB;

    private float rotationTorqueMin;
    private float rotationTorqueMax;
    private float rotationForceMin;
    private float rotationForceMax;
    private float rotationChargeTimeMax;
    private float rotationCooldown;

    private float rotateDir;
    private float rotateValue = 0f;                          // Variable containing the value of rotation to apply left
    private float forceValue = 0f;
    private float holdTime = 0f;

    /*private bool isRotating = false;                        // Boolean indicating whether or not the player is rotating right now
    private bool onCooldown = false;                        // Boolean indicating whether or not the rotation is on cooldown (if so, player can't rotate)
    private bool cooldownRoutineRunning = false;            // Boolean indicating whether or not the cooldown routine is on
    private bool isHolding = false;                         // Boolean indicating whether or not a rotate button is being held
    */
    [System.NonSerialized] public PlayerRotateState PlayerRotateState = PlayerRotateState.Ready;
    // #endregion


    /*
    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Get initial references
    /// </summary>
    private void Awake()
    {
        RB = this.GetComponent<Rigidbody2D>();
        InitParameters();
    }

    private void InitParameters()
    {
        rotationTorqueMin = GameManager.Instance.ParamData.PARAM_Player_RotationTorqueMin;
        rotationTorqueMax = GameManager.Instance.ParamData.PARAM_Player_RotationTorqueMax;
        rotationForceMin = GameManager.Instance.ParamData.PARAM_Player_RotationForceMin;
        rotationForceMax = GameManager.Instance.ParamData.PARAM_Player_RotationForceMax;
        rotationChargeTimeMax = GameManager.Instance.ParamData.PARAM_Player_RotationChargeTimeMax;
        rotationCooldown = GameManager.Instance.ParamData.PARAM_Player_RotationCooldown;
    }


    /// <summary>
    ///     Every frame, the player will rotate if rotateValue is not equal to zero. Otherwise, if rotate should be on cooldown, start the cooldown coroutine
    /// </summary>
    private void Update()
    {
        if (isHolding)
        {
            if (holdTime < rotationChargeTimeMax)
            {
                holdTime += Time.deltaTime;
                holdTime = Mathf.Clamp(holdTime, 0, rotationChargeTimeMax);
                rotateValue = Mathf.Lerp(rotationTorqueMin, rotationTorqueMax, holdTime/rotationChargeTimeMax);
                forceValue = Mathf.Lerp(rotationForceMin, rotationForceMax, holdTime/rotationChargeTimeMax);
            }
        }
        else if (CanRotate())
        {
            holdTime = 0f;
            isRotating = false;
            RB.AddTorque(rotateDir * rotateValue, ForceMode2D.Impulse);
            RB.AddForce(transform.up * rotateDir * forceValue, ForceMode2D.Impulse);
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
        isHolding = false;
        isRotating = true;
        //onCooldown = false;
        rotateDir = _rotateDir;
        rotateValue = rotationTorqueMin;
        forceValue = rotationForceMin;
    }


    /// <summary>
    ///     When rotating right, set rotateDir to right and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateHold(int _rotateDir)
    {
        isHolding = true;
        rotateDir = _rotateDir;
        rotateValue = rotationTorqueMin;
        forceValue = rotationForceMin;
    }


    /// <summary>
    ///     When rotating right, set rotateDir to right and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateRelease(int _rotateDir)
    {
        if (isHolding && rotateDir == _rotateDir)
        {
            isHolding = false;
            isRotating = true;
            //onCooldown = true;
        }
    }


    /// <summary>
    ///     The rotate cooldown set the corresponding booleans at start and at the end of the cooldown
    /// </summary>
    private IEnumerator RotateCooldown()
    {
        cooldownRoutineRunning = true;

        yield return new WaitForSeconds(rotationCooldown);

        cooldownRoutineRunning = false;
        onCooldown = false;
    }


    /// <summary>
    ///     Shortcut for !onCooldown and !isRotating, indicating if the player can rotate
    /// </summary>
    private bool CanRotate()
    {
        return !onCooldown && isRotating;
    }*/

    // #endregion
}
