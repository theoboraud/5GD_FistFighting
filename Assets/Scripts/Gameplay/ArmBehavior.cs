using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///     Class used by each arm for physics behaviours
/// </summary>
public class ArmBehavior : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("Stats")]
    [SerializeField] float impulseForce = 10f;
    [SerializeField] float forceCoef_groundExtension = 1f;  // Push force coefficient for physics interactions with the ground
    [SerializeField] float forceCoef_airPush = 1f;          // Push force coefficient for physics interactions in the air
    [SerializeField] float forceCoef_playerHit = 1f;        // Push force coefficient inflicted to a hit player

    [Header("Refs")]
    public PlayerArmExtender Face;                          // Face reference from which the arm extends

    [Header("Extension Variables")]
    [SerializeField] Vector2 StartScaleEndScale;            //
    [SerializeField] float speedExtension;                  //
    [SerializeField] float speedUnextension;                //
    public bool active = false;                             //

    [Header("Arm Behaviour Events")]
    public UnityEvent OnAppear;                             //
    public UnityEvent OnExtended;                           //
    public UnityEvent OnCollision;                          //
    public UnityEvent OnUnextended;                         //

    private float curScale;                                 //

    private bool isExtended = false;                        // Indicates whether or not the arm is extended at maximum

    private bool hitGround = false;                         // Indicates whether or not the player is hitting the ground
    private bool hitPlayer = false;                         // Indicates whether or note the player is hitting another player
    private Rigidbody2D hitPlayer_RB;                       // Rigidbody reference of hit player (if any)

    private SpriteRenderer spriteRenderer;
    private Sprite armSprite;

    [Header("AirPush Variables")]
    public Sprite airPushSprite;
    private bool airPushing = false;
    [SerializeField] float cooldownAirPush = 0f;
    [SerializeField] float airPushAnimationTime = 0f;

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        armSprite = spriteRenderer.sprite;
    }


    /// <summary>
    ///     Modify every frame the arm physic (scale, position...) depending on whether or not it is active
    /// </summary>
    private void Update()
    {
        if(active)
        {
            if (!isExtended)
            {
                UpdateScale();
            }
            else
            {
                UnextendArm();
            }
        }
    }

    // #endregion



    // #region ================ ARM EXTENSION FUNCTIONS ================

    /// <summary>
    ///     Start the extension by activating it
    /// </summary>
    public void ExtensionStart()
    {
        active = true;
        isExtended = false;
    }


    /// <summary>
    ///
    /// </summary>
    private void UpdateScale()
    {
        if(curScale >= StartScaleEndScale.y)
        {
            // FMOD event
            OnExtended.Invoke();

            // If hitting the ground, use force impulsion to move to the opposite side
            if (hitGround)
            {
                Face.rb.AddForce(this.transform.up * impulseForce * forceCoef_groundExtension, ForceMode2D.Impulse);
                hitGround = false;
            }
            else if (!airPushing)
            {
                spriteRenderer.sprite = airPushSprite;
                Face.rb.AddForce(this.transform.up * impulseForce * forceCoef_airPush, ForceMode2D.Impulse);
                airPushing = true;
                Invoke("ResetArmSprite", airPushAnimationTime);
                Invoke("ResetAirPush", cooldownAirPush);
            }

            // If hitting a player, that player will receive a force impulsion
            if (hitPlayer)
            {
                if (hitPlayer_RB != null)
                {
                    hitPlayer_RB.AddForce(-this.transform.up * impulseForce * forceCoef_playerHit, ForceMode2D.Impulse);
                    hitPlayer = false;
                    hitPlayer_RB = null;
                }
            }

            isExtended = true;
        }
        this.transform.localScale = new Vector3(1f, curScale, 1f);

        curScale += speedExtension * Time.deltaTime;
    }


    /// <summary>
    ///
    /// </summary>
    private void UnextendArm()
    {
        if(curScale <= StartScaleEndScale.x)
        {
            OnStopExtension();
        }

        this.transform.localScale = new Vector3(1f, curScale, 1f);

        curScale -= speedUnextension * Time.deltaTime;
    }


    /// <summary>
    ///     When the arm retracts, set active to false for this arm
    /// </summary>
    private void OnStopExtension()
    {
        active = false;
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
            hitGround = true;
            OnCollision.Invoke();
            Face.OnCollision.Invoke();
        }

        if(_GO.CompareTag("Player"))
        {
            hitPlayer = true;
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
            hitGround = false;
        }
        else if (_GO.CompareTag("Player"))
        {
            hitPlayer = false;
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
        airPushing = false;
    }

    // #endregion
}
