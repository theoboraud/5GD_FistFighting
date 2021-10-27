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
        SkinsData = gameObject.GetComponent<SkinsData>();
    }


    //Function that's called when Avatar is spawned
    //Allows this script to manage all players
    public void AddPlayer(Player _player)
    {
        Players.Add(_player);
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
            if (Players[i].PlayerState != PlayerState.Ready)
            {
                return false;
            }
        }

        return true;
    }
}
