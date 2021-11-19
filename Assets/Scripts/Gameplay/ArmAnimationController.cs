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
        Invoke("StopAnimation", 0.3f);
        Invoke("ActivateCollider", 0.2f);
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
        Invoke("DeactivateCollider", 0.1f);
    }

    public void DeactivateCollider()
    {
        collider.enabled = false;
    }
}
