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
    [System.NonSerialized] public List<GameObject> SpawnPoints;     // Current level spawn points
    [System.NonSerialized] public int CurrentSceneIndex = 0;        // Index of the current scene (in Build Settings)
    private List<int> LevelsPlayed = new List<int>();

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
        // Singleton declaration
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Init scene index and spawn points
            CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Init spawn points of the first level
            InitSpawnPoints();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // #endregion



    // #region ==================== LEVEL FUNCTIONS ====================

    /// <summary>
    ///     Init current level spawn points by loading them
    /// </summary>
    public void InitSpawnPoints()
    {
        SpawnPoints = new List<GameObject>();
        GameObject _GO_SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints")[0];

        foreach (Transform _child in _GO_SpawnPoints.transform)
        {
            SpawnPoints.Add(_child.gameObject);
        }

        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.InitSpawnTimerPos();
        }
    }


    /// <summary>
    ///     Load scene at the given index
    /// </summary>
    public void LoadScene(int _levelIndex)
    {
        SceneManager.LoadScene(_levelIndex);
        CurrentSceneIndex = _levelIndex;

        if (_levelIndex != 0)
        {
            MenuManager.Instance.StartTimer();
        }

        MenuManager.Instance.UpdateLives();
    }


    /// <summary>
    ///     Get the current scene index
    /// </summary>
    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }


    /// <summary>
    ///     Load the lobby level
    /// </summary>
    public void LoadLobbyLevel()
    {
        // Players only have 1 life in the lobby
        PlayersManager.Instance.ResetPlayersLives(1);

        LoadScene(0);
    }


    /// <summary>
    ///     Load a random level from Build Settings, except the first one
    /// </summary>
    public void LoadRandomLevel()
    {
        int _randomSceneIndex = CurrentSceneIndex;

        while(_randomSceneIndex == CurrentSceneIndex)
        {
            _randomSceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings);
        }

        LoadScene(_randomSceneIndex);
    }


    /// <summary>
    ///     Load the next level of the list, except the first one
    /// </summary>
    public void LoadNextLevel()
    {
        // If not lobby level, add it to the levels played
        if (CurrentSceneIndex != 0)
        {
            LevelsPlayed.Add(CurrentSceneIndex);
        }

        // If we played every level, empty the list
        if (LevelsPlayed.Count >= SceneManager.sceneCountInBuildSettings - 2)
        {
            LevelsPlayed.Clear();
        }

        // Get a random scene index not yet in LevelsPlayed
        int _nextSceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings - 1);

        if (LevelsPlayed.Count > 0)
        {
            while (LevelsPlayed.Contains(_nextSceneIndex))
            {
                _nextSceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings - 1);
            }
        }

        PlayersManager.Instance.ResetPlayersLives(GameManager.Instance.ParamData.PARAM_Player_Lives);

        LoadScene(_nextSceneIndex);
    }


    public void Reset()
    {
        MenuManager.Instance.PrintScoreScreen(false);

        for (int i = 0; i < MenuManager.Instance.UI_PlayersLives.Count; i++)
        {
            Destroy(MenuManager.Instance.UI_PlayersLives[i].transform.parent.gameObject);
        }

        PlayersManager.Instance.PlayersDeathOrder.Clear();
        GameManager.Instance.Feedback.ResetAllVFX();
        GameManager.Instance.GlobalGameState = GlobalGameState.InPlay;

        for (int i = 0; i < PlayersManager.Instance.Players.Count; i++)
        {
            Destroy(PlayersManager.Instance.Players[i].gameObject);
        }

        Destroy(GameManager.Instance);
        Destroy(PlayersManager.Instance);
        Destroy(MenuManager.Instance);
        LoadLobbyLevel();
    }

    // #endregion
}
