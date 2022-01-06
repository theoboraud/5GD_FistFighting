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
    [SerializeField] private GameObject GO_Heart;
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
            GO_Heart.SetActive(true);
            GO_NbLives.SetActive(true);
            GO_Cross.SetActive(false);

            // Reset the number of lives
            nbLives.text = GameManager.Instance.ParamData.PARAM_Player_Lives.ToString();
        }
        else
        {
            GO_Heart.SetActive(false);
            GO_NbLives.SetActive(false);
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
            }
            else
            {
                nbLives.text = "";
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
        GO_Heart.SetActive(false);
        GO_NbLives.SetActive(false);
        GO_Cross.SetActive(true);
    }
}
