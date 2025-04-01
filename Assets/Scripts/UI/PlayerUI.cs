using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

/// <summary>
/// UI of player, show player's lives
/// ??We have to separate players UI under control of their own player's class//TODO
/// </summary>

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> GO_Hearts = new List<GameObject>();
    [SerializeField] private GameObject GO_NbLives;
    [SerializeField] private Text nbLives;
    [SerializeField] private GameObject GO_Cross;
    [SerializeField] private GameObject PlayerIndicator;
    [SerializeField] private GameObject GO_IsReady;

    [Header("Variables")]
    [System.NonSerialized] public int PlayerIndex;

    private Vector2[] uiPositions = {
        new Vector2(50, 0),   // right up
        new Vector2(-50, 0),  // left up
        new Vector2(50,50),  // right down
        new Vector2(-50, 50)  // left down
    };

    private Vector2[] anchors = {
        new Vector2(0, 1),  //  left up
        new Vector2(1, 1),  // right up
        new Vector2(0, 0), //  left down
        new Vector2(1, 0),  //  right down
    };

    private Player _player;


    /// <summary>
    /// Add the PlayerUI (with player index, player color, maybe player name in the future)
    /// </summary>
    public void AddPlayerUI(int _playerIndex, Color _playerColor)
    {
        this.gameObject.SetActive(true);
        PlayerIndex = _playerIndex;

        //Set player name text
        Text playerNameTxt = this.gameObject.GetComponent<Text>();
        playerNameTxt.text = "J" + (_playerIndex + 1);

        //Set indicator color
        PlayerIndicator.GetComponent<Image>().color = _playerColor;

        foreach (GameObject go in GO_Hearts) { 
        go.GetComponent<Image>().color = _playerColor;
        }
        RectTransform rect = GetComponent<RectTransform>();

        // Set anchor
        rect.anchorMin = anchors[_playerIndex];
        rect.anchorMax = anchors[_playerIndex];
        rect.pivot = anchors[_playerIndex];

        // Set rect transform
        rect.anchoredPosition = uiPositions[_playerIndex];

        Init();
    }


    /// <summary>
    ///     Reset all PlayerUI objects and values
    /// </summary>
    public void Init()
    {
        InitCallbacks();

        _player = GetComponent<Player>();
        //Disable is ready btn
        GO_IsReady.SetActive(false);

        if (LevelManager.Instance.CurrentSceneIndex > 0)
        {
            // Set the game objects visibility
            GO_Hearts[0].SetActive(true);
            GO_Hearts[1].SetActive(true);
            GO_Hearts[2].SetActive(true);
            //GO_NbLives.SetActive(true);
            GO_Cross.SetActive(false);

            // Reset the number of lives
            nbLives.text = GameManager.Instance.ParamData.PARAM_Player_Lives.ToString();
        }
        else
        {
            GO_Hearts[0].SetActive(false);
            GO_Hearts[1].SetActive(false);
            GO_Hearts[2].SetActive(false);
            //GO_NbLives.SetActive(false);
            GO_Cross.SetActive(false);
        }

        Invoke("UpdateLivesUI", 0.3f);
    }

    
    private void OnDisable()
    {
        RemoveCallBacks();
    }

    private void InitCallbacks()
    {
        GEventCenter.Subscribe<GameScene>(GameEvent.OnLoadScene, OnNewSceneLoad);
    }
    private void RemoveCallBacks()
    {
        GEventCenter.Unsubscribe<GameScene>(GameEvent.OnLoadScene, OnNewSceneLoad);
    }

    public void GetReady(bool _isReady)
    {
        Debug.Log("UI set ready" + _isReady);
        GO_IsReady.SetActive(_isReady);
    }

    private void OnNewSceneLoad(GameScene _scene)
    {
        if (_scene is GameScene.Playable)
        {
            UpdateLivesUI();
        }
    }

    /// <summary>
    ///     Update the lives on the player UI
    /// </summary>
    public void UpdateLivesUI()
    {
        if (GameManager.Instance.GlobalGameState is GlobalGameState.InPlay)
        {
	        if (_player == null) return;
            int _playerLives = _player.PlayerData.PlayerLives;

            if (_playerLives > 0)
            {
                nbLives.text = _playerLives.ToString();
                if (_playerLives == 3)
                {
                    SetHeart(GO_Hearts[0], true);
                    SetHeart(GO_Hearts[1], true);
                    SetHeart(GO_Hearts[2], true);
                }
                if (_playerLives == 2)
                {
                    SetHeart(GO_Hearts[0], true);
                    SetHeart(GO_Hearts[1], true);
                    SetHeart(GO_Hearts[2], false);
                }
                if (_playerLives == 1)
                {
                    SetHeart(GO_Hearts[0], true);
                    SetHeart(GO_Hearts[1], false);
                    SetHeart(GO_Hearts[2], false);
                }
            }
            else
            {
                //nbLives.text = "";
                Eliminated();
            }

            // Deprecated
            /*if (_playerLives == 1 && GameManager.Instance.ParamData.PARAM_Player_Lives > 1)
            {
                nbLives.color = Color.red;
            }
            else
            {
                nbLives.color = Color.black;
            }*/
        }
    }


    /// <summary>
    ///     Enable the UI behaviour when the player has 0 life left (i.e. is eliminated)
    /// </summary>
    public void Eliminated()
    {
        SetHeart(GO_Hearts[0], false);
        SetHeart(GO_Hearts[1], false);
        SetHeart(GO_Hearts[2], false);
        //GO_NbLives.SetActive(false);
        GO_Cross.SetActive(true);
    }

    /// <summary>
    ///     Change heart display
    /// </summary>
    public void SetHeart(GameObject heartGO, bool isAlive)
    {
        Image img = heartGO.GetComponent<Image>();

        if (isAlive)
        {
            Color color = Color.white;
            color.a = 1f;
            img.color = color;
        }
        else
        {
            Color color = Color.black;
            color.a = 0.3f;
            img.color = color;
        }
    }
}
