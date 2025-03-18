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
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerInit, Init);;
	}

	private void RemoveCallBacks()
	{
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerInit, Init);
    }

	public void Init()
	{
		PlayerGameState = PlayerGameState.Alive;
		PlayerPhysicState = PlayerPhysicState.InAir;
	}
}
