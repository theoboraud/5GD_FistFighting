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
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // #endregion



    // #region ==================== GAME FUNCTIONS ====================

    /// <summary>
    ///     Starts a new game round
    /// </summary>
    public void NewGameRound()
    {
        // TODO: Implement loading screen...
        if (GlobalGameState == GlobalGameState.ScoreScreen)
        {
            // Still in the first level
            PlayersManager.Instance.ResetSpawnedPlayers();
            Feedback.ResetAllVFX();
            MenuManager.Instance.PrintWinnerScreen(false, 0);
            MenuManager.Instance.PrintWinnerScreen_Alone(false);

            // Load the new level
            LevelManager.Instance.LoadNextLevel();

            // Change game state
            GlobalGameState = GlobalGameState.InPlay;
        }
    }


    /// <summary>
    ///     End the current game round
    /// </summary>
    public void EndOfRound(Player _winner)
    {
        // TODO: Implement score screen, victory/defeat feedbacks...
        GlobalGameState = GlobalGameState.ScoreScreen;

        if (_winner != null)
        {
            MenuManager.Instance.PrintWinnerScreen(true, PlayersManager.Instance.Players.IndexOf(_winner));
        }
        else
        {
            MenuManager.Instance.PrintWinnerScreen_Alone(true);
        }
    }


    /// <summary>
    ///     Quit the game application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    // #endregion
}
