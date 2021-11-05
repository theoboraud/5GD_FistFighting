using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class CharacterSelectMenu : Menu
{
    public List<GameObject> GO_PlayerSelectors = new List<GameObject>();
    private List<PlayerSelector> PlayerSelectors = new List<PlayerSelector>();

    public override void Init()
    {
        base.Init();
        this.NavType = NavType.Horizontal;

        for (int i = 0; i < GO_PlayerSelectors.Count; i++)
        {
            PlayerSelectors[i] = GO_PlayerSelectors[i].GetComponent<PlayerSelector>();
        }
    }

    public void ButtonReady()
    {
        MenuManager.Instance.GoTo_CharacterSelectMenu();
    }

    private bool AllPlayersReady()
    {
        if (PlayersManager.Instance.Players.Count > 0)
        {
            for (int i = 0; i < PlayersManager.Instance.Players.Count; i++)
            {
                if (PlayersManager.Instance.Players[i].PlayerGameState != PlayerGameState.Ready)
                {
                    return false;
                }
                return true;
            }
        }
        return false;
    }

    public override void Validate()
    {
        if (AllPlayersReady())
        {
            MenuManager.Instance.GoTo_LevelSelectMenu();
        }
    }

    public override void Activate()
    {
        base.Activate();
        SpawnPlayerSelectors();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        DespawnPlayerSelectors();
    }

    public void SpawnPlayerSelectors()
    {
        /*for (int i = PlayerSelectors.Count; i < PlayersManager.Instance.Players.Count; i++)
        {
            GO_PlayerSelectors[i].SetActive(true);
            PlayerSelectors[i].Player = PlayersManager.Instance.Players[i];
            PlayersManager.Instance.Players[i].PlayerSelector = PlayerSelectors[i];
        }*/
    }

    public void DespawnPlayerSelectors()
    {
        /*for (int i = 0; i < PlayersManager.Instance.Players.Count; i++)
        {
            GO_PlayerSelectors[i].SetActive(false);
            PlayerSelectors[i].Player = null;
            PlayersManager.Instance.Players[i].PlayerSelector = null;
        }*/
    }
}
