using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Enums;

public class PlayersManager : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public static PlayersManager Instance;   // Singleton reference
    [System.NonSerialized] public SkinsData SkinsData;              // SkinData reference for loading skins

    [Header("Variables")]
    [System.NonSerialized] public List<Player> Players;             // All players references
    [System.NonSerialized] public List<Player> PlayersAlive;        // All players alive in the current game
    [System.NonSerialized] public int IndexPlayerSpawn;             // Index of the current number of spawned players

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
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Init references
        SkinsData = gameObject.GetComponent<SkinsData>();

        // Init variables
        Players = new List<Player>();
        PlayersAlive = new List<Player>();
    }

    // #endregion



    // #region ==================== PLAYERS FUNCTIONS ====================

    /// <summary>
    ///     Add a given player to the game
    /// </summary>
    public void AddPlayer(Player _player)
    {
        Players.Add(_player);
        SpawnPlayer(_player);

        // Add its timer reference to SpawningTimers in MenuManager
        MenuManager.Instance.SpawningTimers.Add(0f);
    }


    /// <summary>
    ///     Spawn all players registered by PlayersManager
    /// </summary>
    public void SpawnAllPlayers()
    {
        for (int i = IndexPlayerSpawn; i < PlayersManager.Instance.Players.Count; i++)
        {
            SpawnPlayer(PlayersManager.Instance.Players[i]);
        }
    }


    /// <summary>
    ///     Spawn a given player
    /// </summary>
    public void SpawnPlayer(Player _player)
    {
        _player.Spawn(LevelManager.Instance.SpawnPoints[Players.IndexOf(_player)].transform.position);

        IndexPlayerSpawn++;

        // Add the player to PlayersAlive references in PlayerManager
        PlayersAlive.Add(_player);
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
        IndexPlayerSpawn = 0;

        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Kill();
        }

        PlayersAlive.Clear();
    }


    /// <summary>
    ///     Simulate or stop simulating all players in the game, depending on the boolean parameter _bool
    /// </summary>
    public void SimulateAllPlayers(bool _bool)
    {
        foreach (var _player in Players)
        {
            _player.RB.simulated = _bool;
        }
    }


    /// <summary>
    ///     Returns whether or not all players are ready to play (from menu or score screen)
    /// </summary>
    public bool AllPlayersReady()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].PlayerGameState != PlayerGameState.Ready)
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
        PlayersAlive.Remove(_player);

        if (GameManager.Instance.GlobalGameState == GlobalGameState.InPlay)
        {
            if (PlayersAlive.Count == 1)
            {
                GameManager.Instance.EndOfRound(PlayersAlive[0]);
            }
            // If there is only one player
            else if (PlayersAlive.Count == 0)
            {
                GameManager.Instance.EndOfRound(null);
            }
        }
    }

    // #endregion
}
