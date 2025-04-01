using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Enums;

public class PlayersManager : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================
    [SerializeField]
    [Header("References")]
    [System.NonSerialized] public static PlayersManager Instance;               // Singleton reference
    [System.NonSerialized] public SkinsData SkinsData;                          // SkinData reference for loading skins

    [Header("Variables")]
    [System.NonSerialized] public List<Player> Players;                         // All players references
    [System.NonSerialized] public List<Player> PlayersSpawned;                  // All players spawned currently
    [System.NonSerialized] public List<Player> PlayersAlive;                    // All players alive in the current game


    private PlayerInputManager _playerInputManager;
    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init singleton, references and variables
    /// </summary>
    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();

        // Init references
        SkinsData = gameObject.GetComponent<SkinsData>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
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
        // Init variables
        Players = new List<Player>();
        PlayersSpawned = new List<Player>();
        PlayersAlive = new List<Player>();

        SubsribeEvents();

        // Players have only 1 life in the lobby
        ResetPlayersLives(1);
    }

    /// <summary>
    /// Listen event of on player joined call by input manager
    /// </summary>
    /// <param name="playerInput"></param>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Maximum number of players is 4
        if (Players.Count >= 4) return;

        Debug.Log("New Player Joined: " + playerInput.playerIndex);
        Player newPlayer = playerInput.GetComponent<Player>();
        int playerIndex = playerInput.playerIndex;

        if (newPlayer != null)
        {
            newPlayer.StartInit(playerIndex);
            Players.Add(newPlayer);
            GEventCenter.Invoke<int>(GameEvent.OnPlayerJoin, playerIndex);
            // Spawn the player
            SpawnPlayer(newPlayer);
        }

        // Set action map on Gameplay if player is spawning InPlay
        if (GameManager.Instance.GlobalGameState is GlobalGameState.InPlay)
        {
            playerInput.SwitchCurrentActionMap("Gameplay");
        }
    }

    public void OnDisable()
    {
        UnsribeEvents();
    }
    private void SubsribeEvents()
    {
        GEventCenter.Subscribe(GameEvent.OnNewGameRound, OnNewGameRound);
        GEventCenter.Subscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Subscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
        GEventCenter.Subscribe<Player>(GameEvent.OnPlayerDead, OnPlayerKilled);
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe(GameEvent.OnNewGameRound, OnNewGameRound);
        GEventCenter.Unsubscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Unsubscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
        GEventCenter.Unsubscribe<Player>(GameEvent.OnPlayerDead, OnPlayerKilled);
        _playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }

    // #endregion



    // #region ==================== PLAYERS FUNCTIONS ====================


    /// <summary>
    ///     Spawn all players registered by PlayersManager
    /// </summary>
    public void ResetPlayersLives(int _lives)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            // Reinit player lives
            Players[i].PlayerData.PlayerLives = _lives;
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
            if (!Players[i].isReady)
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    ///Remove reference in player alive list
    /// </summary>
    public void OnPlayerKilled(Player _player)
    {
        // The player loses a life
        PlayersSpawned.Remove(_player);

        if (_player.PlayerData.PlayerLives <= 0)
        {
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
    }

    //Call when game manager start a NEW GAME ROUND
    private void OnNewGameRound()
    {

    }

    private void OnSceneLoad(GameScene _scene)
    {
        if (_scene == GameScene.Playable)
        {
            ResetPlayersLives(GameManager.Instance.ParamData.PARAM_Player_Lives);
        }
        else if (_scene == GameScene.Lobby)
        {
            ResetPlayersLives(1);
        }
    }
    // #endregion




}
