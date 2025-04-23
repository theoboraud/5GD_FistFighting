using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

/// <summary>
///     Class used as a game reference : contains game state, game parameters, feedback manager...
/// </summary>
public class GameManager : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public static GameManager Instance;      // Singleton reference
    private GameState _gameState;                      //Current state of the  game (MainMenu,lobby, Inplay...)
    public ParamData ParamData;                                     // Game parameters customizable directly via the ParamData file
    public FeedbackManager Feedback;                                // Feedback manager reference, used to instantiate VFX and audio effects

    [Header("Game Modes")]
    public GameMode GameMode;

    [Header("Variables")]
    [System.NonSerialized] public List<int> PlayerScores = new List<int>();             // Score value of each player
    [System.NonSerialized] public bool PlayerHasWon = false;                            // Whether or not a player has won
    [System.NonSerialized] public int IndexWinner;
    [System.NonSerialized] public Player RoundWinner;                                   // Winning player of the round reference

    Guid taskId; //ID for time task

    /// <summary>
    /// Actual In-Game Sub State (Send state change event on new value set)
    /// </summary>
    public GameState GameState
    {
        get { return _gameState; }
        set
        {
            if (_gameState != value)
            {
                _gameState = value;
                GEventCenter.Invoke<GameState>(GameEvent.OnGameStateChange, _gameState);
            }
        }
    }
    // #endregion

    //#endregion

    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Init as a singleton
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
    private void Start()
    {
        InitGame();
    }

    /// <summary>
    ///     Init all managers
    /// </summary>
    private void InitGame()
    {
        GameState = GameState.MainMenu;
        PlayersManager.Instance.Init();
        LevelManager.Instance.Init();
        MenuManager.Instance.Init();
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
        GEventCenter.Subscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
        GEventCenter.Subscribe<Player>(GameEvent.OnPlayerJoin, OnNewPlayerJoin);
        GEventCenter.Subscribe(GameEvent.OnGameReset, ResetGame);
        GEventCenter.Subscribe(GameEvent.OnShowScore, ScoreScreen);
        GEventCenter.Subscribe(GameEvent.OnStartInput,OnStartInput);
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
        GEventCenter.Unsubscribe<Player>(GameEvent.OnPlayerJoin, OnNewPlayerJoin);
        GEventCenter.Unsubscribe(GameEvent.OnGameReset, ResetGame);
        GEventCenter.Unsubscribe(GameEvent.OnShowScore, ScoreScreen);
        GEventCenter.Unsubscribe(GameEvent.OnStartInput, OnStartInput);
    }

    // #endregion



    // #region ==================== GAME FUNCTIONS ====================

    #region Game Stats
    private void EnterIntroScene()
    {
        GameState = GameState.Intro;
    }
    private void EnterMainMenu()
    {
        GameState = GameState.MainMenu; //Main menu first, press start to lobby
        GEventCenter.Invoke<string>(GameEvent.OnInputModeChange, "Menu");
    }
    public void EnterLobby()
    {
        GameState = GameState.LobbyWaiting;
        GEventCenter.Invoke(GameEvent.OnEnterLobby);
        GEventCenter.Invoke<string>(GameEvent.OnInputModeChange, "Gameplay");
    }
    private void CheckAllPlayerReady()
    {
        if (PlayersManager.Instance.AllPlayersReady()) //If all players are ready
        {
            GameState = GameState.AllReady;
            taskId = TimerUtility.Invoke(3,StartNewGameRound); //Start a New Game Round In 3 sec
        }
    }
    private void CancelAllReady()
    {
        GameState = GameState.LobbyWaiting;
        TimerUtility.CancelInvoke(taskId); //Cancel task of start new game round
    }
    /// <summary>
    /// Starts a new game stage
    /// </summary>
    private void StartNewGameRound()
    {
        GEventCenter.Invoke(GameEvent.OnNewGameRound);
    }

    /// <summary>
    /// Start Stage count down once we enter the stage
    /// </summary>
    private void StartStageCountDown()
    {
        GameState = GameState.PrePlayCountdown; //New game stage
        TimerUtility.Invoke(3, BeginGameStage);
    }

    /// <summary>
    /// Call at Game stage start
    /// </summary>
    private void BeginGameStage()
    {
        GameState = GameState.InPlay;
    }

    /// <summary>
    /// Call to resume gameplay when game paused
    /// </summary>
    public void ResumeGameplay()
    {
        GameState = GameState.InPlay;
        Time.timeScale = 1f;
        GEventCenter.Invoke<string>(GameEvent.OnInputModeChange, "Gameplay");
    }

    /// <summary>
    /// Call to pauseGame
    /// </summary>
    public void PauseGame()
    {
        GameState = GameState.Paused;
        Time.timeScale = 0f;
        GEventCenter.Invoke<string>(GameEvent.OnInputModeChange, "Menu");
    }

    /// <summary>
    /// Show score screen at the end of each stage
    /// </summary>
    public void ScoreScreen()
    {
        // Disable the winner screen and reset all players
        MenuManager.Instance.PrintWinnerScreen(false, 0);
        MenuManager.Instance.PrintWinnerScreen_Alone(false);
        PlayersManager.Instance.ResetSpawnedPlayers();

        if (LevelManager.Instance.CurrentSceneIndex > 2)
        {
            GameState = GameState.EndRound;

            int _indexRoundWinner = PlayersManager.Instance.Players.IndexOf(RoundWinner);
            int _winnerIndex = -1;
            PlayerScores[_indexRoundWinner] += 1;
            RoundWinner.VoiceController.PlayVictory();
            AudioManager.Instance.PlayWinSound();

            if (PlayerScores[_indexRoundWinner] >= 5)
            {
                _winnerIndex = _indexRoundWinner;
            }

            if (_winnerIndex > -1)
            {
                IndexWinner = _winnerIndex;
                PlayerHasWon = true;
                MenuManager.Instance.InGameUI.SetActive(false);
            }

            // Print out the score screen
            MenuManager.Instance.PrintScoreScreen(true);
            Debug.Log("Screen Score");

            // Reset the RoundWinner
            RoundWinner = null;
        }
        else //???
        {
            GameState = GameState.EndRound;

            for (int i = 0; i < PlayersManager.Instance.PlayersAlive.Count; i++)
            {
                PlayersManager.Instance.PlayersAlive[i].Kill();
            }
        }
    }

    private void EnterOutroScene()
    {
        GameState = GameState.Outro;
    }
    #endregion

    #region Global Game Stats
    /// <summary>
    /// Whether Actual game state is in lobby
    /// </summary>
    public bool IsInLobby()
    {
        return GameState is GameState.LobbyWaiting || GameState is GameState.AllReady ;
    }
    /// <summary>
    /// Whether Actual game state is in Gameplay
    /// </summary>
    public bool IsInGameplay( )
    {
        return GameState >= GameState.PrePlayCountdown && GameState < GameState.EndRound;
    }
    #endregion

    private void OnStartInput()
    {
        Debug.Log("Start pressed!!!");
        switch (GameState)
        {
            case GameState.MainMenu:
                EnterLobby();
                break;
            case GameState.LobbyWaiting:
                CheckAllPlayerReady();
                break;
            case GameState.AllReady:
                CancelAllReady();
                break;
            case GameState.InPlay:
                PauseGame();
                break;
            case GameState.Paused:
                ResumeGameplay();
                break;
            case GameState.EndStage:
                break;
            case GameState.ScoreScreen:
                break;
            case GameState.EndRound:
                StartNewGameRound();
                break;
            default:
                break;
        }
    }
    private void OnSceneLoad(GameScene _scene)
    {
        switch (_scene)
        {
            case GameScene.Lobby:
                EnterMainMenu();
                break;
            case GameScene.Playable:
                StartStageCountDown();
                break;
            case GameScene.Intro:
                EnterIntroScene();
                break;
            case GameScene.Outro:
                EnterOutroScene();
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///     End the current game round, and prints out the winner screen
    /// </summary>
    public void EndOfRound(Player _winner)
    {
        GameState = GameState.EndStage;

        if (_winner != null)
        {
            RoundWinner = _winner;
            MenuManager.Instance.PrintWinnerScreen(true, PlayersManager.Instance.Players.IndexOf(_winner));
            PlayersManager.Instance.KillOtherPlayers(_winner);
        }
        else
        {
            RoundWinner = PlayersManager.Instance.Players[0];
            MenuManager.Instance.PrintWinnerScreen_Alone(true);
        }

        // TODO: Delete this, and implement a timer "3, 2, 1" when all players are ready to load a new level
        if (LevelManager.Instance.IsLobbyScene())
        {
            for (int i = 0; i < PlayersManager.Instance.PlayersAlive.Count; i++)
            {
                PlayersManager.Instance.PlayersAlive[i].Kill();
            }
            ScoreScreen();
        }
    }


    /// <summary>
    ///     Reset the game
    /// </summary>
    public void ResetGame()
    {
        GEventCenter.Invoke(GameEvent.OnGameReset);
        GameState = GameState.InPlay;
    }

    /// <summary>
    ///     Quit the game application
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    private void OnNewPlayerJoin(Player _player)
    {
        PlayerScores.Add(0);
    }
    // #endregion
}
