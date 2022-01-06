using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnimationController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Animator animator;

    public void PlayAnimation()
    {
        animator.SetBool("Extend", true);
        Invoke("StopAnimation", 0.25f);
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
        transform.localScale.Set(2, 2, 1);
    }

}
