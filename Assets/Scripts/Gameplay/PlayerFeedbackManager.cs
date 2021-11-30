using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedbackManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer InvincibleVFX;

    public void StartInvincibleFeedback()
    {
        StartCoroutine(InvincibleFlashAnim());
    }

    public void StopInvincibleFeedback()
    {
        StopAllCoroutines();
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
