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
    private string levelName = "";

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

            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /// <summary>
    ///     Public init method
    /// </summary>
    public void Init()
    {
        // If the scene is not the lobby, we will only launch this scene
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            levelName = SceneManager.GetActiveScene().name;
            Invoke("Reset", 0.1f);
        }
        else
        {
            // Init scene index and spawn points
            CurrentSceneIndex = 0;
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

        if (MenuManager.Instance != null)
        {
            if (_levelIndex > 2)
            {
                MenuManager.Instance.StartTimer();
            }
        }

        MenuManager.Instance.NewGameRound();
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
        if (PlayersManager.Instance != null)
        {
            PlayersManager.Instance.ResetPlayersLives(1);
        }

        LoadScene(2);
    }


    /// <summary>
    ///     Load the outro scene
    /// </summary>
    public void LoadOutroScene()
    {
        LoadScene(1);
        AudioManager.Instance.StopMusic();
        GameManager.Instance.Feedback.ResetAllVFX();

        Invoke("OutroGameState", 0.1f);
    }


    private void OutroGameState()
    {
        MenuManager.Instance.ScoreScreen.SetActive(false);
        GameManager.Instance.GlobalGameState = GlobalGameState.Outro;
    }


    /// <summary>
    ///     Load a random level from Build Settings, except the first one
    /// </summary>
    public void LoadRandomLevel()
    {
        int _randomSceneIndex = CurrentSceneIndex;

        while(_randomSceneIndex == CurrentSceneIndex)
        {
            _randomSceneIndex = Random.Range(3, SceneManager.sceneCountInBuildSettings);
        }

        LoadScene(_randomSceneIndex);
    }


    /// <summary>
    ///     Load the next level of the list, except the first one
    /// </summary>
    public void LoadNextLevel()
    {
        if (levelName == "")
        {
            // If not lobby level, add it to the levels played
            if (CurrentSceneIndex > 2)
            {
                LevelsPlayed.Add(CurrentSceneIndex);
            }

            // If we played every level, empty the list
            if (LevelsPlayed.Count >= SceneManager.sceneCountInBuildSettings - 3)
            {
                LevelsPlayed.Clear();
            }

            // Get a random scene index not yet in LevelsPlayed
            int _nextSceneIndex = Random.Range(3, SceneManager.sceneCountInBuildSettings);

            if (LevelsPlayed.Count > 0)
            {
                while (LevelsPlayed.Contains(_nextSceneIndex) || _nextSceneIndex == CurrentSceneIndex)
                {
                    _nextSceneIndex = Random.Range(3, SceneManager.sceneCountInBuildSettings);
                }
            }

            PlayersManager.Instance.ResetPlayersLives(GameManager.Instance.ParamData.PARAM_Player_Lives);

            if (_nextSceneIndex > 2) AudioManager.Instance.ChangeParam(2);
            else if (_nextSceneIndex == 0) AudioManager.Instance.ChangeParam(1);
            else AudioManager.Instance.ChangeParam(0);
            LoadScene(_nextSceneIndex);
        }
        else
        {
            PlayersManager.Instance.ResetPlayersLives(GameManager.Instance.ParamData.PARAM_Player_Lives);

            SceneManager.LoadScene(levelName);

            CurrentSceneIndex = SceneManager.sceneCountInBuildSettings;

            MenuManager.Instance.StartTimer();

            MenuManager.Instance.NewGameRound();
        }
    }


    public void Reset()
    {
        MenuManager.Instance.Reset();
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.StopWinSound();

        GameManager.Instance.Feedback.ResetAllVFX();

        AudioManager.Instance.StopMusic();
        Destroy(AudioManager.Instance);

        GameManager.Instance.GlobalGameState = GlobalGameState.InPlay;

        PlayersManager.Instance.Reset();

        Destroy(GameManager.Instance);
        Destroy(PlayersManager.Instance);
        Destroy(MenuManager.Instance);
        Invoke("LoadLobbyLevel", 0.1f);
    }

    // #endregion
}
