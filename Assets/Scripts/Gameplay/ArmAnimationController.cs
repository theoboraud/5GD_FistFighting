using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnimationController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] BoxCollider2D collider;
    [SerializeField] Animator animator;

    public void PlayAnimation()
    {
        animator.SetBool("Extend", true);
        Invoke("StopAnimation", 0.25f);
        Invoke("ActivateCollider", 0.1f);
    }

    public void PlayHoldAnimation()
    {
        animator.SetBool("Hold", true);
    }

    public void PlayHoldMaxAnimation()
    {
        animator.SetBool("HoldMax", true);
    }

    public void StopAnimation()
    {
        animator.SetBool("Extend", false);
        animator.SetBool("Hold", false);
        animator.SetBool("HoldMax", false);
    }

    public void ActivateCollider()
    {
        collider.enabled = true;
        Invoke("DeactivateCollider", GameManager.Instance.ParamData.PARAM_Player_ArmCooldown * 0.125f);
    }

    public void DeactivateCollider()
    {
        collider.enabled = false;
    }
}
