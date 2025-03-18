using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Enums;

public class PlayersManager : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public static PlayersManager Instance;               // Singleton reference
    [System.NonSerialized] public SkinsData SkinsData;                          // SkinData reference for loading skins

    [Header("Variables")]
    [System.NonSerialized] public List<Player> Players;                         // All players references
    [System.NonSerialized] public List<Player> PlayersSpawned;                  // All players spawned currently
    [System.NonSerialized] public List<Player> PlayersAlive;                    // All players alive in the current game
    [System.NonSerialized] public List<Player> PlayersDeathOrder;               // All players that died in the current game, in the death order
    [System.NonSerialized] public List<int> PlayersLives = new List<int>();     // Nb of lives for each player

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init singleton, references and variables
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /// <summary>
    ///     Public init method
    /// </summary>
    public void Init()
    {
        // Init references
        SkinsData = gameObject.GetComponent<SkinsData>();

        // Init variables
        Players = new List<Player>();
        PlayersSpawned = new List<Player>();
        PlayersAlive = new List<Player>();
        PlayersDeathOrder = new List<Player>();

        // Players have only 1 life in the lobby
        ResetPlayersLives(1);
    }

    public void OnEnable()
    {
        SubsribeEvents();
    }
    public void OnDisable()
    {
        UnsribeEvents();
    }
    private void SubsribeEvents()
    {
        GEventCenter.Instance.Subscribe(GameEvent.OnNewGameRound, OnNewGameRound);
        GEventCenter.Subscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Subscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe(GameEvent.OnNewGameRound, OnNewGameRound);
        GEventCenter.Unsubscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Unsubscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
    }

    // #endregion



    // #region ==================== PLAYERS FUNCTIONS ====================

    /// <summary>
    ///     Add a given player to the game
    /// </summary>
    public void AddPlayer(Player _player)
    {
        // Maximum number of players is 4
        if (Players.Count < 4)
        {
            // Add the player for every manager and init its values
            Players.Add(_player);
            // Set lives to starting value
            if (LevelManager.Instance.CurrentSceneIndex == 0)
            {
                PlayersLives.Add(1);
            }
            else
            {
                PlayersLives.Add(GameManager.Instance.ParamData.PARAM_Player_Lives);
            }

            MenuManager.Instance.AddPlayer(Players.IndexOf(_player));
            GameManager.Instance.PlayerScores.Add(0);

            // Set action map on Gameplay if player is spawning InPlay
            if (GameManager.Instance.GlobalGameState is GlobalGameState.InPlay)
            {
                _player.GetComponent<UnityEngine.InputSystem.PlayerInput>().SwitchCurrentActionMap("Gameplay");
            }

            // Set the layer of the player
            int _playerIndex = PlayersManager.Instance.Players.IndexOf(_player) + 1;
            string _playerLayer = "Player" + _playerIndex.ToString();
            _player.gameObject.layer = LayerMask.NameToLayer(_playerLayer);
            _player.PlayerLayer = _playerLayer;

            // Spawn the player
            SpawnPlayer(_player);

            // Add its timer reference to SpawningTimers in MenuManager
            MenuManager.Instance.SpawningTimers.Add(0f);

            // Set the player to not ready
            _player.IsReadyUI(false);
        }
        // If the game tries to spawn a 5th player
        else
        {
            Destroy(_player.gameObject);
        }

    }


    /// <summary>
    ///     Spawn all players registered by PlayersManager
    /// </summary>
    public void ResetPlayersLives(int _lives)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            // Reinit player lives
            PlayersLives[i] = _lives;
        }
    }


    /// <summary>
    ///     Spawn a given player
    /// </summary>
    public void SpawnPlayer(Player _player)
    {
        _player.Spawn(LevelManager.Instance.SpawnPoints[Players.IndexOf(_player)].position);

        // Add the player to PlayersAlive references in PlayerManager
        if (!PlayersAlive.Contains(_player))
        {
            PlayersAlive.Add(_player);
        }
        PlayersSpawned.Add(_player);
        
    }


    /// <summary>
    ///     Start the timer to spawn a player
    /// </summary>
    public void StartSpawningPlayer(Player _player)
    {
        MenuManager.Instance.StartSpawnTimer(Players.IndexOf(_player));
    }


    public void ResetSpawnedPlayers()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Kill();
        }

        PlayersSpawned.Clear();
        PlayersAlive.Clear();
    }


    /// <summary>
    ///     Returns whether or not all players are ready to play (from menu or score screen)
    /// </summary>
    public bool AllPlayersReady()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (!Players[i].IsReady)
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    ///     Kill a given player
    /// </summary>
    public void KillPlayer(Player _player)
    {
        // The player loses a life
        PlayersLives[Players.IndexOf(_player)] -= 1;
        MenuManager.Instance.UpdateLives();
        PlayersSpawned.Remove(_player);

        if (PlayersLives[Players.IndexOf(_player)] <= 0)
        {
            PlayersDeathOrder.Add(_player);
            PlayersAlive.Remove(_player);

            if (GameManager.Instance.GlobalGameState == GlobalGameState.InPlay)
            {
                // If there is a winning player
                if (PlayersAlive.Count == 1)
                {
                    GameManager.Instance.EndOfRound(PlayersAlive[0]);
                }
                // If there is only one player playing
                else if (PlayersAlive.Count == 0)
                {
                    GameManager.Instance.EndOfRound(null);
                }
            }
        }
        else if (GameManager.Instance.GlobalGameState is GlobalGameState.InPlay)
        {
            StartSpawningPlayer(_player);
        }
    }


    /// <summary>
    ///     Kill all other players than _player
    /// </summary>
    public void KillOtherPlayers(Player _player)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] != _player)
            {
                Players[i].Kill();
            }
        }
    }


    public void ChangeMode(string _newMode)
    {
        if (_newMode == "Gameplay" || _newMode == "Menu")
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].GetComponent<UnityEngine.InputSystem.PlayerInput>().SwitchCurrentActionMap(_newMode);
            }
        }
        else
        {
            Debug.Log("ERROR: _newMode has the value " + _newMode.ToString() + " which is not valid. See PlayersManager.ChangeMode()");
        }
    }


    /// <summary>
    ///     Reset all variables
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Destroy(Players[i].gameObject);
        }

        Players.Clear();
        PlayersSpawned.Clear();
        PlayersAlive.Clear();
        PlayersDeathOrder.Clear();
    }

    //Call when game manager start a NEW GAME ROUND
    private void OnNewGameRound()
    {
        PlayersDeathOrder.Clear();
    }

    private void OnSceneLoad(GameScene _scene)
    {
        if (_scene == GameScene.Playable)
        {
            ResetPlayersLives(GameManager.Instance.ParamData.PARAM_Player_Lives);
        }
        else if(_scene == GameScene.Lobby)
        {
            ResetPlayersLives(1);
        }
    }
    // #endregion
}
