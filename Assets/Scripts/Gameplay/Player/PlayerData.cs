
using UnityEngine;
using UnityEngine.Events;

public class PlayerData 
{
	private Player _player;
	private int _playerIndex = 1;
    private int _playerLives = 0;
    private int _playerScore = 0;//TODO

    public PlayerData(Player _myPlayer)
    {
        _player = _myPlayer;
    }

    public int PlayerIndex
	{
		get { return _playerIndex; }
		set
		{
            _playerIndex = value;
        }
	}
    public int PlayerLives
    {
        get { return _playerLives; }
        set
        {
            _playerLives = value;
        }
    }
    public int PlayerScore
    {
        get { return _playerScore; }
        set
        {
            _playerScore = value;
        }
    }
}