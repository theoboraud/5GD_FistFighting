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
    [System.NonSerialized] public Player Player;                                     // Player reference from which the arm extends

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

    private Rigidbody2D hitPlayer_RB;                       // Rigidbody reference of hit player (if any)

    private SpriteRenderer spriteRenderer;
    private Sprite armSprite;

    [Header("AirPush Variables")]
    public Sprite airPushSprite;
    public bool airPush_bool = false;

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
        if (armState == PlayerArmState.Extending)
        {
            Extend();
        }
        else if (armState == PlayerArmState.Unextending)
        {
            Unextend();
        }
    }

    // #endregion


    // #region =================== CONTROLS FUNCTIONS ==================

    public void Input_Extend(InputAction.CallbackContext _context)
    {
        if (_context.control.IsPressed())
        {
            armState = PlayerArmState.Extending;
        }
        else
        {
            armState = PlayerArmState.Unextending;
            //Debug.LogFormat("released {0} from {1} on {2}", _context.interaction.ToString(), this.GetInstanceID(), this.gameObject.transform.parent.name);
        }
    }
    
    // #endregion


    private void Extend()
    {
        print("Extend()");
        curScaleY = Mathf.Clamp(curScaleY + extendingTime * Time.deltaTime, 0, endScale);
        curScale.y = curScaleY;
        transform.localScale = curScale;
        //Air Push
        if (transform.localScale.y >= endScale && Player.PlayerPhysicState == PlayerPhysicState.OnAir)
        {
            if (!hitObjet_bool && !airPush_bool)
            {
                Player.RB.AddForce(this.transform.up * airPushForce * Player.AirPushFactor, ForceMode2D.Impulse);
                Player.AirPushFactor *= airPushForceLossFactor;
                spriteRenderer.sprite = airPushSprite;
                airPush_bool = true;
            }
        }
    }


    private void Unextend()
    {
        curScaleY = Mathf.Clamp(curScaleY - unextendingTime * Time.deltaTime, 0, endScale);
        curScale.y = curScaleY;
        transform.localScale = curScale;

        hitObjet_bool = false;
        airPush_bool = false;
        spriteRenderer.sprite = armSprite;
    }

    /// <summary>
    ///  When entering a collision with the ground of another player
    /// </summary>
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        GameObject _GO = _collision.gameObject;

        if (_GO.CompareTag("StaticGround") && !Player.HitObject_bool)
        {
            Player.HitObject_bool = true;
            OnCollision.Invoke();
            //Player.OnCollision.Invoke();
            Player.RB.AddForce(this.transform.up * groundForce, ForceMode2D.Impulse);
        }

        if (_GO.CompareTag("Player") && !Player.HitObject_bool)
        {
            hitPlayer_RB.AddForce(-this.transform.up * hitForce, ForceMode2D.Impulse);
            Player.HitObject_bool = true;
            hitPlayer_RB = _GO.GetComponent<Rigidbody2D>();
        }
    }
}
