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
    [System.NonSerialized] public List<Player> PlayersAlive;                    // All players alive in the current game
    [System.NonSerialized] public List<Player> PlayersDeathOrder;               // All players that died in the current game, in the death order
    [System.NonSerialized] public List<int> PlayerLives = new List<int>();      // Nb of lives for each player

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

            // Init references
            SkinsData = gameObject.GetComponent<SkinsData>();

            // Init variables
            Players = new List<Player>();
            PlayersAlive = new List<Player>();
            PlayersDeathOrder = new List<Player>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // #endregion



    // #region ==================== PLAYERS FUNCTIONS ====================

    /// <summary>
    ///     Add a given player to the game
    /// </summary>
    public void AddPlayer(Player _player)
    {
        // Add the player for every manager and init its values
        Players.Add(_player);
        PlayerLives.Add(GameManager.Instance.ParamData.PARAM_Player_Lives); // Set lives to starting value
        MenuManager.Instance.AddPlayerColor(Players.IndexOf(_player));
        MenuManager.Instance.AddPlayerScore(Players.IndexOf(_player));
        GameManager.Instance.PlayerScores.Add(0);
        
        // Set action map on Gameplay if player is spawning InPlay
        if (GameManager.Instance.GlobalGameState is GlobalGameState.InPlay)
        {
            _player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
        }

        // Spawn the player
        SpawnPlayer(_player);

        // Add its timer reference to SpawningTimers in MenuManager
        MenuManager.Instance.SpawningTimers.Add(0f);
    }


    /// <summary>
    ///     Spawn all players registered by PlayersManager
    /// </summary>
    public void ResetPlayerLives()
    {
        for (int i = 0; i < PlayersManager.Instance.Players.Count; i++)
        {
            // Reinit player lives
            PlayerLives[i] = GameManager.Instance.ParamData.PARAM_Player_Lives;
        }
    }


    /// <summary>
    ///     Spawn a given player
    /// </summary>
    public void SpawnPlayer(Player _player)
    {
        _player.Spawn(LevelManager.Instance.SpawnPoints[Players.IndexOf(_player)].transform.position);

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
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Kill();
        }

        PlayersAlive.Clear();
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
        // The player loses a life
        PlayerLives[Players.IndexOf(_player)] -= 1;
        PlayersAlive.Remove(_player);

        if (PlayerLives[Players.IndexOf(_player)] <= 0)
        {
            PlayersDeathOrder.Add(_player);

            if (GameManager.Instance.GlobalGameState == GlobalGameState.InPlay)
            {
                // If there is a winning player
                if (PlayersDeathOrder.Count == Players.Count - 1)
                {
                    GameManager.Instance.EndOfRound(PlayersAlive[0]);
                }
                // If there is only one player playing
                else if (PlayersDeathOrder.Count == Players.Count)
                {
                    GameManager.Instance.EndOfRound(null);
                }
            }
        }
        else
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
                Players[i].GetComponent<PlayerInput>().SwitchCurrentActionMap(_newMode);
            }
        }
        else
        {
            Debug.Log("ERROR: _newMode has the value " + _newMode.ToString() + " which is not valid. See PlayersManager.ChangeMode()");
        }
    }

    // #endregion
}
