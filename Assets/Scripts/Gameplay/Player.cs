using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;
using UnityEngine.UI;
using Enums;

/// <summary>
///     Class used to spawn the player arms during gameplay
/// </summary>
public class Player : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public Rigidbody2D RB;       // Player rigidbody ref
    public ArmBehaviour[] Arms = new ArmBehaviour[4];     // Array containing each arm behaviour script
    [System.NonSerialized] public CharacterSkin CharSkin;
    public PlayerArmController PlayerArmController;
    [SerializeField] private SpriteRenderer Face_SpriteRenderer;
    [SerializeField] private SpriteRenderer[] Arms_SpriteRenderers;
    [SerializeField] private GameObject playerIndicator;
    [SerializeField] private GameObject UI_PlayerIndicator;
    [SerializeField] private BoxCollider2D BoxCollider;

    [Header("Events for FMOD")]
    public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
    public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

    [Header("Variables")]
    [System.NonSerialized] public PlayerGameState PlayerGameState;
    public PlayerPhysicState PlayerPhysicState;
    [System.NonSerialized] public PlayerRotateState PlayerRotateState;      // Contain the enum of the rotate state (Ready, RotatingRight, RotatingLeft, or OnCooldown)
    [System.NonSerialized] public float AirPushFactor = 1f;
    [System.NonSerialized] public bool HitObject_bool = false;
    [System.NonSerialized] public bool HoldingTrigger = false;

    [System.NonSerialized] public float StunRecoveryTime;
    [System.NonSerialized] public float StunTimer;
    [System.NonSerialized] public float ForceIncreaseFactor;

    // #endregion


    // #region ==================== INIT FUNCTIONS ====================

    /// <summary>
    ///     Init variables
    /// </summary>
    public void Awake()
    {
        // Keep the player game object between scenes
        DontDestroyOnLoad(gameObject);

        // Call init methods
        InitReferences();
        InitVariables();
        InitParameters();

        // Init player controls
        gameObject.GetComponent<PlayerControls>().Init();

        // Get a random skin at start -> TODO: Select skin
        ChangeSkin(PlayersManager.Instance.SkinsData.GetRandomSkin());

        // Init player color and add to PlayersManager Players references
        InitIndicatorColor();

        // Add player to the PlayersManager
        PlayersManager.Instance.AddPlayer(this);
    }


    /// <summary>
    ///
    /// </summary>
    private void InitReferences()
    {
        RB = this.GetComponent<Rigidbody2D>();
        /*for (int i = 0; i < Arms.Length; i++)
        {
            Arms[i].Player = this;
        }*/
    }


    /// <summary>
    ///
    /// </summary>
    private void InitVariables()
    {
        PlayerGameState = PlayerGameState.Alive;
        PlayerPhysicState = PlayerPhysicState.InAir;
    }


    /// <summary>
    ///
    /// </summary>
    private void InitParameters()
    {
        RB.mass = GameManager.Instance.ParamData.PARAM_Player_Mass;
        RB.gravityScale = GameManager.Instance.ParamData.PARAM_Player_GravityScale;
        RB.drag = GameManager.Instance.ParamData.PARAM_Player_LinearDrag;
        RB.angularDrag = GameManager.Instance.ParamData.PARAM_Player_AngularDrag;
        StunRecoveryTime = GameManager.Instance.ParamData.PARAM_Player_StunRecoveryTime;
    }


    /// <summary>
    ///
    /// </summary>
    private void InitSkin()
    {
        Face_SpriteRenderer.sprite = CharSkin.SpriteFace;
        /*for (int i = 0; i < Arms_SpriteRenderers.Length; i++)
        {
            Arms_SpriteRenderers[i].sprite = CharSkin.SpriteArm;
        }*/
    }


    /// <summary>
    ///
    /// </summary>
    public void InitIndicatorColor()
    {
        switch (PlayersManager.Instance.Players.Count)
        {
            case 0:
                playerIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;
                UI_PlayerIndicator.GetComponent<Image>().color = Color.yellow;
                break;
            case 1:
                playerIndicator.GetComponent<SpriteRenderer>().color = Color.blue;
                UI_PlayerIndicator.GetComponent<Image>().color = Color.blue;
                break;
            case 2:
                playerIndicator.GetComponent<SpriteRenderer>().color = Color.red;
                UI_PlayerIndicator.GetComponent<Image>().color = Color.red;
                break;
            case 3:
                playerIndicator.GetComponent<SpriteRenderer>().color = Color.green;
                UI_PlayerIndicator.GetComponent<Image>().color = Color.green;
                break;
            default:
                break;
        }
    }

    // #endregion


    // #region ==================== SKIN FUNCTIONS ====================

    /// <summary>
    ///
    /// </summary>
    public void ChangeSkin(CharacterSkin _charSkin)
    {
        this.CharSkin = _charSkin;
        InitSkin();
    }

    // #endregion


    // #region ==================== PLAYER FUNCTIONS ====================

    /// <summary>
    ///     Enable the player's sprite renderer and set its position to _targetPos
    /// </summary>
    public void Spawn(Vector3 _targetPos)
    {
        Face_SpriteRenderer.enabled = true;
        this.transform.position = _targetPos;
        // Reset rotation and velocity
        this.transform.rotation = Quaternion.identity;
        RB.velocity = new Vector2(0f, 0f);

        PlayerGameState = PlayerGameState.Alive;
    }


    /// <summary>
    ///     Disable the player's sprite renderer and set its position to somewhere far from the map (to change?)
    /// </summary>
    public void Kill()
    {
        GameManager.Instance.Feedback.ShakeCamera(0.5f, 0.7f);
        GameManager.Instance.Feedback.SpawnExpulsionVFX(this.transform.position);
        Face_SpriteRenderer.enabled = false;
        this.transform.position = new Vector3(1000, 1000, 0);

        PlayerGameState = PlayerGameState.Dead;
        PlayerArmController.Init();

        // Remove the player from the PlayersAlive reference in PlayersManager
        PlayersManager.Instance.KillPlayer(this);
    }


    /// <summary>
    ///     Stun the player
    /// </summary>
    public void Hit()
    {
        PlayerPhysicState = PlayerPhysicState.IsHit;
        StunTimer = 0;
    }


    /// <summary>
    ///     Check if hit a lethal object or an arrival
    /// </summary>
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        GameObject _GO = _collision.gameObject;

        if (_GO.CompareTag("Lethal"))
        {
            Kill();
        }

        if (_GO.CompareTag("Arrival"))
        {
            GameManager.Instance.EndOfRound(this);
        }
    }


    /// <summary>
    ///     Check if hit a StaticGround object from the bottom, with raycast
    /// </summary>
    private bool IsGrounded()
    {
        float extraDistance = 0.1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(BoxCollider.bounds.center, Vector2.down, BoxCollider.bounds.extents.y + extraDistance, LayerMask.GetMask("StaticGround"));

        // DEBUG TEST
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(BoxCollider.bounds.center, Vector2.down * (BoxCollider.bounds.extents.y + extraDistance),rayColor);

        if (raycastHit.collider != null)
        {
            return raycastHit.collider.gameObject.CompareTag("StaticGround");
        }
        return false;
    }


    /// <summary>
    ///     Set the player physic state to OnGround if hitting the ground, otherwise its InAir
    /// </summary>
    private void Update()
    {
        if (PlayerPhysicState != PlayerPhysicState.IsHit)
        {
            if (IsGrounded())
            {
                PlayerPhysicState = PlayerPhysicState.OnGround;
                AirPushFactor = 1f;

                //HitObject_bool = true;

            }
            else
            {
                //HitObject_bool = false;
                PlayerPhysicState = PlayerPhysicState.InAir;
            }
        }
    }

    // #endregion
}
