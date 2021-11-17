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
    private bool hitObjet_bool = false;                                 // Indicates whether or not the player is hitting the ground
   /* private bool hitPlayer_bool = false;   */                         // Indicates whether or note the player is hitting another player
    private Player hitPlayer;                                           // Player reference of the avatar that is hit (if he's hit)
    private Rigidbody2D RB_dynamicEnvironment;                          // RB reference to the hit object's rigidbody
    private Sprite armSprite;                                           // Current sprite of the arm
    private bool buttonHold;                                            //
    private bool Force = true;                                          //
    private bool HitObject;                                             //
    private bool bool_HitPlayer;                                        //
    private bool bool_HitDynamic;                                       //

    [Header("Arm Behaviour Events")]
    public UnityEvent OnAppear;                                         //
    public UnityEvent OnExtended;                                       //
    public UnityEvent OnCollision;                                      //
    public UnityEvent OnUnextended;                                     //

    [Header("AirPush Variables")]
    public Sprite airPushSprite;                                        //
    public bool airPush_bool = false;                                   //




    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init class variables
    /// </summary>
    private void Awake()
    {
        InitReferences();
        InitParameters();
        InitVariables();
    }


    /// <summary>
    ///
    /// </summary>
    private void InitReferences()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }


    /// <summary>
    ///
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
    ///
    /// </summary>
    private void InitVariables()
    {
        armSprite = spriteRenderer.sprite;
        curScale = new Vector3(1f, curScaleY, 1f);
        transform.localScale = curScale;
        buttonHold = false;
    }

    // #endregion



    // #region =================== CONTROLS FUNCTIONS ==================

    /// <summary>
    ///     Function called once the input to extend arm is pressed
    /// </summary>
    public void Input_StartExtend()
    {
        //Check if player is hit.
        //Player arms cannot extend if in StunState
        if (Player.PlayerGameState == PlayerGameState.Alive && Player.PlayerPhysicState != PlayerPhysicState.isHit)
        {
            buttonHold = true;
            if (armState == PlayerArmState.Ready)
            {
                armState = PlayerArmState.Extending;
            }
        }
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
        if (Player.PlayerPhysicState != PlayerPhysicState.isHit)
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
            Force = true;
            armState = PlayerArmState.Extended;

            //In the case that the player hasn't hit anything
            if (!hitObjet_bool && !airPush_bool && !bool_HitPlayer && !HitObject && !Player.HoldingTrigger)
            {
                Player.RB.AddForce(this.transform.up * airPushForce * Player.AirPushFactor * Player.ForceIncreaseFactor, ForceMode2D.Impulse);
                if (Player.PlayerPhysicState == PlayerPhysicState.InAir)
                {
                    Player.AirPushFactor *= airPushForceLossFactor;
                }
                airPush_bool = true;
                GameManager.Instance.Feedback.SpawnHitAvatarVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90+this.transform.rotation.eulerAngles.z, Vector3.forward));
            }

            //In the case that the player has hit a static object
            else if (HitObject && Force)
            {
                OnCollision.Invoke();
                Player.OnCollision.Invoke();
                Player.RB.AddForce(this.transform.up * groundForce * Player.ForceIncreaseFactor, ForceMode2D.Impulse);
                Player.HitObject_bool = true;
                HitObject = false;
                //Debug.Log("HERE WE GO!!!!!");
                GameManager.Instance.Feedback.SpawnHitVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90 + this.transform.rotation.eulerAngles.z, Vector3.forward));
            }

            //In the case that the player has hit another Player
            else if (bool_HitPlayer && Force)
            {
                hitPlayer.RB.AddForce(-this.transform.up * hitForce * Player.ForceIncreaseFactor, ForceMode2D.Impulse);
                AudioManager.audioManager.PlayTrack("event:/Voices/Victory", hitPlayer.transform.position);
                GameManager.Instance.Feedback.SpawnHitVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90 + this.transform.rotation.eulerAngles.z, Vector3.forward));
                Player.HitObject_bool = true;
                hitPlayer.Hit();
                bool_HitPlayer = false;
                hitPlayer = null;
            }

            //In the case that the player hits a dynamic object
            else if (bool_HitDynamic && Force)
            {
                RB_dynamicEnvironment.AddForce(-this.transform.up * hitForce * Player.ForceIncreaseFactor, ForceMode2D.Impulse);
                GameManager.Instance.Feedback.SpawnHitVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90 + this.transform.rotation.eulerAngles.z, Vector3.forward));
                Player.HitObject_bool = true;
                bool_HitDynamic = false;
                RB_dynamicEnvironment = null;
            }
            Invoke("TurnForceOff", 0.2f);
        }
    }


    /// <summary>
    ///
    /// </summary>
    private void TurnForceOff()
    {
        Force = false;
    }


    /// <summary>
    ///     Function called when unextending the arm
    /// </summary>
    private void Unextend()
    {
        curScaleY = Mathf.Clamp(curScaleY - unextendingTime * Time.deltaTime, 0, endScale);
        curScale.y = curScaleY;
        transform.localScale = curScale;

        hitObjet_bool = false;
        airPush_bool = false;

        if(curScaleY <= 0)
        {
            //Debug.Log("I'm ready baby!!!");
            armState = PlayerArmState.Ready;
            ReinitializeAllBools();
        }
    }


    /// <summary>
    ///     Reinitialize all variables related to extension, once extension is over
    /// </summary>
    private void ReinitializeAllBools()
    {
        Force = true;
        hitObjet_bool = false;
        bool_HitPlayer = false;
        hitPlayer = null;
        bool_HitDynamic = false;
        RB_dynamicEnvironment = null;
    }


    /// <summary>
    ///     When entering a collision with the ground of another player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        GameObject _GO = _collision.gameObject;

        if (_GO.CompareTag("StaticGround"))
        {
            HitObject = true;
        }

        if (_GO.CompareTag("Player"))
        {
            hitPlayer = _GO.GetComponent<Player>();
            bool_HitPlayer = true;
        }

        if(_GO.CompareTag("DynamicEnvironment"))
        {
            bool_HitDynamic = true;
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
            HitObject = false;
        }
    }

    // #endregion
}
