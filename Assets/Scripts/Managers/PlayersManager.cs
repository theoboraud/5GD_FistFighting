using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        _player.ChangeSkin(SkinsData.GetRandomSkin());
        LevelManager.Instance.SpawnPlayer(_player);
        PlayersAlive.Add(_player);
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
        _player.Kill();
        PlayersAlive.Remove(_player);

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


    /// <summary>
    ///     Destroy all players in the game
    /// </summary>
    public void DestroyAllPlayers()
    {
        PlayersAlive.Clear();
        for (int i = 0; i < Players.Count; i++)
        {
            Player _player = Players[i];
            Players.Remove(_player);
            Destroy(_player.gameObject);
        }
    }

    // #endregion
}
