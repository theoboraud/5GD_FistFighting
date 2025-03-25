using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;

public enum GameScene
{
    Lobby,
    Playable,
    Intro,
    Outro,
}

/// <summary>
///     Class used as a level reference : contains the different spawn points, number of spawned player, scene index...
///     Also used to load and end levels and scenes, spawn players, etc...
/// </summary>
public class LevelManager : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public static LevelManager Instance;     // Singleton reference

    [Header("Scene Names")]
    [SerializeField] private string introSceneName;
    [SerializeField] private string lobbySceneName;
    [SerializeField] private string outroSceneName;
    [SerializeField] private List<string> playableSceneNames = new List<string>();

    [Header("Variables")]
    public List<Transform> SpawnPoints;     // Current level spawn points
    [System.NonSerialized] public int CurrentSceneIndex = 0;        // Index of the current scene (in Build Settings)
    [System.NonSerialized] public string CurrentSceneName;        // Name of the current scene
    private List<string> LevelsPlayed = new List<string>();

    [System.Serializable]
    public class GameModeLevel
    {
        public string modeName;
        public List<string> levelNames = new List<string>();
    }

    public List<GameModeLevel> gameModeLevels = new List<GameModeLevel>();
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
        /*if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            #if UNITY_EDITOR
            levelName = SceneManager.GetActiveScene().name;
            #endif

            Invoke("Reset", 0.1f);
        }
        else
        {*/
            // Init scene index and spawn points
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        CurrentSceneName = SceneManager.GetActiveScene().name;

    }

    public void OnEnable()
    {
        SubsribeEvents();
    }
    public void OnDisable()
    {
        UnsribeEvents();
    }

    private void SubsribeEvents()
    {
        GEventCenter.Subscribe(GameEvent.OnNewGameRound,LoadNextLevel) ;
        GEventCenter.Subscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Subscribe<List<Transform>>(GameEvent.OnSpawnPointsInit, InitSpawnPoints);
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe(GameEvent.OnNewGameRound, LoadNextLevel);
        GEventCenter.Unsubscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Unsubscribe<List<Transform>>(GameEvent.OnSpawnPointsInit, InitSpawnPoints);
    }
    // #endregion

    // #region ==================== LEVEL FUNCTIONS ====================

    /// <summary>
    ///     Init current level spawn points by loading them
    /// </summary>
    public void InitSpawnPoints(List<Transform> _spawnPoints)
    {
        SpawnPoints = _spawnPoints;
    }


    /// <summary>
    ///     Whether or not we are in the intro scene
    /// </summary>
    public bool IsIntroScene()
    {
        return CurrentSceneName == introSceneName;
    }


    /// <summary>
    ///     Whether or not we are in the lobby scene
    /// </summary>
    public bool IsLobbyScene()
    {
        return CurrentSceneName == lobbySceneName;
    }


    /// <summary>
    ///     Whether or not we are in the lobby scene
    /// </summary>
    public bool IsPlayableScene()
    {
        return playableSceneNames.Contains(CurrentSceneName);
    }


    /// <summary>
    ///     Load scene at the given index
    /// </summary>
    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);

        if (playableSceneNames.Contains(_sceneName))
        {
            GEventCenter.Invoke<GameScene>(GameEvent.OnLoadScene, GameScene.Playable);
        }
        else if(_sceneName == lobbySceneName)
        {
            GEventCenter.Invoke<GameScene>(GameEvent.OnLoadScene, GameScene.Lobby);
        }
        else if (_sceneName == introSceneName)
        {
            GEventCenter.Invoke<GameScene>(GameEvent.OnLoadScene, GameScene.Intro);

        }
        else if(_sceneName == outroSceneName)
        {
            GEventCenter.Invoke<GameScene>(GameEvent.OnLoadScene, GameScene.Outro);
        }

            //CurrentSceneIndex = _levelIndex;
            CurrentSceneName = _sceneName;
    }


    /// <summary>
    ///     Get the current scene index
    /// </summary>
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }


    /// <summary>
    ///     Load the lobby level
    /// </summary>
    public void LoadLobbyLevel()
    {
        LoadScene(lobbySceneName);
    }


    /// <summary>
    ///     Load the outro scene
    /// </summary>
    public void LoadOutroScene()
    {
        LoadScene(outroSceneName);
    }

    /// <summary>
    ///     Load a random level from Build Settings, except the first one
    /// </summary>
    public void LoadRandomLevel()
    {
        string _randomScene = CurrentSceneName;

        while(_randomScene == CurrentSceneName)
        {
            _randomScene = playableSceneNames[Random.Range(0, playableSceneNames.Count - 1)];
        }

        LoadScene(_randomScene);
    }

    /// <summary>
    ///     Load the next level of the list, except the first one
    /// </summary>
    public void LoadNextLevel()
    {
        if (true)
        {
            // If playable level, add it to the levels played
            if (playableSceneNames.Contains(CurrentSceneName))
            {
                LevelsPlayed.Add(CurrentSceneName);
            }

            // If we played every level, empty the list
            if (LevelsPlayed.Count >= playableSceneNames.Count)
            {
                LevelsPlayed.Clear();
            }

            // Get a random scene index not yet in LevelsPlayed
            string _nextScene = playableSceneNames[Random.Range(0, playableSceneNames.Count - 1)];

            if (LevelsPlayed.Count > 0)
            {
                while (LevelsPlayed.Contains(_nextScene) || _nextScene == CurrentSceneName)
                {
                    _nextScene = playableSceneNames[Random.Range(0, playableSceneNames.Count - 1)];
                }
            }
            LoadScene(_nextScene);
        }
        else
        {
            //PlayersManager.Instance.ResetPlayersLives(GameManager.Instance.ParamData.PARAM_Player_Lives);

            //SceneManager.LoadScene(CurrentSceneName);

            //CurrentSceneIndex = SceneManager.sceneCountInBuildSettings;

            //MenuManager.Instance.StartTimer();

            //MenuManager.Instance.ResetPlayersUI();
        }
    }

    public void Reset()
    {
        if (CurrentSceneName != lobbySceneName)
        {
            Invoke("LoadLobbyLevel", 0.1f);
        }
    }

    // #endregion
}
