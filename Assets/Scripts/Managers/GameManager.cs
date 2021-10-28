using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [System.NonSerialized] public static GameManager Instance;
    public GlobalGameState GlobalGameState;
    public ParamData ParamData;

    // Init as a singleton
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


    public void EndOfRound(Player _winner)
    {
        // TODO: Implement score screen, victory/defeat feedbacks...
        GlobalGameState = GlobalGameState.ScoreScreen;
        MenuManager.Instance.PrintScoreScreen(true);
    }


    public void NewGameRound()
    {
        // TODO: Implement loading screen...
        LevelManager.Instance.LoadRandomLevel();
        MenuManager.Instance.PrintScoreScreen(false);
        GlobalGameState = GlobalGameState.InPlay;
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
