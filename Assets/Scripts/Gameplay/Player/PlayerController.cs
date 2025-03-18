using System;
using Enums;
using UnityEngine;
using UnityEngine.Events;

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
	private PlayerFeedbackManager _playerFeedbackManager;
	private PlayerInput _playerInput;
	private PlayerStates _playerStates;
	private Player _player;
	
	[SerializeField] private BoxCollider2D BoxCollider;

	[Header("Events for FMOD")]
	public UnityEvent OnExtendArm;                      // Event called when an arm extends (for FMOD)
	public UnityEvent OnCollision;                      // Event called when the player enters a collision (for FMOD)

	[Header("Variables")]
	[System.NonSerialized] public PlayerGameState PlayerGameState;
	[System.NonSerialized] public PlayerPhysicState PlayerPhysicState;
	[System.NonSerialized] public PlayerRotateState PlayerRotateState;      // Contain the enum of the rotate state (Ready, RotatingRight, RotatingLeft, or OnCooldown)
	[System.NonSerialized] public bool IsReady = false;                     // Indicates if the player is ready in the lobby
	[System.NonSerialized] public float AirPushFactor = 1f;
	[System.NonSerialized] public bool HitObject_bool = false;
	[System.NonSerialized] public bool HoldingTrigger = false;

	[System.NonSerialized] public float StunRecoveryTime;
	[System.NonSerialized] public float StunTimer;
	[System.NonSerialized] public float ForceIncreaseFactor;

	private int skinIndex;                         // Contains the index of the current skin

	private void OnEnable()
	{
		_player = GetComponent<Player>();
		_playerInput = GetComponent<PlayerInput>();
		_rb = GetComponent<Rigidbody2D>();
		
		InitCallBacks();
	}
	
	private void OnDisable()
	{
		RemoveCallBacks();
	}

	public void Init()
	{
		// Init parameters
		GlobalSettings.ApplyPhysicsSettings(_rb);
		
		_playerStates.Init();
	}

	private void InitCallBacks()
	{
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerInit, Init);
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerSpawn, Spawn);
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerHit, Hit);
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerKilled, Kill);
	}

	private void RemoveCallBacks()
	{
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerInit, Init);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerSpawn, Spawn);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerHit, Hit);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerKilled, Kill);
    }

	private void Spawn()
	{
		this.transform.rotation = Quaternion.identity;
		_rb.linearVelocity = new Vector2(0f, 0f);
		_rb.simulated = true;
	}

	private void Kill()
	{
		_rb.simulated = false;
		_rb.linearVelocity = Vector3.zero;
		_rb.angularVelocity = 0f;
		
		_armController.Init();
	}

	private void Hit()
	{
		_armController.IsHit();
		StunTimer = 0;
	}
	
	/// <summary>
	///     Check if hit a lethal object or an arrival
	/// </summary>
	private void OnCollisionEnter2D(Collision2D _collision)
	{
		GameObject _GO = _collision.gameObject;

		if (_GO.CompareTag("Lethal") && _playerStates.PlayerGameState == PlayerGameState.Alive)
		{
			_player.Kill();
		}

		if (_GO.CompareTag("Arrival"))
		{
			GameManager.Instance.EndOfRound(_player);
		}

		_player.EventCenter.Invoke(PlayerEvent.OnPlayerCollisionEnter);
	}
	
	/// <summary>
	///     Check if hit a StaticGround object from the bottom, with raycast
	/// </summary>
	private bool IsGrounded()
	{
		float extraDistance = 0.25f;
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
		Debug.DrawRay(BoxCollider.bounds.center, Vector2.down * (BoxCollider.bounds.extents.y + extraDistance), rayColor);

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
		if (_playerStates.PlayerPhysicState != PlayerPhysicState.IsHit)
		{
			if (IsGrounded())
			{
				_player.EventCenter.Invoke(PlayerEvent.OnGround);
				AirPushFactor = 1f;
			}
			else
			{
                _player.EventCenter.Invoke(PlayerEvent.OnAir);
			}
		}
	}
}