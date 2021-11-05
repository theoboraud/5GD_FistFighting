using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;

/// <summary>
///     Class used as a level reference : contains the different spawn points, number of spawned player, scene index...
///     Also used to load and end levels and scenes, spawn players, etc...
/// </summary>
public class LevelManager : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public static LevelManager Instance;     // Singleton reference


    [Header("Variables")]
    private List<GameObject> spawnPoints;                           // Current level spawn points
    [System.NonSerialized] public int IndexPlayerSpawn;             // Index of the current number of spawned players
    private int currentSceneIndex;                                  // Index of the current scene (in Build Settings)

    // TODO Implement level classes to load data ?
    //[System.NonSerialized] public List<LevelData> Levels;
    //[System.NonSerialized] public LevelData Level;

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init as a singleton and other class variables
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

        // Init scene index and spawn points
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        InitSpawnPoints();
    }

    // #endregion



    // #region ==================== LEVEL FUNCTIONS ====================

    /// <summary>
    ///     Init current level spawn points by loading them
    /// </summary>
    public void InitSpawnPoints()
    {
        IndexPlayerSpawn = 0;

        spawnPoints = new List<GameObject>();
        GameObject _GO_SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints")[0];

        int _index = 0;
        foreach (Transform _child in _GO_SpawnPoints.transform)
        {
            spawnPoints.Add(_child.gameObject);
            _index++;
        }
    }

    /// <summary>
    ///     Load scene at the given index
    /// </summary>
    public void LoadScene(int _levelIndex)
    {
        PlayersManager.Instance.SimulateAllPlayers(_levelIndex > 0);
        SceneManager.LoadScene(_levelIndex);
    }

    /// <summary>
    ///     Get the current scene index
    /// </summary>
    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
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
        _player.transform.position = spawnPoints[IndexPlayerSpawn].transform.position;

        if (_player.CharSkin == null)
        {
            _player.ChangeSkin(PlayersManager.Instance.SkinsData.GetRandomSkin());
        }

        IndexPlayerSpawn++;
    }

    /// <summary>
    ///     Load a random level from Build Settings
    /// </summary>
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

    // #endregion
}
