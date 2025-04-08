using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerStunBehaviour : MonoBehaviour
{
    private PlayerFeedbackManager _playerFeedbackManager;
    private Player _player;
    private PlayerController _playerController;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] PhysicsMaterial2D bounce;
    [SerializeField] ParticleSystemController particleSystemController;
    [SerializeField] float timeToReduceStunAccumulation = 2;
    [SerializeField] int StunAccumulation;

    private float stunTimer;
    private float timer;
    private float stunRecoveryTime;

    public void Init()
    {
        _player = GetComponent<Player>();
	    _playerFeedbackManager = GetComponent<PlayerFeedbackManager>();
	    _playerController = GetComponent<PlayerController>();

        stunRecoveryTime = GlobalSettings.StunRecoveryTime;

        SubscribeEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void SubscribeEvents()
    {
        _player.EventCenter.Subscribe<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, OnPlayerPhysicStateChange);
    }
    private void UnsubscribeEvents()
    {
        _player.EventCenter.Unsubscribe<PlayerPhysicState>(PlayerEvent.OnPhysicStateChange, OnPlayerPhysicStateChange);
    }

    private void Update()
    {
        if (_player.IsHit())
        {
            stunTimer += Time.deltaTime;
            //Debug.Log(_playerController.StunTimer);
            float addedTimeBasedOnStunAccumulation = StunAccumulation * (0.2f * stunRecoveryTime);
            //Check if timer has gone above the required stun time
            if (stunTimer >= stunRecoveryTime + addedTimeBasedOnStunAccumulation)
            {
                StopStunState();
            }
        }
        else
        {
            if (StunAccumulation > 0)
            {
                timer += Time.deltaTime;
                if (timer > timeToReduceStunAccumulation)
                {
                    timer = 0f;
                    StunAccumulation = Mathf.Clamp(StunAccumulation - 1, 0, 5);
                    _playerFeedbackManager.UpdateStunFeedback(StunAccumulation);
                }
            }
        }
    }
    private void OnPlayerPhysicStateChange(PlayerPhysicState _physicState)
    {
        if (_physicState == PlayerPhysicState.IsHit)
        {
            StartStunState();
        }
    }

    //Initialisation of StunState
    private void StartStunState()
    {
        stunTimer = 0;
        StunAccumulation++;
        StunAccumulation = Mathf.Clamp(StunAccumulation, 0, 5);
        _playerFeedbackManager.UpdateStunFeedback(StunAccumulation);
        timer = 0;
        boxCollider.sharedMaterial = bounce;
        //particleSystemController.StartSystem();
    }

    //The function that stops the stun state
    private void StopStunState()
    {
        Debug.Log("StopStun");
        _player.PlayerStates.PlayerPhysicState = PlayerPhysicState.InAir;
        boxCollider.sharedMaterial = null;
        //particleSystemController.StopSystem();
    }
}
