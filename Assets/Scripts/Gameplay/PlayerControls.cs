using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

/// <summary>
///     Class used to spawn the player arms during gameplay
/// </summary>
public class PlayerControls : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    [Header("Refs")]
    [System.NonSerialized] public Rigidbody2D RB;       // Player rigidbody ref
    public ArmBehavior[] Arms = new ArmBehavior[4];     // Array containing each arm

    [Header("Events")]
    public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
    public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

    [Header("Input Action Refs")]
    public InputActionReference UpArm;
    public InputActionReference RightArm;
    public InputActionReference DownArm;
    public InputActionReference LeftArm;
    // #endregion


    // #region ================ INIT CONTROLS FUNCTIONS ================

    /// <summary>
    ///     Init variables
    /// </summary>
    private void Awake()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
        SetupControls();
    }


    /// <summary>
    ///     Init player controls using Unity Input System and assigning corresponding interactions
    /// </summary>
    private void SetupControls()
    {
        AssignArmInteraction(UpArm, Arms[0]);
        AssignArmInteraction(RightArm, Arms[1]);
        AssignArmInteraction(DownArm, Arms[2]);
        AssignArmInteraction(LeftArm, Arms[3]);
    }


    private void AssignArmInteraction(InputActionReference _armAction, ArmBehavior _arm)
    {
        _armAction.action.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                _arm.ExtensionHoldStart();

                // FMOD Event
                OnExtendArm.Invoke();
            }
            else if (context.interaction is TapInteraction)
            {
                _arm.ExtensionTapStart();

                // FMOD Event
                OnExtendArm.Invoke();
            }
        };

        _armAction.action.canceled += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                _arm.ExtensionHoldEnd();
            }
        };
    }

    // #endregion
}
