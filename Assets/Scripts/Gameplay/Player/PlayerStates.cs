using System;
using Enums;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
	private Player _player;
	private PlayerController _playerController;
	private PlayerData _playerData;
	
	public PlayerGameState playerGameState;
	public PlayerPhysicState playerPhysicState;
	public PlayerRotateState playerRotateState;

	public bool IsReady = false;

	private void OnEnable()
	{
		_player = GetComponent<Player>();
		_playerController = GetComponent<PlayerController>();
		_playerData = GetComponent<PlayerData>();

		InitCallBacks();
	}

	private void OnDisable()
	{
		RemoveCallBacks();
	}

	private void InitCallBacks()
	{
		_player.onInit += Init;
		_player.onSpawn += SetPlayerAlive;
		_player.onHit += SetPhysicHit;
		_player.onKilled += SetPlayerDead;
		_player.onInvincibilityStart += SetPlayerInvincible;
		_player.onInvincibilityStop += SetPlayerAlive;
		_playerController.onGround += SetPlayerGround;
		_playerController.onAir += SetPlayerAir;
	}

	private void RemoveCallBacks()
	{
		_player.onInit -= Init;
		_player.onSpawn -= SetPlayerAlive;
		_player.onHit -= SetPhysicHit;
		_player.onKilled -= SetPlayerDead;
		_player.onInvincibilityStart -= SetPlayerInvincible;
		_player.onInvincibilityStop -= SetPlayerAlive;
		_playerController.onGround -= SetPlayerGround;
		_playerController.onAir -= SetPlayerAir;
	}

	public void Init()
	{
		playerGameState = PlayerGameState.Alive;
		playerPhysicState = PlayerPhysicState.InAir;
	}

	private void SetPlayerAlive()
	{
		playerGameState = PlayerGameState.Alive;
		gameObject.layer = LayerMask.NameToLayer($"Player{_playerData.playerIndex}");
	}

	private void SetPlayerDead()
	{
		playerGameState = PlayerGameState.Dead;
	}
	
	private void SetPlayerInvincible()
	{
		playerGameState = PlayerGameState.Invincible;
		gameObject.layer = LayerMask.NameToLayer("Invincible");
	}

	private void SetPhysicHit()
	{
		playerPhysicState = PlayerPhysicState.IsHit;
	}

	private void SetPlayerGround()
	{
		playerPhysicState = PlayerPhysicState.OnGround;
	}

	private void SetPlayerAir()
	{
		playerPhysicState = PlayerPhysicState.InAir;
	}
}
