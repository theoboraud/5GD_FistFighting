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

    private float timer;

    private void OnEnable()
    {
        _player = GetComponent<Player>();
	    _playerFeedbackManager = GetComponent<PlayerFeedbackManager>();
	    _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_player.PlayerStates.PlayerPhysicState == PlayerPhysicState.IsHit)
        {
            //Check if material is applied to know whether it's the beginning of the Stun State
            if (!boxCollider.sharedMaterial)
            {
                StartStunState();
            }
            float stunRecoveryTime = GlobalSettings.StunRecoveryTime;
            _playerController.StunTimer += Time.deltaTime;
            //Debug.Log(_playerController.StunTimer);
            float addedTimeBasedOnStunAccumulation = StunAccumulation * (0.2f * stunRecoveryTime);
            //Check if timer has gone above the required stun time
            if (_playerController.StunTimer >= stunRecoveryTime + addedTimeBasedOnStunAccumulation)
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
        timer = 0;
        _playerFeedbackManager.StartStunFeedback();
        boxCollider.sharedMaterial = bounce;
        //particleSystemController.StartSystem();
    }

    //The function that stops the stun state
    private void StopStunState()
    {
        Debug.Log("StopStun");
        _playerFeedbackManager.EndStunFeedback();
        _player.PlayerStates.PlayerPhysicState = PlayerPhysicState.InAir;
        boxCollider.sharedMaterial = null;
        //particleSystemController.StopSystem();
    }
}
