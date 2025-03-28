using System;
using Enums;
using UnityEngine;

public class PlayerStates
{
    private Player _player;
    private PlayerGameState _playerGameState;
    private PlayerPhysicState _playerPhysicState;
    private PlayerRotateState _playerRotateState;
    //Not In Use
    //private PlayerArmState _playerArmState;

    public PlayerStates(Player _myPlayer)
    {
        _player = _myPlayer;
    }
    public PlayerGameState PlayerGameState
    {
        get { return _playerGameState; }
        set
        {
            if (_playerGameState != value)
            {
	            _playerGameState = value;
                _player.EventCenter.Invoke<PlayerGameState>(PlayerEvent.OnGameStateChange, _playerGameState);
            }
        }
    }
    public PlayerPhysicState PlayerPhysicState
	    {
        get { return _playerPhysicState; }
        set
        {
            if (_playerPhysicState != value)
            {
	            _playerPhysicState = value;
                _player.EventCenter.Invoke<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, _playerPhysicState);
            }
        }
    }
    
    /// <summary>
    /// TODO
    /// </summary>
    public PlayerRotateState PlayerRotateState
	{
        get { return _playerRotateState; }
        set
        {
            if (_playerRotateState != value)
            {
	            _playerRotateState = value;
                //Broadcast of rotate state change, not in use
                //_player.EventCenter.Invoke<PlayerRotateState>(PlayerEvent.OnRotateStateChange, _playerRotateState);
            }
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    //public PlayerArmState PlayerArmState
    //{
    //    get { return _playerArmState; }
    //    set
    //    {
    //        if (_playerArmState != value)
	   //         _playerArmState = value;
    //        _player.EventCenter.Invoke<PlayerArmState>(PlayerEvent.OnArmStateChange, _playerArmState);
    //    }
    //}

    public bool IsReady = false;
	public void Init()
	{
		PlayerGameState = PlayerGameState.Alive;
		PlayerPhysicState = PlayerPhysicState.InAir;
	}
}
