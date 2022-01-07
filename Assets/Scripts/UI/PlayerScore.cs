using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerScore : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerFace;                                         // Reference to the player face GameObject
    [SerializeField] private GameObject playerOutline;
    [SerializeField] private List<GameObject> scorePos = new List<GameObject>();            // References to all score pos game objects
    public Player Player;

    [Header("Variables")]
    private int newScore = 0;
    private int playerScoreValue = 0;      // Contains the current score value


    /// <summary>
    ///     Set the color of the player score text and score token fillers
    /// </summary>
    public void SetColor(Color _color)
    {
        playerOutline.GetComponent<Image>().color = _color;
    }


    /// <summary>
    ///     Set the skin of the player
    /// </summary>
    public void SetFace()
    {
        playerFace.GetComponent<Image>().sprite = Player.CharSkin.SpriteFace;
        playerOutline.GetComponent<Image>().sprite = Player.CharSkin.SpriteFace;
    }


    /// <summary>
    ///     Reset the score and the score tokens to 0
    /// </summary>
    public void ResetScore()
    {
        SetFace();
        newScore = 0;
        playerScoreValue = 0;
        UpdateScorePos();
    }


    /// <summary>
    ///     Set the score value to _newValue, and adjust the score tokens accordingly
    /// </summary>
    public void SetScore(int _newValue)
    {
        SetFace();
        newScore = Mathf.Clamp(_newValue, 0, 5);
        UpdateScorePos();
    }


    /// <summary>
    ///     Update the pos of the face depending on the score value
    /// </summary>
    private void UpdateScorePos()
    {
        if (newScore != playerScoreValue)
        {
            playerScoreValue = newScore;
            playerFace.transform.DOMoveX(scorePos[playerScoreValue - 1].transform.position.x, 1f, false);
            playerOutline.transform.DOMoveX(scorePos[playerScoreValue - 1].transform.position.x, 1f, false);
            Invoke("Shake", 1f);
        }
    }


    private void Shake()
    {
        playerFace.transform.DOShakePosition(0.3f, 1f, 1, 90f, false, true);
        playerOutline.transform.DOShakePosition(0.3f, 1f, 1, 90f, false, true);
    }
}
