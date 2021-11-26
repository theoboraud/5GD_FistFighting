using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static MenuManager Instance;
    [SerializeField] private GameObject ScoreScreen;
    [SerializeField] private GameObject ScoreScreen_Alone;
    public Text ScoreScreen_WinnerName;
    public GameObject UI_StartingTimer;                                                 // Reference to the starting timer
    public List<GameObject> UI_SpawningTimers = new List<GameObject>();                 // Reference to the spawning timers of each player
    public List<Text> Text_SpawningTimers = new List<Text>();                           // Reference to the Text component of each spawning timers
    public List<Color> PlayerColors;

    [Header("Menu Screens")]
    [SerializeField] private CharacterSelectMenu CharacterSelectMenu;
    [SerializeField] private MainMenu MainMenu;
    [SerializeField] private OptionsMenu OptionsMenu;
    [SerializeField] private LevelSelectMenu LevelSelectMenu;

    [Header("Variables")]
    public List<int> CharacterSkinIndex = new List<int>();
    private List<bool> playersReady = new List<bool>();
    private Menu activeMenu;
    private float startingTimer = 0f;                                                   // Contains the general spawn timer when starting a new level
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

            // Init the active menu as the main menu
            InitMenu();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /// <summary>
    ///
    /// </summary>
    private void InitMenu()
    {
        DeactivateAllMenu();
        switch (GameManager.Instance.GlobalGameState)
        {
            case GlobalGameState.MainMenu:
                GoTo_MainMenu();
                break;
            case GlobalGameState.CharacterSelectMenu:
                GoTo_CharacterSelectMenu();
                break;
            case GlobalGameState.LevelSelectMenu:
                GoTo_LevelSelectMenu();
                break;
            case GlobalGameState.OptionsMenu:
                GoTo_OptionsMenu();
                break;
            default:
                break;
        }
    }


    /// <summary>
    ///
    /// </summary>
    private void Update()
    {
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
        for (int i = 0; i < SpawningTimers.Count; i++)
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
        float _newScale = 10f / Camera.main.orthographicSize;
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
    public void GoTo_MainMenu()
    {
        if (activeMenu != null)
        {
            activeMenu.Deactivate();
        }
        activeMenu = MainMenu;
        activeMenu.Activate();

        // Update GlobalGameState
        GameManager.Instance.GlobalGameState = GlobalGameState.MainMenu;
    }


    /// <summary>
    ///
    /// </summary>
    public void GoTo_CharacterSelectMenu()
    {
        activeMenu.Deactivate();
        activeMenu = CharacterSelectMenu;
        activeMenu.Activate();

        // Update GlobalGameState
        GameManager.Instance.GlobalGameState = GlobalGameState.CharacterSelectMenu;
    }


    /// <summary>
    ///
    /// </summary>
    public void GoTo_OptionsMenu()
    {
        activeMenu.Deactivate();
        activeMenu = OptionsMenu;
        activeMenu.Activate();

        // Update GlobalGameState
        GameManager.Instance.GlobalGameState = GlobalGameState.OptionsMenu;
    }


    /// <summary>
    ///
    /// </summary>
    public void GoTo_LevelSelectMenu()
    {
        activeMenu.Deactivate();
        activeMenu = LevelSelectMenu;
        activeMenu.Activate();

        // Update GlobalGameState
        GameManager.Instance.GlobalGameState = GlobalGameState.LevelSelectMenu;
    }


    /// <summary>
    ///
    /// </summary>
    private void DeactivateAllMenu()
    {
        MainMenu.Deactivate();
        CharacterSelectMenu.Deactivate();
        LevelSelectMenu.Deactivate();
        OptionsMenu.Deactivate();
    }


    /// <summary>
    ///
    /// </summary>
    public void PrintScoreScreen(bool _bool, int _indexWinner)
    {
        ScoreScreen.SetActive(_bool);

        if (_bool)
        {
            ScoreScreen_WinnerName.text = "Player " + (_indexWinner + 1).ToString();
            ScoreScreen_WinnerName.color = PlayerColors[_indexWinner];
        }
    }


    public void PrintScoreScreen_Alone(bool _bool)
    {
        ScoreScreen_Alone.SetActive(_bool);
    }


    /// <summary>
    ///
    /// </summary>
    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
