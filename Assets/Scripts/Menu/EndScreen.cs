using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] SpriteRenderer face;

    private void Awake()
    {
        if (GameManager.Instance != null && PlayersManager.Instance != null)
        {
            SetFaceForWinner(PlayersManager.Instance.Players[GameManager.Instance.IndexWinner]);
        }
    }

    public void SetFaceForWinner(Player _winner)
    {
        face.sprite = _winner.CharSkin.SpriteFace;
    }
}
