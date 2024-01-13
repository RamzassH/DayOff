using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected StateMachine FSM;
    protected Rigidbody2D RB;
    protected MovementData Data;

    protected ChController controller;

    protected Vector2 groundCheckSize;
    protected Vector2 wallCheckSize;

    protected LayerMask groundLayer;

    protected Vector2 _moveInput;

    protected bool IsFacingRight;
    protected static bool IsDashing;

    public PlayerState(ChController controller)
    {
        FSM = controller.FSM;
        RB = controller.RB;
        Data = controller.data;
        this.controller = controller;

        groundCheckSize = new Vector2(0.5f, 0.03f);
        wallCheckSize = new Vector2(0.03f, 1f);

        groundLayer = 8;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Awake()
    {

    }

    public virtual void LateUpdate()
    {

    }

    #region GOVNO_COD
    protected bool IsFalling()
    {
        return RB.velocity.y <= 0 && !Physics2D.OverlapBox(controller.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    protected bool IsCanClimb()
    {
        float distance = 1f;
        return !Physics2D.Raycast(controller.headRayCastPos.position, controller.playerBody.localScale,
            distance, groundLayer);
    }

    protected bool IsGrounded()
    {
        return Physics2D.OverlapBox(controller.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    protected bool IsInAir()
    {
        return !Physics2D.OverlapBox(controller.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    protected bool IsTouchWall()
    {
        return Physics2D.OverlapBox(controller.rightWallCheck.position, wallCheckSize,
                   0, groundLayer) ||
               Physics2D.OverlapBox(controller.leftWallCheck.position, wallCheckSize,
                   0, groundLayer);
    }

    public void OnJumpInput()
    {
        controller.lastPressedJumpTime = controller.data.jumpInputBufferTime;
    }

    public void OnDashInput()
    {
        if (controller.dashRechargeTime <= 0)
        {
            controller.lastPressedDashTime = controller.data.dashInputBufferTime;
        }
    }

    public void RechargeCoyoteTime()
    {
        controller.coyoteTime = controller.data.coyoteTime;
    }
    #endregion
}
