using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;
using Enums;

/// <summary>
///     Class used to spawn the player arms during gameplay
/// </summary>
public class Player : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public Rigidbody2D RB;       // Player rigidbody ref
    public ArmBehaviour[] Arms = new ArmBehaviour[4];     // Array containing each arm
    //public ArmBehaviourDegressif[] ArmsDeg = new ArmBehaviourDegressif[4];
    [System.NonSerialized] public CharacterSkin CharSkin;
    [SerializeField] private SpriteRenderer Face_SpriteRenderer;
    [SerializeField] private SpriteRenderer[] Arms_SpriteRenderers;

    [Header("Events for FMOD")]
    public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
    public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

    [Header("Variables")]
    [System.NonSerialized] public PlayerGameState PlayerGameState;
    [System.NonSerialized] public PlayerPhysicState PlayerPhysicState;
    [System.NonSerialized] public float AirPushFactor = 1f;
    [System.NonSerialized] public bool HitObject_bool = false;

    [System.NonSerialized] public float StunRecoveryTime;

    [System.NonSerialized] public float StunTimer;

    // #endregion


    // #region ===================== INIT FUNCTIONS ====================

    /// <summary>
    ///     Init variables
    /// </summary>
    private void Awake()
    {
        InitReferences();
        InitVariables();
        InitParameters();
        AddToPlayersManager();
    }


    private void InitReferences()
    {
        RB = this.GetComponent<Rigidbody2D>();
        for (int i = 0; i < Arms.Length; i++)
        {
            Arms[i].Player = this;
        }
    }


    private void InitVariables()
    {
        PlayerGameState = PlayerGameState.NotReady;
        PlayerPhysicState = PlayerPhysicState.InAir;
    }


    private void InitParameters()
    {
        RB.mass = GameManager.Instance.ParamData.PARAM_Player_Mass;
        RB.gravityScale = GameManager.Instance.ParamData.PARAM_Player_GravityScale;
        RB.drag = GameManager.Instance.ParamData.PARAM_Player_LinearDrag;
        RB.angularDrag = GameManager.Instance.ParamData.PARAM_Player_AngularDrag;
        StunRecoveryTime = GameManager.Instance.ParamData.PARAM_Player_StunRecoveryTime;
    }


    private void InitSkin()
    {
        print(CharSkin);
        Face_SpriteRenderer.sprite = CharSkin.SpriteFace;
        for (int i = 0; i < Arms_SpriteRenderers.Length; i++)
        {
            Arms_SpriteRenderers[i].sprite = CharSkin.SpriteArm;
        }
    }


    private void AddToPlayersManager()
    {
        print("Adding to player manager");
        PlayersManager.Instance.AddPlayer(this);
    }

    // #endregion


    // #region ===================== SKIN FUNCTIONS ====================

    public void ChangeSkin(CharacterSkin _charSkin)
    {
        this.CharSkin = _charSkin;
        InitSkin();
    }

    // #endregion



    public void Kill()
    {
        Face_SpriteRenderer.enabled = false;

        PlayerGameState = PlayerGameState.Dead;
    }

    public void Spawn()
    {
        Face_SpriteRenderer.enabled = true;

        PlayerGameState = PlayerGameState.Alive;
    }

    public void Hit()
    {
        PlayerPhysicState = PlayerPhysicState.isHit;
        StunTimer = 0;
    }


    private void OnCollisionEnter2D(Collision2D _collision)
    {
        GameObject _GO = _collision.gameObject;

        if(PlayerPhysicState != PlayerPhysicState.isHit)
        {
            if (_GO.CompareTag("StaticGround") && !HitObject_bool)
            {
                HitObject_bool = true;
                PlayerPhysicState = PlayerPhysicState.OnGround;
                // Reset air push factor
                AirPushFactor = 1f;
            }
        }

        if (_GO.CompareTag("Arrival"))
        {
            GameManager.Instance.EndOfRound(this);
        }

        if (_GO.CompareTag("Lethal"))
        {
            PlayersManager.Instance.KillPlayer(this);
        }
    }


    private void OnCollisionExit2D(Collision2D _collision)
    {
        GameObject _GO = _collision.gameObject;

        if(PlayerPhysicState != PlayerPhysicState.isHit)
        {
            if (_GO.CompareTag("StaticGround") || _GO.CompareTag("Arrival"))
            {
                HitObject_bool = false;
                PlayerPhysicState = PlayerPhysicState.InAir;
            }
        }
    }
}
