using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private GameObject scoreText;                                          // Reference to the player score text game object (J1, J2...)
    //[SerializeField] private List<GameObject> scoreTokenFillers = new List<GameObject>();   // References to all token fillers game objects
    [SerializeField] private List<GameObject> GO_Hearts = new List<GameObject>();
    [SerializeField] private GameObject GO_NbLives;
    [SerializeField] private Text nbLives;
    [SerializeField] private GameObject GO_Cross;

    [Header("Variables")]
    [System.NonSerialized] public int PlayerIndex;


    /// <summary>
    ///     Add the PlayerUI
    /// </summary>
    public void AddPlayerUI(int _playerIndex, Color _playerColor)
    {
        this.gameObject.SetActive(true);
        PlayerIndex = _playerIndex;
        this.gameObject.GetComponent<Text>().color = _playerColor;
        Init();
    }


    /// <summary>
    ///     Reset all PlayerUI objects and values
    /// </summary>
    public void Init()
    {
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


    /// <summary>
    ///     Update the lives on the player UI
    /// </summary>
    public void UpdateLivesUI()
    {
        if (GameManager.Instance.GlobalGameState is GlobalGameState.InPlay)
        {
            int _playerLives = PlayersManager.Instance.PlayersLives[PlayerIndex];

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
