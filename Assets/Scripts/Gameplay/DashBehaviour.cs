using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DashBehaviour : MonoBehaviour
{

    // #region ==================== CLASS VARIABLES ====================

    private const float CONSTANT_dashSpeed = 10f;          // Player speed during the dash
    private const float CONSTANT_dashTime = 0.1f;          // How much time the dash takes

    private const float CONSTANT_endOfDashSpeed = 5f;      // Player speed set at the end of a dash
    private const float CONSTANT_dashCooldown = 1f;        // How much time the dash is not available for after using it

    private Rigidbody2D rb;                 // Rigidbody reference

    private Vector2 dashDirection;          // Vector containing the dash direction (right or left)
    private bool isDashing = false;         // Boolean telling whether or not the player is dashing right now
    private bool canDash = true;            // Boolean telling whether or not the player can dash

    // #endregion



    // #region ==================== UNITY FUNCTIONS ====================

    /// <summary>
    ///     Get initial references
    /// </summary>
    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }


    /// <summary>
    ///     Each frame, if dashing, the velocity is set using dash speed (not affected by physics)
    /// </summary>
    private void FixedUpdate()
    {
        if(isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;
        }
    }

    // #endregion



    // #region ===================== DASH FUNCTIONS ====================

    /// <summary>
    ///     When dashing right, set the dashDirection to right and start the Dash coroutine
    /// </summary>
    public void DashRight()
    {
        if(!isDashing && canDash)
        {
            dashDirection = Vector2.right;
            StartCoroutine("Dash");
        }
    }


    /// <summary>
    ///     When dashing left, set the dashDirection to left and start the Dash coroutine
    /// </summary>
    public void DashLeft()
    {
        if(!isDashing && canDash)
        {
            dashDirection = Vector2.left;
            StartCoroutine("Dash");
        }
    }


    /// <summary>
    ///     When dashing, change the gravity during the dash, wait for dashTime, and set back gravity and velocity at the end of the dash
    /// </summary>
    private IEnumerator Dash()
    {
        isDashing = true;
        float _gravity = rb.gravityScale;
        rb.gravityScale = 0.1f;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        rb.gravityScale = _gravity;
        rb.velocity = dashDirection * endOfDashSpeed;
        dashDirection = Vector2.zero;

        // Call the cooldown coroutine
        StartCoroutine("DashCooldown");
    }


    /// <summary>
    ///     The player cannot dash while the dash cooldown is running
    /// </summary>
    private IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // #endregion
}
