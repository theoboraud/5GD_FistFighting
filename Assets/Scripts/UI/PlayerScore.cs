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

    [Header("Variables")]
    private int playerScoreValue;      // Contains the current score value


    /// <summary>
    ///     Set the color of the player score text and score token fillers
    /// </summary>
    public void SetColor(Color _color)
    {
        playerOutline.GetComponent<Image>().color = _color;
    }


    /// <summary>
    ///     Reset the score and the score tokens to 0
    /// </summary>
    public void ResetScore()
    {
        playerScoreValue = 0;
        UpdateScorePos();
    }


    /// <summary>
    ///     Set the score value to _newValue, and adjust the score tokens accordingly
    /// </summary>
    public void SetScore(int _newValue)
    {
        playerScoreValue = Mathf.Clamp(_newValue, 0, 5);
        UpdateScorePos();
    }


    /// <summary>
    ///     Update the pos of the face depending on the score value
    /// </summary>
    private void UpdateScorePos()
    {
        playerFace.transform.DOMoveX(scorePos[playerScoreValue - 1].transform.position.x, 1f, false);
        playerOutline.transform.DOMoveX(scorePos[playerScoreValue - 1].transform.position.x, 1f, false);
        Invoke("Shake", 1f);
    }


    private void Shake()
    {
        playerFace.transform.DOShakePosition(0.3f, 1f, 1, 90f, false, true);
        playerOutline.transform.DOShakePosition(0.3f, 1f, 1, 90f, false, true);
    }
}
