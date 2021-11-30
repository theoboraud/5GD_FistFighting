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
    public Player Player;

    [Header("Parameters")]
    private float rotationTorque_InAir;                                     // Rotation torque parameter value in air
    private float rotationForce_InAir;                                      // Rotation force parameter value in air
    private float rotationTorque_OnGround;                                  // Rotation torque parameter value in air
    private float rotationForce_OnGround;                                   // Rotation force parameter value in air
    private bool useFactorForRotation;                                      // Whether or not to use a factor for the rotation input value

    [Header("Variables")]
    private float torqueValue;                                              // Current value of torque to apply
    private float forceValue;                                               // Current value of force to apply
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
        rotationTorque_InAir = GameManager.Instance.ParamData.PARAM_Player_RotationTorque_InAir;
        rotationForce_InAir = GameManager.Instance.ParamData.PARAM_Player_RotationForce_InAir;
        rotationTorque_OnGround = GameManager.Instance.ParamData.PARAM_Player_RotationTorque_OnGround;
        rotationForce_OnGround = GameManager.Instance.ParamData.PARAM_Player_RotationForce_OnGround;
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
    public void Input_Rotating(float _inputValue)
    {
        if (Player.PlayerGameState == PlayerGameState.Alive || Player.PlayerGameState == PlayerGameState.Invincible)
        {
            if (useFactorForRotation)
            {
                rotationFactor = Mathf.Abs(_inputValue);
            }

            // If the stick is pushed to the right side, then the player will rotate to the right
            if (_inputValue > 0f)
            {
                Player.PlayerRotateState = PlayerRotateState.RotatingRight;
            }
            // If the stick is pushed to the left side, then the player will rotate to the left
            else if (_inputValue < 0f)
            {
                Player.PlayerRotateState = PlayerRotateState.RotatingLeft;
            }
            // Else, set the player state to ready
            else
            {
                Player.PlayerRotateState = PlayerRotateState.Ready;
            }
        }
    }

    // #endregion



    // #region ==================== ROTATE FUNCTIONS ====================

    /// <summary>
    ///
    /// </summary>
    private void FixedUpdate()
    {
        float _torqueDir = 0;
        Vector3 _forceDir = Vector3.zero;

        torqueValue = rotationTorque_InAir;
        forceValue = rotationForce_InAir;

        if (Player.PlayerRotateState is PlayerRotateState.RotatingRight)
        {
            _torqueDir = -1f;
            _forceDir = Vector3.right;
        }
        else if (Player.PlayerRotateState is PlayerRotateState.RotatingLeft)
        {
            _torqueDir = 1f;
            _forceDir = -Vector3.right;
        }

        if (Player.PlayerPhysicState is PlayerPhysicState.OnGround)
        {
            _forceDir = Vector3.up;
            torqueValue = rotationTorque_OnGround;
            forceValue = rotationForce_OnGround;
        }

        RB.AddTorque(_torqueDir * torqueValue * rotationFactor * Time.fixedDeltaTime, ForceMode2D.Force);
        RB.AddForce(_forceDir * forceValue * rotationFactor * Time.fixedDeltaTime, ForceMode2D.Force);

        //RB.angularVelocity = Mathf.Clamp(RB.angularVelocity, 0f, 1f);
    }

    // #endregion

}
