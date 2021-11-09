using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;
using Enums;

public class RotateBehaviour : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    public Rigidbody2D RB;                                                  // Rigidbody2D reference

    [Header("Parameters")]
    private float rotationTorque;                                           // Rotation torque parameter value
    private float rotationForce;                                            // Rotation force parameter value
    private bool useFactorForRotation;                              // Whether or not to use a factor for the rotation input value

    [Header("Variables")]
    [System.NonSerialized] public PlayerRotateState PlayerRotateState;      // Contain the enum of the rotate state (Ready, RotatingRight, RotatingLeft, or OnCooldown)
    private float rotateValue;                                              // Current value of rotation to apply
    private float rotationFactor;                                           // Rotation factor value, useful if useFactorForRotation is true; otherwise is set to 1

    // #endregion



    // #region ==================== INIT FUNCTIONS ====================

    /// <summary>
    ///     Init all class variables
    /// </summary>
    private void Awake()
    {
        InitParameters();
        InitVariables();
    }


    /// <summary>
    ///     Init parameters
    /// </summary>
    private void InitParameters()
    {
        // TODO: Add these parameters to ParamData
        rotationTorque = GameManager.Instance.ParamData.PARAM_Player_RotationTorque;
        rotationForce = GameManager.Instance.ParamData.PARAM_Player_RotationForce;
        useFactorForRotation = GameManager.Instance.ParamData.PARAM_Player_UseFactorForRotation;
    }


    /// <summary>
    ///     Init variables
    /// </summary>
    private void InitVariables()
    {
        rotationFactor = 1f;
    }

    // #endregion



    // #region =================== CONTROLS FUNCTIONS ==================

    /// <summary>
    ///     Input function called when rotating using the stick
    /// </summary>
    public void Input_RotateStick(InputAction.CallbackContext _context)
    {
        // Read Vector2 value of the stick input
        Vector2 _inputValue = _context.ReadValue<Vector2>();

        if (useFactorForRotation)
        {
            rotationFactor = Mathf.Abs(_inputValue.x);
        }

        // If the stick is pushed to the right side, then the player will rotate to the right
        if (_inputValue.x > 0)
        {
            PlayerRotateState = PlayerRotateState.RotatingRight;
        }
        // If the stick is pushed to the left side, then the player will rotate to the left
        else if (_inputValue.x < 0)
        {
            PlayerRotateState = PlayerRotateState.RotatingLeft;
        }
        // Else, set the player state to ready
        else
        {
            PlayerRotateState = PlayerRotateState.Ready;
        }
    }

    // #endregion



    // #region ==================== ROTATE FUNCTIONS ====================

    /// <summary>
    ///
    /// </summary>
    private void Update()
    {
        if (PlayerRotateState is PlayerRotateState.RotatingRight)
        {
            RB.AddTorque(-1 * rotationTorque * rotationFactor, ForceMode2D.Force);
            RB.AddForce(transform.up * -1 * rotationForce * rotationFactor, ForceMode2D.Force);
        }
        else if (PlayerRotateState is PlayerRotateState.RotatingLeft)
        {
            RB.AddTorque(rotationTorque * rotationFactor, ForceMode2D.Force);
            RB.AddForce(transform.up * rotationForce * rotationFactor, ForceMode2D.Force);
        }
    }







    /*
    // #region ==================== CLASS VARIABLES ====================

    public Rigidbody2D RB;

    private float rotationTorqueMin;
    private float rotationTorqueMax;
    private float rotationForceMin;
    private float rotationForceMax;
    private float rotationChargeTimeMax;
    private float rotationCooldown;

    private int rotateDir = 0;                                  // Variable containing the trigonometric direction (1 for left, -1 for right)
    //private float rotateValue = 0f;                           // Variable containing the value of rotation to apply left
    //private float forceValue = 0f;

    private float rotateValueRight = 0f;                          // Variable containing the value of rotation to apply left
    private float forceValueRight = 0f;
    private float rotateValueLeft = 0f;                          // Variable containing the value of rotation to apply left
    private float forceValueLeft = 0f;

    //  private float holdTime = 0f;
    private float holdTimeLeft = 0f;
    private float holdTimeRight = 0f;

    public System.DateTime startTimeRight = System.DateTime.MinValue;
    public System.DateTime startTimeLeft = System.DateTime.MinValue;


    private bool isRotating = false;

    private bool onCooldown = false;                        // Boolean indicating whether or not the rotation is on cooldown (if so, player can't rotate)
    private bool cooldownRoutineRunning = false;            // Boolean indicating whether or not the cooldown routine is on
    //private bool isHolding = false;                         // Boolean indicating whether or not a rotate button is being held
    private bool isHoldingLeft = false;                         // Boolean indicating whether or not a rotate button is being held
    private bool isHoldingRight = false;                         // Boolean indicating whether or not a rotate button is being held

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Get initial references
    /// </summary>
    private void Awake()
    {
        //RB = gameObject.GetComponent<Rigidbody2D>();
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


    // #endregion



    // #region =================== CONTROLS FUNCTIONS ==================

    public void Input_RotateRight(InputAction.CallbackContext _context)
    {
        print("Input right");
        if (_context.interaction is HoldInteraction && _context.started &&  startTimeRight == System.DateTime.MinValue)
        {
            startTimeRight = System.DateTime.UtcNow;
        }
        else if (_context.canceled)
        {
            if (_context.interaction is HoldInteraction)
            {
                print("RR");
                System.TimeSpan ts = System.DateTime.UtcNow - startTimeRight;
                holdTimeRight = Mathf.Clamp((float)ts.TotalSeconds, 0, rotationChargeTimeMax);
                rotateValueRight = Mathf.Lerp(rotationTorqueMin, rotationTorqueMax, holdTimeRight / rotationChargeTimeMax);
                if (rotateValueRight == float.NaN) rotateValueRight = rotationTorqueMin;


                forceValueRight = Mathf.Lerp(rotationForceMin, rotationForceMax, holdTimeRight / rotationChargeTimeMax);
                if (forceValueRight == float.NaN) forceValueRight = rotationForceMin;

                print("hold");
                startTimeRight = System.DateTime.MinValue;

                print("rotate dir left time pressed " + holdTimeRight + " : force " + forceValueRight + "/" + rotationForceMax);


                RB.AddTorque(-1 * rotationTorqueMin, ForceMode2D.Impulse);
                RB.AddForce(transform.up * -1 * rotationForceMin, ForceMode2D.Impulse);

                //RB.AddTorque(-1 * rotateValueRight, ForceMode2D.Impulse);
                //RB.AddForce(transform.up * -1 * forceValueRight, ForceMode2D.Impulse);
            }
            else if (_context.interaction is TapInteraction)
            {
                //RB.AddTorque(-1 * rotationTorqueMin, ForceMode2D.Impulse);
                //RB.AddForce(transform.up * -1 * rotationForceMin, ForceMode2D.Impulse);
            }
        }
    }


    public void Input_RotateLeft(InputAction.CallbackContext _context)
    {
        print("Input left");
        if (_context.interaction is HoldInteraction && _context.started && startTimeLeft == System.DateTime.MinValue)
        {
            startTimeLeft = System.DateTime.UtcNow;
        }
        else if (_context.canceled)
        {
            if (_context.interaction is HoldInteraction)
            {
                print("RL");
                // print("holded");
                System.TimeSpan ts = System.DateTime.UtcNow - startTimeLeft;
                holdTimeLeft = Mathf.Clamp((float)ts.TotalSeconds, 0, rotationChargeTimeMax);

                rotateValueLeft = Mathf.Lerp(rotationTorqueMin, rotationTorqueMax, holdTimeLeft / rotationChargeTimeMax);
                if (rotateValueLeft == float.NaN) rotateValueLeft = rotationTorqueMin;

                forceValueLeft = Mathf.Lerp(rotationForceMin, rotationForceMax, holdTimeLeft / rotationChargeTimeMax);


                if (forceValueLeft == float.NaN) forceValueLeft = rotationForceMin;

                print("rotate dir left time pressed "+holdTimeLeft+" : force "+forceValueLeft+"/"+rotationForceMax);

                startTimeLeft = System.DateTime.MinValue;

                RB.AddTorque(1 * rotationTorqueMin, ForceMode2D.Impulse);
                RB.AddForce(transform.up * 1 * rotationForceMin, ForceMode2D.Impulse);

                //RB.AddTorque(1 * rotateValueLeft, ForceMode2D.Impulse);
                //RB.AddForce(transform.up * 1 * forceValueLeft, ForceMode2D.Impulse);
            }
            else if (_context.interaction is TapInteraction)
            {

                //print("tapped");
                ////print("rotate dir left tapping");
                //RB.AddTorque(1 * rotationTorqueMin, ForceMode2D.Impulse);
                //RB.AddForce(transform.up * 1 * rotationForceMin, ForceMode2D.Impulse);
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
        if(_rotateDir>0)
        {
            isHoldingLeft = false;
            isRotating = true;
            //onCooldown = false;
            rotateDir = _rotateDir;
            rotateValueLeft = rotationTorqueMin;
            forceValueLeft = rotationForceMin;
        }
        else
        {
            isHoldingRight = false;
            isRotating = true;
            //onCooldown = false;
            rotateDir = _rotateDir;
            rotateValueRight = rotationTorqueMin;
            forceValueRight = rotationForceMin;
        }

    }


    /// <summary>
    ///     When rotating right, set rotateDir to right and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateHold(int _rotateDir)
    {
        if (_rotateDir > 0)
        {
            isHoldingLeft = true;
            rotateDir = _rotateDir;
            rotateValueLeft = rotationTorqueMin;
            forceValueLeft = rotationForceMin;
        }
        else
        {
            isHoldingRight = true;
            rotateDir = _rotateDir;
            rotateValueRight = rotationTorqueMin;
            forceValueRight = rotationForceMin;
        }

    }


    /// <summary>
    ///     When rotating right, set rotateDir to right and initialize the rotate value and the state booleans
    /// </summary>
    public void RotateRelease(int _rotateDir)
    {

        if (_rotateDir > 0)
        {
            isHoldingLeft = false;
            isRotating = true;
        }
        else
        {
            isHoldingRight = false;
            isRotating = true;
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
        return !onCooldown;
        //return !onCooldown && isRotating;
    }

    // #endregion
    */
}
