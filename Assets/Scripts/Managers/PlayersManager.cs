using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayersManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static PlayersManager Instance;
    [System.NonSerialized] public List<Player> Players;
    [System.NonSerialized] public SkinsData SkinsData;

    [Header("Variables")]
    private List<Player> PlayersAlive;


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
        Players = new List<Player>();
        PlayersAlive = new List<Player>();
        SkinsData = gameObject.GetComponent<SkinsData>();
    }


    //Function that's called when Avatar is spawned
    //Allows this script to manage all players
    public void AddPlayer(Player _player)
    {
        Players.Add(_player);
        _player.ChangeSkin(SkinsData.GetRandomSkin());
        LevelManager.Instance.SpawnPlayer(_player);
    }


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


    public void SimulateAllPlayers(bool _bool)
    {
        foreach (var _player in Players)
        {
            _player.RB.simulated = _bool;
        }
    }


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
}
