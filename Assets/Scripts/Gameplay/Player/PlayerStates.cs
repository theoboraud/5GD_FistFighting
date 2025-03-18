using System;
using Enums;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
	private PlayerData _playerData;
    private Player _player;
    public PlayerGameState PlayerGameState
    {
        get { return PlayerGameState; }
        set
        {
            if (PlayerGameState != value)
            {
                PlayerGameState = value;
                _player.EventCenter.Invoke<PlayerGameState>(PlayerEvent.OnGameStateChange, PlayerGameState);
            }
        }
    }
    public PlayerPhysicState PlayerPhysicState
	    {
        get { return PlayerPhysicState; }
        set
        {
            if (PlayerPhysicState != value)
            {
                PlayerPhysicState = value;
                _player.EventCenter.Invoke<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, PlayerPhysicState);
            }
        }
    }
    public PlayerRotateState PlayerRotateState
	    {
        get { return PlayerRotateState; }
        set
        {
            if (PlayerRotateState != value)
            {
                PlayerRotateState = value;
                _player.EventCenter.Invoke<PlayerRotateState>(PlayerEvent.OnRotateStateChange, PlayerRotateState);
            }
        }
    }

    public bool IsReady = false;

	private void OnEnable()
	{
		_playerData = GetComponent<PlayerData>();
        _player = GetComponent<Player>();

        InitCallBacks();
	}

	private void OnDisable()
	{
		RemoveCallBacks();
	}

	private void InitCallBacks()
	{
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerInit, Init);
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerSpawn, SetPlayerAlive);
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerHit, SetPhysicHit);
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerKilled, SetPlayerDead);
        _player.EventCenter.Subscribe(PlayerEvent.OnInvinciblityStart, SetPlayerInvincible);
        _player.EventCenter.Subscribe(PlayerEvent.OnInvinciblityStop, SetPlayerAlive);
        _player.EventCenter.Subscribe(PlayerEvent.OnGround, SetPlayerGround);
        _player.EventCenter.Subscribe(PlayerEvent.OnAir, SetPlayerAir);
	}

	private void RemoveCallBacks()
	{
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerInit, Init);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerSpawn, SetPlayerAlive);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerHit, SetPhysicHit);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerKilled, SetPlayerDead);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnInvinciblityStart, SetPlayerInvincible);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnInvinciblityStop, SetPlayerAlive);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnGround, SetPlayerGround);
        _player.EventCenter.Unsubscribe(PlayerEvent.OnAir, SetPlayerAir);
    }

	public void Init()
	{
		PlayerGameState = PlayerGameState.Alive;
		PlayerPhysicState = PlayerPhysicState.InAir;
	}

	private void SetPlayerAlive()
	{
		PlayerGameState = PlayerGameState.Alive;
		gameObject.layer = LayerMask.NameToLayer($"Player{_playerData.playerIndex}");
	}

	private void SetPlayerDead()
	{
		PlayerGameState = PlayerGameState.Dead;
	}
	
	private void SetPlayerInvincible()
	{
		PlayerGameState = PlayerGameState.Invincible;
		gameObject.layer = LayerMask.NameToLayer("Invincible");
	}

	private void SetPhysicHit()
	{
		PlayerPhysicState = PlayerPhysicState.IsHit;
	}

	private void SetPlayerGround()
	{
		PlayerPhysicState = PlayerPhysicState.OnGround;
	}

	private void SetPlayerAir()
	{
		PlayerPhysicState = PlayerPhysicState.InAir;
	}

}
