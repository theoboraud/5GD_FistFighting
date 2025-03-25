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
    private FeedbackFaceController _feedbackFaceController;
    public Player LastPlayerHit;

    private float StunAlpha;

    private void OnEnable()
    {
	    _player = GetComponent<Player>();
	    _feedbackFaceController = GetComponentInChildren<FeedbackFaceController>();
	    
	    _playerController = GetComponent<PlayerController>();
	    _playerStates = GetComponent<PlayerStates>();

	    
    }

    private void OnDisable()
    {
	    RemoveCallBacks();
    }

    private void InitCallBacks()
    {
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerCollisionEnter, CollisionFeedback);
        _player.EventCenter.Subscribe<PlayerGameState>(PlayerEvent.OnGameStateChange, OnPlayerGameStateChange);
        _player.EventCenter.Subscribe<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, OnPlayerPhysicStateChange);
    }

    private void RemoveCallBacks()
    {
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerCollisionEnter, CollisionFeedback);

        _player.EventCenter.Unsubscribe<PlayerGameState>(PlayerEvent.OnGameStateChange, OnPlayerGameStateChange);
        _player.EventCenter.Unsubscribe<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, OnPlayerPhysicStateChange);
    }

    private void Start()
    {
	    InitCallBacks();
        StunAlpha = StunAccumulation.color.a;
        StunAccumulation.color = new Color(StunAccumulation.color.r, StunAccumulation.color.g, StunAccumulation.color.b, 0.0f);
        StunAccumulationParticles.Stop();
    }

    private void Update()
    {
        //TODO
        //!!Here we need to check or call by an event, in place of follow in update
        if(LastPlayerHit != null && LastPlayerHit.IsDead()) 
        {
            VoiceController.PlayPush();
            LastPlayerHit = null;
        }
        //!!NeedChange
    }
    private void OnPlayerGameStateChange(PlayerGameState _gameState)
    {
        if (_gameState == PlayerGameState.Invincible)
        {
            StartInvincibleFeedback();
        }
        else
        {
            StopInvincibleFeedback();
        }

        if (_gameState == PlayerGameState.Dead)
        {
            KilledFeedback();
        }
    }

    private void OnPlayerPhysicStateChange(PlayerPhysicState _physicState)
    {
        if (_physicState == PlayerPhysicState.InAir)
        {
            IsInAir();
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
        AvatarFace.sprite = _player.GetSkin().StunSprite;
        VoiceController.PlayHurt();
    }

    public void EndStunFeedback()
    {
        AvatarFace.sprite = _player.GetSkin().SpriteFace;
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
	    if(_playerStates.PlayerPhysicState == PlayerPhysicState.OnGround && _feedbackFaceController.CanShake)
	    {
		    _feedbackFaceController.ShakeFace();
		    _feedbackFaceController.CanShake = false;
	    }
    }

    private void IsInAir()
    {
	    _feedbackFaceController.CanShake = true;
    }
}
