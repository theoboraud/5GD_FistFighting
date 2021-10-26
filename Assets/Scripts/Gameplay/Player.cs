using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

/// <summary>
///     Class used to spawn the player arms during gameplay
/// </summary>
public class Player : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    [Header("Refs")]
    [System.NonSerialized] public Rigidbody2D RB;       // Player rigidbody ref
    public ArmBehaviourDoubleJump[] Arms = new ArmBehaviourDoubleJump[4];     // Array containing each arm

    [Header("Events")]
    public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
    public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

    // #endregion


    // #region ================ INIT CONTROLS FUNCTIONS ================

    /// <summary>
    ///     Init variables
    /// </summary>
    private void Awake()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("StaticGround"))
    //    {
    //        for (int i = 0; i < Arms.Length; i++)
    //        {
    //            Arms[i].CanAirPush = true;
    //        }
            
    //    }
    //}
    // #endregion
}
