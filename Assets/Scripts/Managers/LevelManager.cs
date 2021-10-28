using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static LevelManager Instance;
    public List<GameObject> SpawnPoints = new List<GameObject>();

    //[System.NonSerialized] public List<LevelData> Levels;
    //[System.NonSerialized] public LevelData Level;
    [Header("Variables")]
    [System.NonSerialized] public int IndexPlayerSpawn = 0;

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
        PlayersManager.Instance.SimulateAllPlayers(_levelIndex > 0);
        SceneManager.LoadScene(_levelIndex);
    }


    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }


    public void SpawnAllPlayers()
    {
        for (int i = IndexPlayerSpawn; i < PlayersManager.Instance.Players.Count; i++)
        {

            if (PlayersManager.Instance.Players[i].CharSkin == null)
            {
                PlayersManager.Instance.Players[i].ChangeSkin(PlayersManager.Instance.SkinsData.GetRandomSkin());
            }
        }

        IndexPlayerSpawn = PlayersManager.Instance.Players.Count;
    }

    public void SpawnPlayer(Player _player)
    {
        _player.transform.position = SpawnPoints[IndexPlayerSpawn].transform.position;
        IndexPlayerSpawn++;
    }

    public void LoadRandomLevel()
    {
        SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings));
    }
}
