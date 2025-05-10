using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

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
    [System.NonSerialized] public Player StageWinner;                                   // Winning player of the round reference

    Guid taskId; //ID for time task

    /// <summary>
    /// If the player is play alone
    /// </summary>
    public bool IsPlayAlone = true;
    public Player RoundWinner;                                                        //Game round winner

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
                print("Game state change:" + _gameState.ToString());
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
        GEventCenter.Subscribe<Player>(GameEvent.OnStageEnd, EndOfStage);
        GEventCenter.Subscribe(GameEvent.OnStartInput,OnStartInput);
    }

    private void UnsribeEvents()
    {
        GEventCenter.Unsubscribe<GameScene>(GameEvent.OnLoadScene, OnSceneLoad);
        GEventCenter.Unsubscribe<Player>(GameEvent.OnPlayerJoin, OnNewPlayerJoin);
        GEventCenter.Unsubscribe<Player>(GameEvent.OnStageEnd, EndOfStage);
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
    private void StartNewStage()
    {
        GEventCenter.Invoke(GameEvent.OnNewStage);
        //Start Stage count down
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
    /// End the current game round, and prints out the winner screen
    /// </summary>
    public void EndOfStage(Player _winner)
    {
        GameState = GameState.EndStage;
        StageWinner = _winner;
    }

    /// <summary>
    /// Show score screen at the end of each stage
    /// </summary>
    public void ScoreScreen()
    {
        if (IsPlayAlone) //If player is play alone, we will not show score screen, and check to play next level directly
        {
            CheckRoundEnd();
            return;
        }
        StageWinner.PlayerData.PlayerScore += 1;

        GameState = GameState.ScoreScreen;

        GEventCenter.Invoke(GameEvent.OnShowScore);

        int _indexStageWinner = StageWinner.PlayerData.PlayerIndex;

        StageWinner.VoiceController.PlayVictory();
    }

    /// <summary>
    /// Check if there's player win the game round, then end of round
    /// Actual The rule for ending a game round is: 
    /// 1.the game ends if a player reaches 5 wins
    /// or 
    /// 2. if all the levels (or scenes) have been played.
    ///However, if all the scenes have been played and two players are tied with the same score, we’ll keep playing random levels until one player has a higher score than the others.
    /// </summary>
    private void CheckRoundEnd()
    {
        var players = PlayersManager.Instance.Players;
        bool allLevelsPlayed = LevelManager.Instance.AllLevelsPlayed();

        // Check if any player has reached 5 wins
        Player winPlayer = players.FirstOrDefault(p => p.PlayerData.PlayerScore >= 1);
        if (winPlayer != null)
        {
            GEventCenter.Invoke(GameEvent.OnGameRoundEnd);
            RoundWinner = winPlayer;
            return;
        }

        // Determine highest score and who has it
        int highestScore = players.Max(p => p.PlayerData.PlayerScore);
        List<Player> topPlayers = players
            .Where(p => p.PlayerData.PlayerScore == highestScore)
            .ToList();

        if (allLevelsPlayed)
        {
            if (topPlayers.Count == 1)
            {
                // Only one top scorer and levels done => end game
                GEventCenter.Invoke(GameEvent.OnGameRoundEnd, topPlayers[0]);
                RoundWinner = topPlayers[0];
            }
            else
            {
                // Tie and all levels played => keep playing
                LevelManager.Instance.LoadNextLevel();
            }
        }
        else
        {
            // Still levels left => keep playing
            LevelManager.Instance.LoadNextLevel();
        }
    }

    private void EnterOutroScene()
    {
        GameState = GameState.Outro;
    }

    /// <summary>
    ///     Reset the game
    /// </summary>
    public void ResetGame()
    {
        GEventCenter.Invoke(GameEvent.OnGameReset);
        GameState = GameState.MainMenu;
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
    public bool IsInGameplay()
    {
        return GameState >= GameState.PrePlayCountdown && GameState < GameState.EndStage;
    }

    /// <summary>
    /// Whether Actual game state is in menu screen
    /// </summary>
    /// <returns></returns>
    public bool IsInMenu()
    {
        return GameState == GameState.MainMenu && GameState == GameState.Paused && GameState == GameState.ScoreScreen;
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
                ScoreScreen();
                break;
            case GameState.ScoreScreen:
                CheckRoundEnd();
                break;
            case GameState.Outro:
                ResetGame();
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
                StartNewStage();
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

    private void OnNewPlayerJoin(Player newPlayer)
    {
        if (PlayersManager.Instance.Players.Count > 1)
        {
            IsPlayAlone = false;
        }
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

    // #endregion
}
