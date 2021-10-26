using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

/// <summary>
///     Class used by each arm for physics behaviours
/// </summary>
public class ArmBehaviour : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("Stats")]
    [SerializeField] float impulseForce = 10f;
    [SerializeField] float forceCoef_groundExtension = 1f;  // Push force coefficient for physics interactions with the ground
    [SerializeField] float forceCoef_airPush = 1f;          // Push force coefficient for physics interactions in the air
    [SerializeField] float forceCoef_playerHit = 1f;        // Push force coefficient inflicted to a hit player

    [Header("Refs")]
    public Player Face;                                     // Face reference from which the arm extends

    [Header("Extension Variables")]
    [SerializeField] private Vector2 StartScaleEndScale;            //
    [SerializeField] private float speedExtension;                  //
    [SerializeField] private float speedUnextension;                //
    public bool IsExtending = false;                             //
    public bool IsUnextending = false;
    public bool IsExtended = false;                         // Indicates whether or not the arm is extended at maximum

    [Header("Arm Behaviour Events")]
    public UnityEvent OnAppear;                             //
    public UnityEvent OnExtended;                           //
    public UnityEvent OnCollision;                          //
    public UnityEvent OnUnextended;                         //

    private float curScale;                                 //

    private bool hitGround_bool = false;                         // Indicates whether or not the player is hitting the ground
    private bool hitPlayer_bool = false;                         // Indicates whether or note the player is hitting another player
    private Rigidbody2D hitPlayer_RB;                       // Rigidbody reference of hit player (if any)

    private SpriteRenderer spriteRenderer;
    private Sprite armSprite;

    [Header("AirPush Variables")]
    public Sprite airPushSprite;
    private bool airPush_bool = false;
    [SerializeField] float cooldownAirPush = 0f;
    [SerializeField] float airPushAnimationTime = 0f;

    public bool IsGrabbing = false;

    public bool IsHolding = false;


    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init variables
    /// </summary>
    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        armSprite = spriteRenderer.sprite;
    }


    /// <summary>
    ///     Modify every frame the arm physic (scale, position...) depending on whether or not it is extending
    /// </summary>
    private void Update()
    {
        if (IsExtending && !IsExtended)
        {
            Extend();
        }
        else if (IsUnextending && IsExtended)
        {
            Unextend();
        }
    }

    // #endregion


    // #region =================== CONTROLS FUNCTIONS ==================

    public void Input_Extend(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            if (_context.interaction is HoldInteraction)
            {
                ExtensionHoldStart();

                // FMOD Event
                Face.OnExtendArm.Invoke();
            }
            else if (_context.interaction is TapInteraction)
            {
                ExtensionTapStart();

                // FMOD Event
                Face.OnExtendArm.Invoke();
            }
        }
        else if (_context.canceled)
        {
            if (_context.interaction is HoldInteraction)
            {
                ExtensionHoldEnd();
            }
        }
    }

    // #endregion


    // #region ================ ARM EXTENSION FUNCTIONS ================

    /// <summary>
    ///     Start the extension by activating it with a button tap
    /// </summary>
    public void ExtensionTapStart()
    {
        if (CanExtend())
        {
            IsExtending = true;
            IsHolding = false;
            print("Arm Tap");
        }
    }


    /// <summary>
    ///     Start the extension by activating it with a button hold
    /// </summary>
    public void ExtensionHoldStart()
    {
        if (CanExtend())
        {
            IsExtending = true;
            IsHolding = true;
            print("Arm Held");
        }
    }


    /// <summary>
    ///     End the extension by releasing a button hold
    /// </summary>
    public void ExtensionHoldEnd()
    {
        IsUnextending = true;
        IsHolding = false;
        print("Arm Release");
    }


    private bool CanExtend()
    {
        return !IsExtending && !IsUnextending && !IsHolding;
    }


    /// <summary>
    ///
    /// </summary>
    public void Extend()
    {
        if (curScale >= StartScaleEndScale.y)
        {
            // FMOD event
            OnExtended.Invoke();

            // If hitting the ground, use force impulsion to move to the opposite side
            if (hitGround_bool)
            {
                Face.RB.AddForce(this.transform.up * impulseForce * forceCoef_groundExtension, ForceMode2D.Impulse);
                hitGround_bool = false;
            }
            else if (!airPush_bool)
            {
                spriteRenderer.sprite = airPushSprite;
                Face.RB.AddForce(this.transform.up * impulseForce * forceCoef_airPush, ForceMode2D.Impulse);
                airPush_bool = true;
                Invoke("ResetArmSprite", airPushAnimationTime);
                Invoke("ResetAirPush", cooldownAirPush);
            }

            // If hitting a player, that player will receive a force impulsion
            if (hitPlayer_bool)
            {
                if (hitPlayer_RB != null)
                {
                    hitPlayer_RB.AddForce(-this.transform.up * impulseForce * forceCoef_playerHit, ForceMode2D.Impulse);
                    hitPlayer_bool = false;
                    hitPlayer_RB = null;
                }
            }

            IsExtending = false;
            IsUnextending = false;

            IsExtended = true;

            if (!IsHolding)
            {
                IsUnextending = true;
            }
        }

        this.transform.localScale = new Vector3(1f, curScale, 1f);

        curScale += speedExtension * Time.deltaTime;
    }


    /// <summary>
    ///
    /// </summary>
    public void Unextend()
    {
        IsExtending = false;

        if (curScale <= StartScaleEndScale.x)
        {
            OnStopExtension();
            IsExtended = false;
            IsExtending = false;
            IsUnextending = false;
            IsHolding = false;
        }

        this.transform.localScale = new Vector3(1f, curScale, 1f);

        curScale -= speedUnextension * Time.deltaTime;
    }


    /// <summary>
    ///     When the arm is completely retracted, set IsExtending to false for this arm
    /// </summary>
    private void OnStopExtension()
    {
        // FMOD event
        OnUnextended.Invoke();
    }


    /// <summary>
    ///     When entering a collision with the ground of another player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        GameObject _GO = _collision.gameObject;

        if (_GO.CompareTag("StaticGround"))
        {
            hitGround_bool = true;
            OnCollision.Invoke();
            Face.OnCollision.Invoke();
        }

        if(_GO.CompareTag("Player"))
        {
            hitPlayer_bool = true;
            hitPlayer_RB = _GO.GetComponent<Rigidbody2D>();
        }
    }


    /// <summary>
    ///     Change the variable hitGround to false if leaving a collision with the ground, and hitPlayer to false if leaving a collision with a player
    /// </summary>
    private void OnTriggerExit2D(Collider2D _collision)
    {
        GameObject _GO = _collision.gameObject;
        if (_GO.CompareTag("StaticGround"))
        {
            hitGround_bool = false;
        }
        else if (_GO.CompareTag("Player"))
        {
            hitPlayer_bool = false;
        }
    }

    /// <summary>
    ///     Reset arm sprite to default, after using an AirPush
    /// </summary>
    private void ResetArmSprite()
    {
        spriteRenderer.sprite = armSprite;
    }

    /// <summary>
    ///     Reset boolean indicating whether or not the player has used AirPush at the end of a cooldown
    /// </summary>
    private void ResetAirPush()
    {
        airPush_bool = false;
    }

    // #endregion


    // #region ================== ARM GRAB FUNCTIONS ===================
    /*
    /// <summary>
    ///
    /// </summary>
    public void StartGrab()
    {
        if (!IsGrabbing)
        {
            IsGrabbing = true;
        }
    }


    /// <summary>
    ///
    /// </summary>
    public void EndGrab()
    {
        if (IsGrabbing)
        {
            IsGrabbing = false;
        }
    }
    */

    // #endregion
}
