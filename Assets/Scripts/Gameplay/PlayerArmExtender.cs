using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

/// <summary>
///     Class used to spawn the player arms during gameplay
/// </summary>
public class PlayerArmExtender : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    [Header("Refs")]
    [SerializeField] GameObject Arm;                    // Arm GO ref for each arm
    public Rigidbody2D rb;                              // Player rigidbody ref
    public ArmBehavior[] Arms = new ArmBehavior[4];     // Array containing each arm

    [Header("Events")]
    public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
    public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

    [SerializeField] private InputActionReference downArm;
    [SerializeField] private InputActionReference leftArm;
    [SerializeField] private InputActionReference rightArm;
    [SerializeField] private InputActionReference upArm;
    // #endregion


    // #region ===================== INIT FUNCTIONS ====================

    private void Awake()
    {
        SetupControls();
    }


    private void SetupControls()
    {
        AssignArmInteraction(downArm, Arms[0]);
        AssignArmInteraction(leftArm, Arms[1]);
        AssignArmInteraction(rightArm, Arms[2]);
        AssignArmInteraction(upArm, Arms[3]);
    }


    private void AssignArmInteraction(InputActionReference _armAction, ArmBehavior _arm)
    {
        _armAction.action.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                ExtendArmHold(_arm);
            }
            else if (context.interaction is TapInteraction)
            {
                ExtendArmTap(_arm);
            }
        };

        _armAction.action.canceled += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                ExtendArmRelease(_arm);
            }
        };
    }

    // #endregion


    // #region ================ ARM EXTENSION FUNCTIONS ================

    /// <summary>
    ///     Function called when extending and holding the button of a given arm, depending on input
    /// <param>
    ///     ArmBehavior _arm: arm reference script
    /// </param>
    /// </summary>
    private void ExtendArmHold(ArmBehavior _arm)
    {
        if (!_arm.IsExtending && !_arm.IsExtended && !_arm.IsUnextending)
        {
            // FMOD Event
            OnExtendArm.Invoke();

            _arm.ExtensionHoldStart();
            print("Arm Held");
        }
    }


    /// <summary>
    ///     Function called when extending a given arm, depending on input
    /// <param>
    ///     ArmBehavior _arm: arm reference script
    /// </param>
    /// </summary>
    private void ExtendArmTap(ArmBehavior _arm)
    {
        if (!_arm.IsExtending && !_arm.IsExtended && !_arm.IsUnextending)
        {
            // FMOD Event
            OnExtendArm.Invoke();

            _arm.ExtensionTapStart();
            print("Arm Tap");
        }
    }


    /// <summary>
    ///     Function called when releasing the button of a held arm, depending on input
    /// <param>
    ///     ArmBehavior _arm: arm reference script
    /// </param>
    /// </summary>
    private void ExtendArmRelease(ArmBehavior _arm)
    {
        _arm.ExtensionHoldEnd();
        print("Arm Release");
    }

    // #endregion
}
