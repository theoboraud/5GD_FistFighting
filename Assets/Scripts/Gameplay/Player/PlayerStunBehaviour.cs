using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerStunBehaviour : MonoBehaviour
{
    private PlayerFeedbackManager _playerFeedbackManager;
    private PlayerController _playerController;
    private PlayerStates _playerStates;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] PhysicsMaterial2D bounce;
    [SerializeField] ParticleSystemController particleSystemController;
    [SerializeField] float timeToReduceStunAccumulation = 2;
    [SerializeField] int StunAccumulation;

    private float timer;

    private void Awake()
    {
	    _playerFeedbackManager = GetComponent<PlayerFeedbackManager>();
	    _playerController = GetComponent<PlayerController>();
		_playerStates = GetComponent<PlayerStates>();
		
    }

    private void Update()
    {
        if (_playerStates.PlayerPhysicState == PlayerPhysicState.IsHit)
        {
            //Check if material is applied to know whether it's the beginning of the Stun State
            if (!boxCollider.sharedMaterial)
            {
                StartStunState();
            }
            _playerController.StunTimer += Time.deltaTime;
            //Debug.Log(player.StunTimer);
            float addedTimeBasedOnStunAccumulation = StunAccumulation * (0.2f * _playerController.StunRecoveryTime);
            //Check if timer has gone above the required stun time
            if (_playerController.StunTimer >= GlobalSettings.StunRecoveryTime + addedTimeBasedOnStunAccumulation)
            {
                StopStunState();
            }
        }
        else
        {
            timer += Time.deltaTime;
            if(timer > timeToReduceStunAccumulation)
            {
                timer = 0f;
                StunAccumulation--;
                StunAccumulation = Mathf.Clamp(StunAccumulation, 0, 5);
                _playerFeedbackManager.UpdateStunFeedback(StunAccumulation);
            }
        }
    }

    //Initialisation of StunState
    private void StartStunState()
    {
        StunAccumulation++;
        StunAccumulation = Mathf.Clamp(StunAccumulation, 0, 5);
        _playerFeedbackManager.UpdateStunFeedback(StunAccumulation);
        Debug.Log(StunAccumulation);
        timer = 0;
        _playerFeedbackManager.StartStunFeedback();
        boxCollider.sharedMaterial = bounce;
        particleSystemController.StartSystem();
    }

    //The function that stops the stun state
    private void StopStunState()
    {
	    _playerFeedbackManager.EndStunFeedback();
	    _playerController.PlayerPhysicState = PlayerPhysicState.InAir;
        boxCollider.sharedMaterial = null;
        particleSystemController.StopSystem();
    }
}
