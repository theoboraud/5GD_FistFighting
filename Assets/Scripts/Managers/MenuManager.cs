using Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject.Asteroids;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static MenuManager Instance;
    [SerializeField] private GameObject WinnerScreen;
    [SerializeField] private GameObject WinnerScreen_Alone;
    public GameObject ScoreScreen;
    [SerializeField] private GameObject Text_Scoreboard;
    [SerializeField] private GameObject Text_PlayerHasWon;
    public Text WinnerScreen_WinnerName;
    public GameObject InGameUI;
    public GameObject UI_StartingTimer;                                                 // Reference to the starting timer
    public GameObject UI_ReadyTimer;
    public List<GameObject> UI_SpawningTimers = new List<GameObject>();                 // Reference to the spawning timers of each player
    public List<Text> Text_SpawningTimers = new List<Text>();                           // Reference to the Text component of each spawning timers
    public List<PlayerScore> PlayerScores = new List<PlayerScore>();                    // Reference to the player score of each player
    public PauseMenu PauseMenu;                                                         // Reference to the PauseMenu script

    [SerializeField] private GameObject _playerIndicator;

    [Header("Menu Screens")]
    public MainMenu MainMenu;

    [Header("Variables")]
    private List<Color> _playerColors;                                                    // Reference to the player colors
    private Menu activeMenu;
    private float startingTimer = 0f;                                                   // Contains the general spawn timer when starting a new level
    [System.NonSerialized] public float ReadyTimer = 0f;
    [System.NonSerialized] public List<float> SpawningTimers = new List<float>();       // Contains the spawn timer of each player
    UIController _uIController;

    /// <summary>
    ///     Init the singleton reference and the class variables
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
    }


    /// <summary>
    ///     Public init method
    /// </summary>
    public void Init()
    {
        _playerColors = GlobalSettings.PlayerColors;
        _uIController = GetComponent<UIController>();

        _uIController.RegisterScreen(MainMenu.gameObject);
        _uIController.RegisterScreen(WinnerScreen);
        _uIController.RegisterScreen(WinnerScreen_Alone);
        _uIController.RegisterScreen(ScoreScreen);
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
        GEventCenter.Subscribe<GameState>(GameEvent.OnGameStateChange,OnGameStateChange);
        GEventCenter.Subscribe(GameEvent.OnEnterLobby, HideMainMenu);
        GEventCenter.Subscribe(GameEvent.OnNewStage, OnNewGameStage);
        GEventCenter.Subscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Subscribe<List<Transform>>(GameEvent.OnSpawnPointsInit, InitSpawnTimerPos);
        GEventCenter.Subscribe<Player>(GameEvent.OnPlayerJoin, OnPlayerJoin);
        GEventCenter.Subscribe(GameEvent.OnMenuUp, GoUp);
        GEventCenter.Subscribe(GameEvent.OnMenuDown, GoDown);
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe<GameState>(GameEvent.OnGameStateChange, OnGameStateChange);
        GEventCenter.Unsubscribe(GameEvent.OnNewStage, OnNewGameStage);
        GEventCenter.Unsubscribe(GameEvent.OnGameReset, Reset);
        GEventCenter.Unsubscribe<List<Transform>>(GameEvent.OnSpawnPointsInit, InitSpawnTimerPos);
        GEventCenter.Unsubscribe<Player>(GameEvent.OnPlayerJoin, OnPlayerJoin);
        GEventCenter.Unsubscribe(GameEvent.OnMenuUp, GoUp);
        GEventCenter.Unsubscribe(GameEvent.OnMenuDown, GoDown);
    }

    /// <summary>
    ///
    /// </summary>
    private void Update()
    {
        if (ReadyTimer > 0f)
        {
            ReadyTimer = UpdateTimer(ReadyTimer, UI_ReadyTimer.GetComponent<Text>());

            if (ReadyTimer == 0f)
            {
                UI_ReadyTimer.SetActive(false);
            }
        }

        if (startingTimer > 0f)
        {
            startingTimer = UpdateTimer(startingTimer, UI_StartingTimer.GetComponent<Text>());

            // If timer reached 0, spawn the players
            if (startingTimer == 0f)
            {
                //PlayersManager.Instance.SpawnAllPlayers();
                UI_StartingTimer.SetActive(false);
            }
        }

        // For each spawning timer, update it if it is active
        for (int i = 0; i < SpawningTimers.Count; i++)
        {
            if (SpawningTimers[i] > 0f)
            {
                SpawningTimers[i] = UpdateTimer(SpawningTimers[i], Text_SpawningTimers[i]);

                // If timer reached 0, spawn the corresponding player
                if (SpawningTimers[i] == 0f)
                {
                    PlayersManager.Instance.SpawnPlayer(PlayersManager.Instance.Players[i]);
                    UI_SpawningTimers[i].SetActive(false);
                }
            }
        }
    }
    private void OnGameStateChange(GameState _subState)
    {
        switch (_subState)
        {
            case GameState.MainMenu:
                ShowMainMenu();
                break;
            case GameState.LobbyWaiting:
                CancelReadyTimer();
                break;
            case GameState.AllReady:
                StartReadyTimer();
                break;
            case GameState.PrePlayCountdown:
                StartTimer();
                break;
            case GameState.InPlay:
                ResumeGame();
                break;
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.EndStage:
                break;
            case GameState.ScoreScreen:
                break;
            case GameState.EndRound:
                break;
            default:
                break;
        }
    }
    public void ShowMainMenu()
    {
        MainMenu.Activate();
    }
    private void HideMainMenu()
    {
        MainMenu.Deactivate();
    }


    /// <summary>
    /// Init Player spawn timer position at stage start
    /// </summary>
    public void InitSpawnTimerPos(List<Transform> _spawnPoints)
    {
        for (int i = 0; i < UI_SpawningTimers.Count; i++)
        {
            Vector3 _screenPos = Camera.main.WorldToScreenPoint(_spawnPoints[i].position);
            UI_SpawningTimers[i].GetComponent<RectTransform>().position = _screenPos;
        }
    }

    public void StartReadyTimer()
    {
        ReadyTimer = 3f;
        UI_ReadyTimer.SetActive(true);
    }

    public void CancelReadyTimer()
    {
        UI_ReadyTimer.SetActive(false);
    }

    /// <summary>
    ///Active Start Timer at beginning of the stage
    /// </summary>
    public void StartTimer()
    {
        UI_StartingTimer.SetActive(true);
        startingTimer = 3f;

        // When using the starting timer, also enable every spawn timer
        for (int i = 0; i < SpawningTimers.Count; i++)
        {
            StartSpawnTimer(i);
        }
    }

    /// <summary>
    ///Start timer of player spawn
    /// </summary>
    public void StartSpawnTimer(int _playerIndex)
    {
        // Enable the timer game objects
        UI_SpawningTimers[_playerIndex].SetActive(true);

        // Update the scale according to the camera distance
        float _newScale = (22f / Camera.main.orthographicSize) * 1.1f;
        UI_SpawningTimers[_playerIndex].GetComponent<RectTransform>().localScale = new Vector3(_newScale, _newScale, _newScale);

        // Init the timer value
        SpawningTimers[_playerIndex] = 3f;
    }


    /// <summary>
    ///     Update the text of the timer UI and returns the timer's updated value
    /// </summary>
    private float UpdateTimer(float _timer, Text _text)
    {
        // Clamp the timer to minimum 0 and maximum 3
        _timer = Mathf.Clamp(_timer - Time.deltaTime, 0f, 3f);

        // If the timer is less or equal to 1, the timer shows 1
        if (_timer <= 1f)
        {
            _text.text = "1";
        }
        // If the timer is less or equal to 2, the timer shows 2
        else if (_timer <= 2f)
        {
            _text.text = "2";
        }
        // If the timer is less or equal to 3, the timer shows 3
        else if (_timer <= 3f)
        {
            _text.text = "3";
        }

        return _timer;
    }


    /// <summary>
    /// print screen with winner player
    /// </summary>
    public void PrintWinnerScreen(bool _bool, int _indexWinner)
    {
        WinnerScreen.SetActive(_bool);

        if (_bool)
        {
            WinnerScreen_WinnerName.text = "Player " + (_indexWinner + 1).ToString();
            WinnerScreen_WinnerName.color = _playerColors[_indexWinner];
        }
    }

    /// <summary>
    /// Show winner screen if player play alone
    /// </summary>

    public void PrintWinnerScreen_Alone(bool _bool)
    {
        WinnerScreen_Alone.SetActive(_bool);
    }

    public void OnPlayerJoin(Player _player)
    {
        int playerIndex = _player.PlayerData.PlayerIndex;
        Color playerColor = _playerColors[playerIndex];
        PlayerScores[playerIndex].gameObject.SetActive(true);
        PlayerScores[playerIndex].SetColor(playerColor);
        PlayerScores[playerIndex].Player = _player;
        Text_SpawningTimers[playerIndex].GetComponent<Text>().color = playerColor;
        // Add its timer reference to SpawningTimers in MenuManager
        SpawningTimers.Add(0f);
        //Spawn player indicator
        CreatePlayerIndicator(_player, playerColor);
    }

    /// <summary>
    /// Spawn player indicator
    /// </summary>
    /// <param name="_player">player spawned</param>
    public void CreatePlayerIndicator(Player _player,Color _playerColor)
    {
        GameObject indiceIcon = Instantiate(_playerIndicator,this.transform);
        PlayerIndicator playerIndicator = indiceIcon.GetComponent<PlayerIndicator>();
        if (playerIndicator)
        {
            playerIndicator.Init(_player,_playerColor);
        }
    }

    /// <summary>
    /// Show score screen at end of the stage
    /// </summary>
    /// <param name="_bool"></param>
    public void PrintScoreScreen(bool _bool)
    {
        ScoreScreen.SetActive(_bool);
        AudioManager.Instance.ChangeParam(1);
        if (_bool)
        {
            for (int i = 0; i < PlayersManager.Instance.Players.Count; i++)
            {
                PlayerScores[i].SetScore(GameManager.Instance.PlayerScores[i]);
            }
        }

        if (GameManager.Instance.PlayerHasWon)
        {
            int _indexWinner = GameManager.Instance.IndexWinner;
            Text_Scoreboard.SetActive(false);
            Text_PlayerHasWon.SetActive(true);
            Text_PlayerHasWon.GetComponent<Text>().color = _playerColors[_indexWinner];

            _indexWinner += 1;
            string _playerWon = "J" + _indexWinner.ToString() + " has won!";
            Text_PlayerHasWon.GetComponent<Text>().text = _playerWon;
        }
    }


    public void Reset()
    {
        PrintScoreScreen(false);
        MainMenu.Deactivate();

        // For each player
        for (int i = 0; i < PlayersManager.Instance.Players.Count; i++)
        {
            Destroy(UI_SpawningTimers[i]);
        }

        Destroy(UI_StartingTimer);
    }

    /// <summary>
    /// Call On new game stage start
    /// </summary>
    private void OnNewGameStage()
    {
        PrintScoreScreen(false);
    }

    private void PauseGame()
    {
        PauseMenu.Activate();
    }

    public void ResumeGame()
    {
        PauseMenu.Deactivate();
    }


    private void GoUp()
    {
        MainMenu.GoUp();
        Debug.Log("MainMenu GoUp");
    }

    private void GoDown()
    {
        MainMenu.GoDown();
        Debug.Log("MainMenu GoDown");
    }
}
