using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DashBehaviour : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 dashDirection;

    private bool isDashing = false;
    private bool canDash = true;

    private float dashSpeed = 10f;
    private float endOfDashSpeed = 5f;

    private float dashTime = 0.1f;
    public float dashCooldown = 1f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;
        }
    }

    public void DashRight()
    {
        if(!isDashing && canDash)
        {
            dashDirection = Vector2.right;
            StartCoroutine("Dash");
        }
    }

    public void DashLeft()
    {
        if(!isDashing && canDash)
        {
            dashDirection = Vector2.left;
            StartCoroutine("Dash");
        }
    }

    private IEnumerator Dash()
    {
        print("Dashing!");
        isDashing = true;
        float _gravity = rb.gravityScale;
        rb.gravityScale = 0.1f;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        rb.gravityScale = _gravity;
        rb.velocity = dashDirection * endOfDashSpeed;
        dashDirection = Vector2.zero;
        StartCoroutine("DashCooldown");
    }

    private IEnumerator DashCooldown()
    {
        print("DashCooldown started");
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        print("DashCooldown over");
    }
}
