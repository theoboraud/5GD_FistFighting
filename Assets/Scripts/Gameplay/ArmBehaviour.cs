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
    private float impulseForce = 50f;
    float forceCoef_groundExtension = 1f;  // Push force coefficient for physics interactions with the ground
    float forceCoef_airPush = 1f;          // Push force coefficient for physics interactions in the air
    float forceCoef_playerHit = 1f;        // Push force coefficient inflicted to a hit player

    [Header("Refs")]
    public Player Face;                                     // Face reference from which the arm extends

    [Header("Extension Variables")]
    private float EndScale = 2f;            //
    private float speedExtension=10f;                  //
    public bool IsExtending = false;                             //
    public bool IsUnextending = false;
    public bool IsExtended = false;                         // Indicates whether or not the arm is extended at maximum

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
    public float coeffAirPush = 1;
    public bool CanAirPush;

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init variables
    /// </summary>
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        armSprite = spriteRenderer.sprite;
        curScale = new Vector3(1, curScaleY, 1);
        transform.localScale = curScale;
    }
    // #endregion

    private  void Update()
    {
        if (IsExtended)
        {
            Extending();
        }
        else
        {
            UnExtended();
        }
    }
    // #region =================== CONTROLS FUNCTIONS ==================
    public void Input_Extend(InputAction.CallbackContext _context)
    {
        if (_context.control.IsPressed())
        {
            IsExtended = true;
        }
        else
        {
            IsExtended = false;
            //Debug.LogFormat("released {0} from {1} on {2}", _context.interaction.ToString(), this.GetInstanceID(), this.gameObject.transform.parent.name);
        }
    }
    //// #endregion

    private void Extending()
    {
        
        curScaleY = Mathf.Clamp(curScaleY+speedExtension * Time.deltaTime, 0, EndScale);
        curScale.y = curScaleY;
        transform.localScale = curScale;
        //Air Push
        if (transform.localScale.y >= EndScale)
        {
            if (!hitObjet_bool&& !airPush_bool)
            {
                Face.RB.AddForce(this.transform.up * impulseForce * forceCoef_airPush * coeffAirPush, ForceMode2D.Impulse);
                coeffAirPush /= 3;
                spriteRenderer.sprite = airPushSprite;
                airPush_bool = true;
            }
        }
    }
    private void UnExtended()
    {
        curScaleY = Mathf.Clamp(curScaleY-speedExtension * Time.deltaTime, 0, EndScale);
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

        if (_GO.CompareTag("StaticGround") )
        {
            hitObjet_bool = true;
            OnCollision.Invoke();
            Face.OnCollision.Invoke();
            Face.RB.AddForce(this.transform.up * impulseForce * forceCoef_groundExtension, ForceMode2D.Impulse);
            coeffAirPush = 1;
        }

        if (_GO.CompareTag("Player") )
        {
            hitPlayer_RB.AddForce(-this.transform.up * impulseForce * forceCoef_playerHit, ForceMode2D.Impulse);
            hitObjet_bool = true;
            hitPlayer_RB = _GO.GetComponent<Rigidbody2D>();
        }
    }
}
