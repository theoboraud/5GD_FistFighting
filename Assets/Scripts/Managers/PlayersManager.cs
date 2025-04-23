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
        PlayersAlive = new List<Player>();

        SubsribeEvents();
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
            // Spawn the player
            SpawnPlayer(newPlayer);

            GEventCenter.Invoke<Player>(GameEvent.OnPlayerJoin, newPlayer);
        }

        // Set action map on Gameplay if player is spawning InPlay or lobby
        if (GameManager.Instance.IsInGameplay() || GameManager.Instance.IsInLobby())
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
        GEventCenter.Subscribe(GameEvent.OnNewStage, OnNewStage);
        GEventCenter.Subscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Subscribe<Player>(GameEvent.OnPlayerDead, OnPlayerKilled);
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe(GameEvent.OnNewGameRound, OnNewGameRound);
        GEventCenter.Unsubscribe(GameEvent.OnNewStage, OnNewStage);
        GEventCenter.Unsubscribe(GameEvent.OnGameReset, Reset);
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
    }


    /// <summary>
    ///     Start the timer to spawn a player
    /// </summary>
    public void StartSpawningPlayer(Player _player)
    {
        MenuManager.Instance.StartSpawnTimer(Players.IndexOf(_player));
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
    ///Remove reference in player alive list
    /// </summary>
    public void OnPlayerKilled(Player _player)
    {
        // The player loses a life
        if (_player.PlayerData.PlayerLives <= 0)
        {
            PlayersAlive.Remove(_player);

            if (GameManager.Instance.IsInGameplay())
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
        else if (GameManager.Instance.IsInGameplay())
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

    /// <summary>
    /// Kill and reset all players in scene
    /// Actual function is set all player's live to 1, and then kill all players. Maybe we should only hide all players in scene //TODO
    /// </summary>
    public void ResetSpawnedPlayers()
    {
        ResetPlayersLives(1);

        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Kill();
        }

        PlayersAlive.Clear();
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
        PlayersAlive.Clear();
    }

    //Call when game manager start a NEW GAME ROUND
    private void OnNewGameRound()
    {
        ResetSpawnedPlayers();
    }

    /// <summary>
    /// Call on new stage loaded
    /// </summary>
    private void OnNewStage()
    {
        ResetPlayersLives(GameManager.Instance.ParamData.PARAM_Player_Lives);
    }
    // #endregion




}
