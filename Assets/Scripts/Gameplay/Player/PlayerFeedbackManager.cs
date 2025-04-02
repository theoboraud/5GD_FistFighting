using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Enums;
using UnityEngine;

public class PlayerFeedbackManager : MonoBehaviour
{
    private Player _player;
    
    [SerializeField] SpriteRenderer InvincibleVFX;
    [SerializeField] SpriteRenderer StunAccumulation;
    [SerializeField] ParticleSystem StunAccumulationParticles;
    [SerializeField] SpriteRenderer AvatarFace;
    [SerializeField] GameObject Plasters;
    [SerializeField] PlayerVoiceController VoiceController;
    private FeedbackFaceController _feedbackFaceController;
    public Player LastPlayerHit;

    [Header ("PlayerVFX")]
    [SerializeField] GameObject hitEnvVFX;
    [SerializeField] GameObject airDashVFX;
    [SerializeField] GameObject playerKilledVFX;
    [SerializeField] GameObject[] hitPlayerVFX;
    [SerializeField] GameObject chargedHitVFX;

    private float StunAlpha;

    private void OnDestroy()
    {
	    RemoveCallBacks();
    }

    private void InitCallBacks()
    {
        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerCollisionEnter, CollisionFeedback);
        _player.EventCenter.Subscribe<PlayerGameState>(PlayerEvent.OnGameStateChange, OnPlayerGameStateChange);
        _player.EventCenter.Subscribe<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, OnPlayerPhysicStateChange);

        GEventCenter.Subscribe<Player>(GameEvent.OnPlayerDead, OnOtherPlayerDead);
    }

    private void RemoveCallBacks()
    {
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerCollisionEnter, CollisionFeedback);
        _player.EventCenter.Unsubscribe<PlayerGameState>(PlayerEvent.OnGameStateChange, OnPlayerGameStateChange);
        _player.EventCenter.Unsubscribe<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, OnPlayerPhysicStateChange);

        GEventCenter.Unsubscribe<Player>(GameEvent.OnPlayerDead, OnOtherPlayerDead);
    }

    public void Init()
    {
        _player = GetComponent<Player>();
        _feedbackFaceController = GetComponentInChildren<FeedbackFaceController>();

        InitCallBacks();

        StunAlpha = StunAccumulation.color.a;
        StunAccumulation.color = new Color(StunAccumulation.color.r, StunAccumulation.color.g, StunAccumulation.color.b, 0.0f);
        StunAccumulationParticles.Stop();
    }

    private void OnOtherPlayerDead(Player _playerDead)
    {
        if(LastPlayerHit ==_playerDead) 
        {
            VoiceController.PlayPush();
            LastPlayerHit = null;
        }
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
        SpawnPlayerKilledVFX(this.transform.position);
    }

    private void CollisionFeedback()
    {
	    if(_player.PlayerStates.PlayerPhysicState == PlayerPhysicState.OnGround && _feedbackFaceController.CanShake)
	    {
		    _feedbackFaceController.ShakeFace();
		    _feedbackFaceController.CanShake = false;
	    }
    }

    private void IsInAir()
    {
	    _feedbackFaceController.CanShake = true;
    }

    //VFX Feedbacks

    /// <summary>
    /// Spawn feed back on arm hit Environment object
    /// </summary>
    /// <param name="position">VFX Spawn position</param>
    /// <param name="rotation">VFX Spawn rotation</param>
    public void SpawnEnvHitVFX(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(hitEnvVFX, position, rotation, this.transform);
    }


    /// <summary>
    /// Spawn feed back on arm hit player/RB objects
    /// </summary>
    /// <param name="strength">HitStrength</param>
    /// <param name="position">VFX Spawn position</param>
    /// <param name="rotation">VFX Spawn rotation</param>
    public void SpawnPlayerHitVFX(int strength, Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(hitPlayerVFX[strength], position, rotation, this.transform);
    }

    /// <summary>
    /// Spawn VFX of hit after on arm charge max
    /// </summary>
    /// <param name="position">VFX Spawn position</param>
    /// <param name="rotation">VFX Spawn rotation</param>
    public void SpawnChargedHit(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(chargedHitVFX, position, rotation, this.transform);
    }

    /// <summary>
    /// Spawn feed back on arm hit noting(air impulse)
    /// </summary>
    /// <param name="position">VFX Spawn position</param>
    /// <param name="rotation">VFX Spawn rotation</param>
    public void SpawnAirDashVFX(Vector3 position, Quaternion rotation)
    {
        GameObject _go = Instantiate(airDashVFX, position, rotation, this.transform);
    }


    /// <summary>
    /// Spawn expulsion effect on player dead
    /// </summary>
    public void SpawnPlayerKilledVFX(Vector3 position)
    {
        Vector3 centerPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0.0f);
        Quaternion rotation = Quaternion.AngleAxis(Vector3.Angle(playerKilledVFX.transform.up - position, (centerPos - position).normalized), Vector3.forward);
        GameObject _go = Instantiate(playerKilledVFX, position, rotation * playerKilledVFX.transform.rotation, this.transform);
        _go.transform.rotation = Quaternion.LookRotation(centerPos - position);
    }
}
