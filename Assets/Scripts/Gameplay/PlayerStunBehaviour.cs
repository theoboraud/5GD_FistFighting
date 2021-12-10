using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerStunBehaviour : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] PhysicsMaterial2D bounce;
    [SerializeField] ParticleSystemController particleSystemController;
    [SerializeField] float timeToReduceStunAccumulation = 2;
    [SerializeField] int StunAccumulation;

    private float timer;

    private void Update()
    {
        if (player.PlayerPhysicState == PlayerPhysicState.IsHit)
        {
            //Check if material is applied to know whether it's the beginning of the Stun State
            if (!boxCollider.sharedMaterial)
            {
                StartStunState();
            }
            player.StunTimer += Time.deltaTime;
            //Debug.Log(player.StunTimer);
            float addedTimeBasedOnStunAccumulation = StunAccumulation * (0.2f * player.StunRecoveryTime);
            //Check if timer has gone above the required stun time
            if (player.StunTimer >= player.StunRecoveryTime + addedTimeBasedOnStunAccumulation)
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
                player.playerFeedbackManager.UpdateStunFeedback(StunAccumulation);
            }
        }
    }

    //Initialisation of StunState
    private void StartStunState()
    {
        StunAccumulation++;
        StunAccumulation = Mathf.Clamp(StunAccumulation, 0, 5);
        player.playerFeedbackManager.UpdateStunFeedback(StunAccumulation);
        Debug.Log(StunAccumulation);
        timer = 0;
        player.playerFeedbackManager.StartStunFeedback();
        boxCollider.sharedMaterial = bounce;
        particleSystemController.StartSystem();
    }

    //The function that stops the stun state
    private void StopStunState()
    {
        player.playerFeedbackManager.EndStunFeedback();
        player.PlayerPhysicState = PlayerPhysicState.InAir;
        boxCollider.sharedMaterial = null;
        particleSystemController.StopSystem();
    }
}
