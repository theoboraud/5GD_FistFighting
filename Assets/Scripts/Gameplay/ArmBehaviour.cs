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

    [Header("Stats")]
    float forceCoef_groundExtension = 1f;  // Push force coefficient for physics interactions with the ground
    float forceCoef_airPush = 1f;          // Push force coefficient for physics interactions in the air
    float forceCoef_playerHit = 1f;        // Push force coefficient inflicted to a hit player

    [Header("Refs")]
    public Player Player;                                     // Player reference from which the arm extends

    [Header("Extension Parameters")]
    private float endScale = 2f;            //
    private float extendingTime;            //
    private float unextendingTime;
    private float groundForce;
    private float hitForce;
    private float cooldown;

    [Header("Air Control Parameters")]
    private float airPushForce;
    private float airPushForceLossFactor;

    [Header("Extension Variables")]
    private PlayerArmState armState = PlayerArmState.Ready;                  // Indicates whether or not the arm is extended at maximum

    [Header("Arm Behaviour Events")]
    public UnityEvent OnAppear;                             //
    public UnityEvent OnExtended;                           //
    public UnityEvent OnCollision;                          //
    public UnityEvent OnUnextended;                         //

    private float curScaleY = 0;                                 //

    private Vector3 curScale;

    private bool hitObjet_bool = false;                         // Indicates whether or not the player is hitting the ground
   /* private bool hitPlayer_bool = false;   */                      // Indicates whether or note the player is hitting another player

    private Player hitPlayer;                       // Player reference of the avatar that is hit (if he's hit)

    private SpriteRenderer spriteRenderer;
    private Sprite armSprite;

    [Header("AirPush Variables")]
    public Sprite airPushSprite;
    public bool airPush_bool = false;

    private bool InputOff;
    private bool Force = true;
    private bool HitObject;
    private bool bool_HitPlayer;
    private bool bool_HitDynamic;
    private Rigidbody2D DynamicRB;

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init variables
    /// </summary>
    private void Awake()
    {
        Init();
        InitParameters();
    }


    private void Init()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        armSprite = spriteRenderer.sprite;
        curScale = new Vector3(1f, curScaleY, 1f);
        transform.localScale = curScale;
    }


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


    private  void Update()
    {
        if (armState == PlayerArmState.Extending && Player.PlayerPhysicState != PlayerPhysicState.isHit)
        {
            Extend();
        }
        else if (InputOff && armState == PlayerArmState.Extended && Player.PlayerPhysicState != PlayerPhysicState.isHit)
        {
            armState = PlayerArmState.Unextending;
            InputOff = false;
        }
        else if (armState == PlayerArmState.Unextending || Player.PlayerPhysicState == PlayerPhysicState.isHit)
        {
            Unextend();
        }
    }

    // #endregion


    // #region =================== CONTROLS FUNCTIONS ==================

    //Function called once input to extend arm is pressed
    public void Input_Extend(InputAction.CallbackContext _context)
    {
        //Check if player is hit.
        //Player arms cannot extend if in StunState
        if(Player.PlayerPhysicState != PlayerPhysicState.isHit)
        {
            if (_context.started && armState == PlayerArmState.Ready)
            {
                armState = PlayerArmState.Extending;
            }
            else if (!_context.control.IsPressed())
            {
                InputOff = true;
            }
        }
    }

    // #endregion


    private void Extend()
    {
        curScaleY = Mathf.Clamp(curScaleY + extendingTime * Time.deltaTime, 0, endScale);
        curScale.y = curScaleY;
        transform.localScale = curScale;

        //Air Push
        //Check if Arm is Extended
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
                Debug.Log("HERE WE GO!!!!!");
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
                DynamicRB.AddForce(-this.transform.up * hitForce * Player.ForceIncreaseFactor, ForceMode2D.Impulse);
                GameManager.Instance.Feedback.SpawnHitVFX(this.transform.position + this.transform.up * -1.8f, Quaternion.AngleAxis(90 + this.transform.rotation.eulerAngles.z, Vector3.forward));
                Player.HitObject_bool = true;
                bool_HitDynamic = false;
                DynamicRB = null;
            }
            Invoke("TurnForceOff", 0.2f);
        }
    }

    private void TurnForceOff()
    {
        Force = false;
    }

    private void Unextend()
    {
        curScaleY = Mathf.Clamp(curScaleY - unextendingTime * Time.deltaTime, 0, endScale);
        curScale.y = curScaleY;
        transform.localScale = curScale;

        hitObjet_bool = false;
        airPush_bool = false;

        if(curScaleY <= 0)
        {
            Debug.Log("I'm ready baby!!!");
            armState = PlayerArmState.Ready;
            ReinitializeAllBools();
        }
    }

    private void ReinitializeAllBools()
    {
        InputOff = false;
        Force = true;
        hitObjet_bool = false;
        bool_HitPlayer = false;
        hitPlayer = null;
        bool_HitDynamic = false;
        DynamicRB = null;
    }

    /// <summary>
    ///  When entering a collision with the ground of another player
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
            DynamicRB = _GO.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject _GO = collision.gameObject;

        if (_GO.CompareTag("StaticGround"))
        {
            HitObject = false;
        }
    }
}
