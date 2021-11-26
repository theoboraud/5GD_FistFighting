using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject scoreText;                                          // Reference to the player score text game object (J1, J2...)
    [SerializeField] private List<GameObject> scoreTokenFillers = new List<GameObject>();   // References to all token fillers game objects

    [Header("Variables")]
    private int playerScoreValue;      // Contains the current score value


    /// <summary>
    ///     Set the color of the player score text and score token fillers
    /// </summary>
    public void SetColor(Color _color)
    {
        // Set the color of the player score text
        scoreText.GetComponent<Text>().color = _color;

        // Set the color for each token filler
        for (int i = 0; i < scoreTokenFillers.Count; i++)
        {
            scoreTokenFillers[i].GetComponent<Image>().color = _color;
        }
    }

    
    /// <summary>
    ///     Reset the score and the score tokens to 0
    /// </summary>
    public void ResetScore()
    {
        playerScoreValue = 0;
        UpdateScoreTokens();
    }


    /// <summary>
    ///     Set the score value to _newValue, and adjust the score tokens accordingly
    /// </summary>
    public void SetScore(int _newValue)
    {
        playerScoreValue = Mathf.Clamp(_newValue, 0, scoreTokenFillers.Count);
        UpdateScoreTokens();
    }


    /// <summary>
    ///     Update the score tokens fillers depending on the current score value
    /// </summary>
    private void UpdateScoreTokens()
    {
        for (int i = 0; i < playerScoreValue; i++)
        {
            scoreTokenFillers[i].SetActive(true);
        }

        for (int i = playerScoreValue; i < scoreTokenFillers.Count; i++)
        {
            scoreTokenFillers[i].SetActive(false);
        }
    }
}
