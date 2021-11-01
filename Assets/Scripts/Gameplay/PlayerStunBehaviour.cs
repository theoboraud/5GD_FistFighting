using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunBehaviour : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] PhysicsMaterial2D bounce;
    [SerializeField] ParticleSystemController particleSystemController;

    private void Update()
    {
        if(player.PlayerPhysicState == Enums.PlayerPhysicState.isHit)
        {
            //Check if material is applied to know whether it's the beginning of the Stun State
            if (!boxCollider.sharedMaterial)
            {
                StartStunState();
            }
            player.StunTimer += Time.deltaTime;
            Debug.Log(player.StunTimer);
            //Check if timer has gone above the required stun time
            if(player.StunTimer >= player.StunRecoveryTime)
            {
                StopStunState();
            }
        }
    }

    //Initialisation of StunState
    private void StartStunState()
    {
        boxCollider.sharedMaterial = bounce;
        particleSystemController.StartSystem();
        Debug.Log("We stunned baby!!!");
    }

    //The function that stops the stun state
    private void StopStunState()
    {
        player.PlayerPhysicState = Enums.PlayerPhysicState.InAir;
        boxCollider.sharedMaterial = null;
        particleSystemController.StopSystem();
        Debug.Log("We not stunned anymore!!");
    }
}
