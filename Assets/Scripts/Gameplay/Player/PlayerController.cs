using System;
using Enums;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controller class for player, excute all player's input behaviours, and moving/physic states
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region EVENTS

    //public event CallBack onStunStart;
    //public event CallBack onStunStop;
    //public event CallBack onCollisionEnter;
    //public event CallBack onGround;
    //public event CallBack onAir;

    #endregion
    [Header("References")]
    private Rigidbody2D _rb;       // Player rigidbody ref
    private ArmController _armController;
    private Player _player;

    private BoxCollider2D _boxCollider;

    [Header("Events for FMOD")]
    //public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
    //public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

    [Header("Variables")]
    [System.NonSerialized] public float AirPushFactor = 1f;
    [System.NonSerialized] public bool HitObject_bool = false;
    [System.NonSerialized] public bool HoldingTrigger = false;

    [System.NonSerialized] public float ForceIncreaseFactor;

    private int skinIndex;                         // Contains the index of the current skin

    private void OnDestroy()
    {
        RemoveCallBacks();
    }

    public void Init()
    {
        _player = GetComponent<Player>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _armController = GetComponent<ArmController>();
        // Init parameters
        GlobalSettings.ApplyPhysicsSettings(_rb);

        InitCallBacks();
    }

    private void InitCallBacks()
    {
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerSpawn, Spawn);
        _player.EventCenter.Subscribe<PlayerGameState>(PlayerEvent.OnGameStateChange, OnPlayerGameStateChange);
    }

    private void RemoveCallBacks()
    {
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerSpawn, Spawn);
        _player.EventCenter.Unsubscribe<PlayerGameState>(PlayerEvent.OnGameStateChange, OnPlayerGameStateChange);
    }

    private void Spawn()
    {
        _player.PlayerStates.PlayerGameState = PlayerGameState.Alive;
        this.transform.rotation = Quaternion.identity;
        _rb.linearVelocity = new Vector2(0f, 0f);
        _rb.simulated = true;
    }

    private void OnPlayerGameStateChange(PlayerGameState _gameState)
    {
        if (_gameState == PlayerGameState.Dead)
        {
            Kill();
        }
    }
    private void Kill()
    {
        _rb.simulated = false;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = 0f;
    }



    /// <summary>
    ///     Check if hit a lethal object or an arrival
    /// </summary>
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        GameObject _GO = _collision.gameObject;

        if (_GO.CompareTag("Lethal"))
        {
            _player.Kill();
        }

        if (_GO.CompareTag("Arrival"))
        {
            GameManager.Instance.EndOfStage(_player);
        }

        _player.EventCenter.Invoke(PlayerEvent.OnPlayerCollisionEnter);
    }

    /// <summary>
    ///     Check if hit a StaticGround object from the bottom, with raycast
    /// </summary>
    private bool IsGrounded()
    {
        float extraDistance = 0.25f;
        RaycastHit2D raycastHit = Physics2D.Raycast(_boxCollider.bounds.center, Vector2.down, _boxCollider.bounds.extents.y + extraDistance, LayerMask.GetMask("StaticGround"));

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
        Debug.DrawRay(_boxCollider.bounds.center, Vector2.down * (_boxCollider.bounds.extents.y + extraDistance), rayColor);

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
        if (_player.PlayerStates.PlayerPhysicState != PlayerPhysicState.IsHit)
        {
            if (IsGrounded())
            {
                _player.PlayerStates.PlayerPhysicState = PlayerPhysicState.OnGround;
                AirPushFactor = 1f;
            }
            else
            {
                _player.PlayerStates.PlayerPhysicState = PlayerPhysicState.InAir;
            }
        }
    }

    public ArmController GetArmController()
    {
        return _armController;
    }

    public Rigidbody2D GetRB()
    {
        return _rb;
    }
}