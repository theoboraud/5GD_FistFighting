using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Enums;

/// <summary>
///     Class used by each arm for physics behaviours
/// </summary>

public class ArmBehaviour : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    public Player Player;                                               // Player reference from which the arm extends
    private SpriteRenderer spriteRenderer;                              // Sprite renderer reference of the arm

    [Header("Extension Parameters")]
    private float endScale = 2f;                                        //
    private float groundForce;                                          //
    private float hitForce;                                             //
    private float extendingTime;                                        //
    private float unextendingTime;                                      //
    private float cooldown;                                             //

    [Header("Air Control Parameters")]
    private float airPushForce;                                         //
    private float airPushForceLossFactor;                               //

    [Header("Extension Variables")]
    private PlayerArmState armState = PlayerArmState.Ready;             // Indicates whether or not the arm is extended at maximum
    private Vector3 curScale;                                           // Current scale of the arm
    private float curScaleY = 0;                                        // Current scale y of the arm
    private Sprite armSprite;                                           // Current sprite of the arm
    private bool buttonHold;                                            // Whether or not the extend button is being held
    private bool bool_hitStatic;                                        // Whether or not the arm has hit a static object
    private bool bool_hitPlayer;                                        // Whether or not the arm has hit another player
    private Player hitPlayer;                                           // Player reference of the avatar that is hit (if he's hit)
    private bool bool_hitDynamic;                                       // Whether or not the arm has hit a dynamic object
    private Rigidbody2D RB_dynamicEnvironment;                          // RB reference to the hit object's rigidbody

    [Header("Arm Behaviour Events")]
    public UnityEvent OnAppear;                                         //
    public UnityEvent OnExtended;                                       //
    public UnityEvent OnCollision;                                      //
    public UnityEvent OnUnextended;                                     //

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init all class variables
    /// </summary>
    private void Awake()
    {
        InitReferences();
        InitParameters();
        InitVariables();
    }


    /// <summary>
    ///     Init references
    /// </summary>
    private void InitReferences()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }


    /// <summary>
    ///     Init parameters
    /// </summary>
    private void InitParameters()
    {
        groundForce = GameManager.Instance.ParamData.PARAM_Player_ArmGroundForce;
        hitForce = GameManager.Instance.ParamData.PARAM_Player_ArmHitForce;
        extendingTime = GameManager.Instance.ParamData.PARAM_Player_ArmExtendingTime;
        unextendingTime = GameManager.Instance.ParamData.PARAM_Player_ArmUnextendingTime;
        cooldown = GameManager.Instance.ParamData.PARAM_Player_ArmCooldown;
        airPushForce = GameManager.Instance.ParamData.PARAM_Player_AirControlForce;
        airPushForceLossFactor = GameManager.Instance.ParamData.PARAM_Player_AirControlForceLossFactor;
    }


    /// <summary>
    ///     Init variables
    /// </summary>
    private void InitVariables()
    {
        armSprite = spriteRenderer.sprite;
        curScale = new Vector3(1f, curScaleY, 1f);
        transform.localScale = curScale;
        buttonHold = false;

        InitPhysicStateVariables();
    }


    /// <summary>
    ///     Init all physics states related variables
    /// </summary>
    private void InitPhysicStateVariables()
    {
        bool_hitStatic = false;

        bool_hitPlayer = false;
        hitPlayer = null;

        bool_hitDynamic = false;
        RB_dynamicEnvironment = null;
    }

    // #endregion



    // #region =================== CONTROLS FUNCTIONS ==================

    /// <summary>
    ///     Function called once the input to extend arm is pressed
    /// </summary>
    public void Input_StartExtend()
    {
        buttonHold = true;
    }


    /// <summary>
    ///     Function called once input to extend arm is released
    /// </summary>
    public void Input_StopExtend()
    {
        buttonHold = false;
    }

    // #endregion



    // #region =================== ARM EXTEND FUNCTIONS ==================

    /// <summary>
    ///     Each frame, check the extension state of the arm and adapt accordingly
    /// </summary>
    private  void Update()
    {
        if (Player.PlayerGameState == PlayerGameState.Alive)
        {
            if (buttonHold && armState == PlayerArmState.Ready)
            {
                armState = PlayerArmState.Extending;
            }

            if (Player.PlayerPhysicState != PlayerPhysicState.IsHit)
            {
                // If the arm is extending, it continues to extend
                if (armState == PlayerArmState.Extending)
                {
                    Extend();
                }
                // If the arm is completely extended and the button is not held, the arm starts to unextend
                else if (armState == PlayerArmState.Extended && !buttonHold)
                {
                    armState = PlayerArmState.Unextending;
                    Unextend();
                }
                // If the arm is unextending, it continues to unextend
                else if (armState == PlayerArmState.Unextending)
                {
                    Unextend();
                }
            }
            // If the player is hit, unextend the arm
            else
            {
                Unextend();
            }
        }
        // If the player is dead, unextend the arm
        else
        {
            Unextend();
        }
    }


    /// <summary>
    ///     Function called when extending the arm
    /// </summary>
    private void Extend()
    {
        // Continue the extension of the arm by increasing its scale
        curScaleY = Mathf.Clamp(curScaleY + extendingTime * Time.deltaTime, 0, endScale);
        curScale.y = curScaleY;
        transform.localScale = curScale;

        // If the arm extension is over
        if (transform.localScale.y >= endScale)
        {
            armState = PlayerArmState.Extended;

            // If the arm has hit a static object (i.e. the ground)
            if (bool_hitStatic)
            {
                // Apply ground force
                Player.RB.AddForce(this.transform.up * groundForce * Player.ForceIncreaseFactor, ForceMode2D.Impulse);

                // Reset the air push factor
                Player.AirPushFactor = 1f;

                // Feedbacks
                OnCollision.Invoke();
                Player.OnCollision.Invoke();
                GameManager.Instance.Feedback.SpawnHitVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90 + this.transform.rotation.eulerAngles.z, Vector3.forward));
            }

            // If the arm has hit another player
            else if (bool_hitPlayer)
            {
                print("HitPlayer");
                // Apply hit force to the hit player
                hitPlayer.RB.AddForce(-this.transform.up * hitForce * Player.ForceIncreaseFactor, ForceMode2D.Impulse);
                hitPlayer.Hit();

                // Feedbacks
                AudioManager.audioManager.PlayTrack("event:/Voices/Victory", hitPlayer.transform.position);
                GameManager.Instance.Feedback.SpawnHitVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90 + this.transform.rotation.eulerAngles.z, Vector3.forward));

                //Player.HitObject_bool = true;
            }

            // If the arm has hit a dynamic object
            else if (bool_hitDynamic)
            {
                // Apply hit force to the hit object
                RB_dynamicEnvironment.AddForce(-this.transform.up * hitForce * Player.ForceIncreaseFactor, ForceMode2D.Impulse);

                // Feedbacks
                GameManager.Instance.Feedback.SpawnHitVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90 + this.transform.rotation.eulerAngles.z, Vector3.forward));
            }

            // If the arm hasn't hit anything and the HoldTrigger is not being held
            else if (!Player.HoldingTrigger)
            {
                // Apply air force with air push factor, then update the air push factor
                Player.RB.AddForce(this.transform.up * airPushForce * Player.AirPushFactor * Player.ForceIncreaseFactor, ForceMode2D.Impulse);
                if (Player.PlayerPhysicState == PlayerPhysicState.InAir)
                {
                    Player.AirPushFactor *= airPushForceLossFactor;
                }

                // Feedbacks
                GameManager.Instance.Feedback.SpawnHitAvatarVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90+this.transform.rotation.eulerAngles.z, Vector3.forward));
            }
        }
    }


    /// <summary>
    ///     Function called when unextending the arm
    /// </summary>
    private void Unextend()
    {
        curScaleY = Mathf.Clamp(curScaleY - unextendingTime * Time.deltaTime, 0, endScale);
        curScale.y = curScaleY;
        transform.localScale = curScale;

        if(curScaleY <= 0)
        {
            //Debug.Log("I'm ready baby!!!");
            armState = PlayerArmState.Ready;
            // Reset all physics states related variables
            InitPhysicStateVariables();
        }
    }


    /// <summary>
    ///     When entering a collision with the ground of another player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        GameObject _GO = _collision.gameObject;

        if (_GO.CompareTag("StaticGround"))
        {
            bool_hitStatic = true;
        }

        if (_GO.CompareTag("Player"))
        {
            hitPlayer = _GO.GetComponent<Player>();
            bool_hitPlayer = true;
        }

        if(_GO.CompareTag("DynamicEnvironment"))
        {
            bool_hitDynamic = true;
            RB_dynamicEnvironment = _GO.GetComponent<Rigidbody2D>();
        }
    }


    /// <summary>
    ///     When exiting a collision with the ground
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject _GO = collision.gameObject;

        if (_GO.CompareTag("StaticGround"))
        {
            bool_hitStatic = false;
        }
    }

    // #endregion
}
