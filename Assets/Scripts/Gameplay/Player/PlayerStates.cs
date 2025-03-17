using System;
using Enums;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
	private PlayerData _playerData;
	
	public PlayerGameState playerGameState;
	public PlayerPhysicState playerPhysicState;
	public PlayerRotateState playerRotateState;

	public bool IsReady = false;

	private void OnEnable()
	{
		_playerData = GetComponent<PlayerData>();

		InitCallBacks();
	}

	private void OnDisable()
	{
		RemoveCallBacks();
	}

	private void InitCallBacks()
	{
        EventCenter.Subscribe(GameEvent.OnPlayerInit, Init);
        EventCenter.Subscribe(GameEvent.OnPlayerSpawn, SetPlayerAlive);
        EventCenter.Subscribe(GameEvent.OnPlayerHit, SetPhysicHit);
        EventCenter.Subscribe(GameEvent.OnPlayerKilled, SetPlayerDead);
        EventCenter.Subscribe(GameEvent.OnInvinciblityStart, SetPlayerInvincible);
        EventCenter.Subscribe(GameEvent.OnInvinciblityStop, SetPlayerAlive);
        EventCenter.Subscribe(GameEvent.OnGround, SetPlayerGround);
        EventCenter.Subscribe(GameEvent.OnAir, SetPlayerAir);
	}

	private void RemoveCallBacks()
	{
        EventCenter.Unsubscribe(GameEvent.OnPlayerInit, Init);
        EventCenter.Unsubscribe(GameEvent.OnPlayerSpawn, SetPlayerAlive);
        EventCenter.Unsubscribe(GameEvent.OnPlayerHit, SetPhysicHit);
        EventCenter.Unsubscribe(GameEvent.OnPlayerKilled, SetPlayerDead);
        EventCenter.Unsubscribe(GameEvent.OnInvinciblityStart, SetPlayerInvincible);
        EventCenter.Unsubscribe(GameEvent.OnInvinciblityStop, SetPlayerAlive);
        EventCenter.Unsubscribe(GameEvent.OnGround, SetPlayerGround);
        EventCenter.Unsubscribe(GameEvent.OnAir, SetPlayerAir);
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
