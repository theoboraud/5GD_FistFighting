using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static GameManager Instance;
    [System.NonSerialized] public GameState GameState;

    private int i;

    // Init as a singleton
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
    }


    //Function that's called when Avatar is spawned
    //Allows this script to manage all players
    public void AddPlayer(Player _player)
    {
        Players.Add(_player);
        _player.transform.position = SpawnPoints[i].position;
        i++;
    }

    //Function that allows the game to change level
    public void ChangeScene(int sceneIndex)
    {
        if (sceneIndex > 0)
        {
            foreach (var item in Players)
            {
                item.RB.simulated = true;
            }
        }
        else
        {
            foreach (var item in Players)
            {
                item.RB.simulated = false;
            }
        }
        SceneManager.LoadScene(sceneIndex);
        i = 0;
    }

    public void PlacePlayers()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].transform.position = SpawnPoints[i].position;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
