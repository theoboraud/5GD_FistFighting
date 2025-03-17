using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Enums;
using UnityEngine;

public class PlayerFeedbackManager : MonoBehaviour
{
    private Player _player;
    private PlayerController _playerController;
    private PlayerStates _playerStates;
    
    [SerializeField] SpriteRenderer InvincibleVFX;
    [SerializeField] SpriteRenderer StunAccumulation;
    [SerializeField] ParticleSystem StunAccumulationParticles;
    [SerializeField] SpriteRenderer AvatarFace;
    [SerializeField] GameObject Plasters;
    [SerializeField] PlayerVoiceController VoiceController;
    public FeedbackFaceController FaceController;
    public Player LastPlayerHit;

    private float StunAlpha;

    private void OnEnable()
    {
	    _player = GetComponent<Player>();
	    _playerController = GetComponent<PlayerController>();
	    _playerStates = GetComponent<PlayerStates>();

	    InitCallBacks();
    }

    private void OnDisable()
    {
	    RemoveCallBacks();
    }

    private void InitCallBacks()
    {
        EventCenter.Subscribe(GameEvent.OnPlayerKilled, KilledFeedback);
        EventCenter.Subscribe(GameEvent.OnPlayerCollisionEnter, CollisionFeedback);
        EventCenter.Subscribe(GameEvent.OnAir, IsInAir);

    }

    private void RemoveCallBacks()
    {
        EventCenter.Unsubscribe(GameEvent.OnPlayerKilled, KilledFeedback);
        EventCenter.Unsubscribe(GameEvent.OnPlayerCollisionEnter, CollisionFeedback);
        EventCenter.Unsubscribe(GameEvent.OnAir, IsInAir);
    }

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
        AvatarFace.sprite = _player.CharSkin.StunSprite;
        VoiceController.PlayHurt();
    }

    public void EndStunFeedback()
    {
        AvatarFace.sprite = _player.CharSkin.SpriteFace;
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

    private void KilledFeedback()
    {
	    GameManager.Instance.Feedback.ShakeCamera(0.5f, 0.7f);
	    GameManager.Instance.Feedback.SpawnExpulsionVFX(this.transform.position);
    }

    private void CollisionFeedback()
    {
	    if(_playerStates.playerPhysicState == PlayerPhysicState.OnGround && FaceController.CanShake)
	    {
		    FaceController.ShakeFace();
		    FaceController.CanShake = false;
	    }
    }

    private void IsInAir()
    {
	    FaceController.CanShake = true;
    }
}
