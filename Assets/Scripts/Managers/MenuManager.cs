using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static MenuManager Instance;
    [SerializeField] private GameObject WinnerScreen;
    [SerializeField] private GameObject WinnerScreen_Alone;
    [SerializeField] private GameObject ScoreScreen;
    [SerializeField] private GameObject Text_Scoreboard;
    [SerializeField] private GameObject Text_PlayerHasWon;
    public Text WinnerScreen_WinnerName;
    public GameObject UI_StartingTimer;                                                 // Reference to the starting timer
    public GameObject UI_ReadyTimer;
    public List<GameObject> UI_SpawningTimers = new List<GameObject>();                 // Reference to the spawning timers of each player
    public List<Text> Text_SpawningTimers = new List<Text>();                           // Reference to the Text component of each spawning timers
    public List<PlayerScore> PlayerScores = new List<PlayerScore>();                    // Reference to the player score of each player
    public List<Color> PlayerColors;                                                    // Reference to the player colors
    public List<PlayerUI> PlayersUI = new List<PlayerUI>();                        // Reference to the UI indicating the number of player lives
    public PauseMenu PauseMenu;                                                         // Reference to the PauseMenu script

    [Header("Menu Screens")]
    public MainMenu MainMenu;

    [Header("Variables")]
    public List<int> CharacterSkinIndex = new List<int>();
    private List<bool> playersReady = new List<bool>();
    private Menu activeMenu;
    private float startingTimer = 0f;                                                   // Contains the general spawn timer when starting a new level
    [System.NonSerialized] public float ReadyTimer = 0f;
    [System.NonSerialized] public List<float> SpawningTimers = new List<float>();       // Contains the spawn timer of each player


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
        // Init the active menu as the main menu
        InitMenu();
    }


    /// <summary>
    ///
    /// </summary>
    private void InitMenu()
    {
        //DeactivateAllMenu();
        MainMenu.Activate();
    }


    /// <summary>
    ///
    /// </summary>
    private void Update()
    {
        if (ReadyTimer > 0f)
        {
            if (PlayersManager.Instance.AllPlayersReady())
            {
                ReadyTimer = UpdateTimer(ReadyTimer, UI_ReadyTimer.GetComponent<Text>());

                if (ReadyTimer == 0f)
                {
                    UI_ReadyTimer.SetActive(false);
                    GameManager.Instance.EndOfRound(null);
                }
            }
            else
            {
                ReadyTimer = 0f;
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


    /// <summary>
    ///
    /// </summary>
    public void InitSpawnTimerPos()
    {
        for (int i = 0; i < UI_SpawningTimers.Count; i++)
        {
            Vector3 _screenPos = Camera.main.WorldToScreenPoint(LevelManager.Instance.SpawnPoints[i].transform.position);
            UI_SpawningTimers[i].GetComponent<RectTransform>().position = _screenPos;
        }
    }


    /// <summary>
    ///
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
    ///
    /// </summary>
    public void StartSpawnTimer(int _playerIndex)
    {
        // Enable the timer game objects
        UI_SpawningTimers[_playerIndex].SetActive(true);

        // Update the scale according to the camera distance
        float _newScale = 22f / Camera.main.orthographicSize;
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
    ///
    /// </summary>
    public void PrintWinnerScreen(bool _bool, int _indexWinner)
    {
        WinnerScreen.SetActive(_bool);

        if (_bool)
        {
            WinnerScreen_WinnerName.text = "Player " + (_indexWinner + 1).ToString();
            WinnerScreen_WinnerName.color = PlayerColors[_indexWinner];
        }
    }


    public void PrintWinnerScreen_Alone(bool _bool)
    {
        WinnerScreen_Alone.SetActive(_bool);
    }


    public void AddPlayer(int _playerIndex)
    {
        UpdateLives();
        PlayerScores[_playerIndex].gameObject.SetActive(true);
        PlayerScores[_playerIndex].SetColor(PlayerColors[_playerIndex]);
        PlayersManager.Instance.Players[_playerIndex].PlayerIndicator.GetComponent<SpriteRenderer>().color = PlayerColors[_playerIndex];
        PlayersManager.Instance.Players[_playerIndex].Outline_SpriteRenderer.color = PlayerColors[_playerIndex];
        PlayersUI[_playerIndex].AddPlayerUI(_playerIndex, PlayerColors[_playerIndex]);
        PlayersManager.Instance.Players[_playerIndex].IsReadyUI(false);
        //UI_PlayersLives[_playerIndex].transform.parent.gameObject.SetActive(true);
        //UI_PlayersLives[_playerIndex].transform.parent.gameObject.GetComponent<Text>().color = PlayerColors[_playerIndex];
    }


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
            Text_PlayerHasWon.GetComponent<Text>().color = PlayerColors[_indexWinner];

            _indexWinner += 1;
            string _playerWon = "J" + _indexWinner.ToString() + " has won!";
            Text_PlayerHasWon.GetComponent<Text>().text = _playerWon;
        }
    }


    /// <summary>
    ///     Update lives UI
    /// </summary>
    public void UpdateLives()
    {
        for (int i = 0; i < PlayersManager.Instance.PlayersLives.Count; i++)
        {
            PlayersUI[i].UpdateLivesUI();
        }
    }


    public void Reset()
    {
        PrintScoreScreen(false);
        MainMenu.Deactivate();

        // For each player
        for (int i = 0; i < PlayersManager.Instance.Players.Count; i++)
        {
            Destroy(PlayersUI[i].gameObject);
            Destroy(UI_SpawningTimers[i]);
        }

        Destroy(UI_StartingTimer);
    }

    public void NewGameRound()
    {
        for (int i = 0; i < PlayersManager.Instance.PlayersLives.Count; i++)
        {
            PlayersUI[i].Init();
        }
    }
}
