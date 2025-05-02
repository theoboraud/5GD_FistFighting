
using Enums;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData 
{
	private Player _player;
	private int _playerIndex = 1;
    private int _playerLives = 0;
    private int _playerScore = 0;//TODO
    private Color _playerColor;

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
            if (_playerLives != value)
            {
                _playerLives = value;
                _player.EventCenter.Invoke<int>(PlayerEvent.OnPlayerLivesChange, _playerLives);
            }
        }
    }
    public int PlayerScore
    {
        get { return _playerScore; }
        set
        {
            if (_playerScore != value)
            {
                _playerScore = value;
                _player.EventCenter.Invoke<int>(PlayerEvent.OnPlayerScoreChange, _playerScore);
            }
        }
    }

    public Color PlayerColor 
    { 
        get => _playerColor; 
        set => _playerColor = value; 
    }
}