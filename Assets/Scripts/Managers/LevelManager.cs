using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static LevelManager Instance;
    //[System.NonSerialized] public List<LevelData> Levels;
    //[System.NonSerialized] public LevelData Level;


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


    public void LoadScene(int _levelIndex)
    {
        PlayerManager.Instance.SimulateAllPlayers(_levelIndex > 0);
        SceneManager.LoadScene(_levelIndex);
    }


    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }


    public void SpawnAllPlayers()
    {
        for (int i = 0; i < PlayersManager.Instance.Players.Count; i++)
        {
            PlayersManager.Instance.Players[i].transform.position = Level.SpawnPoints[i].position;
        }
    }
}
