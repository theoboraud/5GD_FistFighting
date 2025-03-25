
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
	public int nbDeath = 0; //TODO, We need check this variable, it seems not realy in used
	private int _playerIndex = 1;

	public int PlayerIndex
	{
		get { return _playerIndex; }
		set
		{
			if (_playerIndex != value)
			{
				_playerIndex = value;
				OnPlayerIndexChanged.Invoke(_playerIndex);
			}
		}
	}

	public UnityEvent<int> OnPlayerIndexChanged = new UnityEvent<int>();
}