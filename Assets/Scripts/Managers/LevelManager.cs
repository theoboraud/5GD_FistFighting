using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static LevelManager Instance;
    private List<GameObject> SpawnPoints;

    //[System.NonSerialized] public List<LevelData> Levels;
    //[System.NonSerialized] public LevelData Level;
    [Header("Variables")]
    [System.NonSerialized] public int IndexPlayerSpawn = 0;
    private int currentSceneIndex;

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

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        InitSpawnPoints();
    }

    public void InitSpawnPoints()
    {
        SpawnPoints = new List<GameObject>();
        GameObject _GO_SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints")[0];

        int _index = 0;
        foreach (Transform _child in _GO_SpawnPoints.transform)
        {
            SpawnPoints.Add(_child.gameObject);
            _index++;
        }
        // TO CHANGE LATER
        IndexPlayerSpawn = 0;
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
        int _randomSceneIndex = currentSceneIndex;
        while(_randomSceneIndex == currentSceneIndex)
        {
            _randomSceneIndex = Random.Range(0, SceneManager.sceneCountInBuildSettings);
        }
        print("Called LoadRandomLevel");
        print(currentSceneIndex);
        print(_randomSceneIndex);
        currentSceneIndex = _randomSceneIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
