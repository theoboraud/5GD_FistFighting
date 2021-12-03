using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedbackManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer InvincibleVFX;
    [SerializeField] SpriteRenderer AvatarFace;

    public void StartInvincibleFeedback()
    {
        StartCoroutine(InvincibleFlashAnim());
    }

    public void StopInvincibleFeedback()
    {
        StopAllCoroutines();
    }

    public void StartStunFeedback()
    {
        AvatarFace.sprite = player.CharSkin.StunSprite;
    }

    public void EndStunFeedback()
    {
        AvatarFace.sprite = player.CharSkin.SpriteFace;
    }

    IEnumerator InvincibleFlashAnim()
    {
        Color col = InvincibleVFX.color;
        int i = -1;
        while (true)
        {
            col.a += i * Time.deltaTime;
            InvincibleVFX.color = col;
            if (col.a <= 0) i = 1;
            else if (col.a >= 1) i = -1;
            yield return null;
        }
    }
}
