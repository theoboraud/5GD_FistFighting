using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    // #endregion



    // #region ================ ARM EXTENSION FUNCTIONS ================

    /// <summary>
    ///     Function called when extending a given arm, depending on input
    /// <param>
    ///     int _arm: arm index
    /// </param>
    /// </summary>
    public void ExtendArm(int _arm)
    {
        OnExtendArm.Invoke();
        CreateArm(_arm);
    }


    /// <summary>
    ///     Function used to extend the given arm
    /// <param>
    ///     int _arm: arm index
    /// </param>
    /// </summary>
    private void CreateArm(int _arm)
    {
        if(Arms[_arm].active == false)
        {
            Arms[_arm].ExtensionStart();
        }
    }

    // #endregion
}
