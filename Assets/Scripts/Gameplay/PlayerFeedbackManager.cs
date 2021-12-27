using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedbackManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer InvincibleVFX;
    [SerializeField] SpriteRenderer StunAccumulation;
    [SerializeField] ParticleSystem StunAccumulationParticles;
    [SerializeField] SpriteRenderer AvatarFace;
    [SerializeField] GameObject Plasters;
    [SerializeField] PlayerVoiceController VoiceController;
    public Player LastPlayerHit;

    private float StunAlpha;

    private void Start()
    {
        StunAlpha = StunAccumulation.color.a;
        StunAccumulation.color = new Color(StunAccumulation.color.r, StunAccumulation.color.g, StunAccumulation.color.b, 0.0f);
        StunAccumulationParticles.Stop();
    }

    private void Update()
    {
        if(LastPlayerHit != null && LastPlayerHit.PlayerGameState == Enums.PlayerGameState.Dead)
        {
            VoiceController.PlayPush();
            LastPlayerHit = null;
        }
    }

    public void StartInvincibleFeedback()
    {
        StartCoroutine(InvincibleFlashAnim());
    }

    public void StopInvincibleFeedback()
    {
        InvincibleVFX.color = new Color(1, 1, 1, 0);
        StopAllCoroutines();
    }

    public void StartStunFeedback()
    {
        AvatarFace.sprite = player.CharSkin.StunSprite;
        VoiceController.PlayHurt();
    }

    public void EndStunFeedback()
    {
        AvatarFace.sprite = player.CharSkin.SpriteFace;
    }

    public void UpdateStunFeedback(int stat)
    {
        StunAccumulation.color = new Color(StunAccumulation.color.r, StunAccumulation.color.g, StunAccumulation.color.b, ((float)stat / 5) * StunAlpha);
        if (stat >= 3)
        {
            Plasters.SetActive(true);
            StunAccumulationParticles.Play();
        }
        else { 
            Plasters.SetActive(false);
            StunAccumulationParticles.Stop();
        }
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
