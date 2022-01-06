using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] SpriteRenderer Face;

    public void SetFaceForWinner(Player Winner)
    {
        Face.sprite = Winner.CharSkin.SpriteFace;
    }
}
