using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
///     Class used as a game reference : contains game state, game parameters, feedback manager...
/// </summary>
public class GameManager : MonoBehaviour
{
    // #region ==================== CLASS VARIABLES ====================

    [Header("References")]
    [System.NonSerialized] public static GameManager Instance;      // Singleton reference
    public GlobalGameState GlobalGameState;                         // Current state of the game (InPlay, WinnerScreen, MainMenu, CharacterSelectMenu, LevelSelectMenu or OptionsMenu)
    public ParamData ParamData;                                     // Game parameters customizable directly via the ParamData file
    public FeedbackManager Feedback;                                // Feedback manager reference, used to instantiate VFX and audio effects

    [Header("Variables")]
    [System.NonSerialized] public List<int> PlayerScores = new List<int>();             // Score value of each player
    [System.NonSerialized] public bool PlayerHasWon = false;                            // Whether or not a player has won
    [System.NonSerialized] public int IndexWinner;
    [System.NonSerialized] public Player RoundWinner;                                   // Winning player of the round reference

    // #endregion



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

            Invoke("Init", 0.1f);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /// <summary>
    ///     Init all managers
    /// </summary>
    private void Init()
    {
        GlobalGameState = GlobalGameState.MainMenu;
        LevelManager.Instance.Init();
        MenuManager.Instance.Init();
        PlayersManager.Instance.Init();
    }

    // #endregion



    // #region ==================== GAME FUNCTIONS ====================

    /// <summary>
    ///     Starts a new game round
    /// </summary>
    public void NewGameRound()
    {
        // TODO: Implement loading screen...
        if (GlobalGameState == GlobalGameState.ScoreScreen || LevelManager.Instance.IsIntroScene())
        {
            // Still in the first level
            PlayersManager.Instance.PlayersDeathOrder.Clear();
            Feedback.ResetAllVFX();
            MenuManager.Instance.PrintScoreScreen(false);

            // Load the new level
            LevelManager.Instance.LoadNextLevel();

            // Change game state
            Invoke("SetStateToInPlay", 0.1f);
        }
    }


    public void ScoreScreen()
    {
        // Disable the winner screen and reset all players
        MenuManager.Instance.PrintWinnerScreen(false, 0);
        MenuManager.Instance.PrintWinnerScreen_Alone(false);
        PlayersManager.Instance.ResetSpawnedPlayers();

        if (LevelManager.Instance.CurrentSceneIndex > 2)
        {
            GlobalGameState = GlobalGameState.Null;

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
                MenuManager.Instance.UI.SetActive(false);
            }

            // Print out the score screen
            MenuManager.Instance.PrintScoreScreen(true);


            Invoke("SetStateToScoreScreen", 0.1f);

            // Reset the RoundWinner
            RoundWinner = null;
        }
        else
        {
            GlobalGameState = GlobalGameState.ScoreScreen;

            for (int i = 0; i < PlayersManager.Instance.PlayersAlive.Count; i++)
            {
                PlayersManager.Instance.PlayersAlive[i].Kill();
            }

            Invoke("NewGameRound", 0.1f);
        }
    }


    private void SetStateToScoreScreen()
    {
        GlobalGameState = GlobalGameState.ScoreScreen;
    }


    private void SetStateToInPlay()
    {
        GlobalGameState = GlobalGameState.InPlay;
    }


    /// <summary>
    ///     End the current game round, and prints out the winner screen
    /// </summary>
    public void EndOfRound(Player _winner)
    {
        GlobalGameState = GlobalGameState.WinnerScreen;

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
        LevelManager.Instance.Reset();
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


    public void PlayMode()
    {
        GlobalGameState = GlobalGameState.InPlay;
        PlayersManager.Instance.ChangeMode("Gameplay");
    }


    public void MenuMode(GlobalGameState _menuState)
    {
        GlobalGameState = _menuState;
        PlayersManager.Instance.ChangeMode("Menu");
    }

    // #endregion
}
