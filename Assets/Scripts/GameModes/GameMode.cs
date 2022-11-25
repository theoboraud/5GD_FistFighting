using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMode : ScriptableObject
{
    [SerializeField]
    protected int minNbPlayers;
    [SerializeField]
    protected int maxNbPlayers;

    [SerializeField]
    protected List<string> levelName = new List<string>();

    private List<string> loadedLevels = new List<string>();

    [SerializeField]
    protected UnityEvent OnRoundEnd;


    // Returns the next level to load
    public string GetNextLevel()
    {
        return "";
    }

    // Init game mode once the level has been loaded
    public virtual void InitGameMode()
    {

    }

    // Check if the round should end
    public virtual void OnPlayerDeath()
    {
        /*if (PlayersManager.instance.Players.Count <= 1)
        {
            OnRoundEnd();
        }*/
    }
}
