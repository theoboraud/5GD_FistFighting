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


    public void PlayerReachedArrival(Player _player)
    {
        LevelManager.Instance.LoadRandomLevel();
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
