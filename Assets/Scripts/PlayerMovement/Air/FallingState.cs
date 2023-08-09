using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingState : AirState
{
    public FallingState(ChController controller) :
        base(controller)
    {
    }


    public override void Enter()
    {
        base.Enter();
        RB.gravityScale = Data.jumpFallingGravityScale;
    }

    public override void Exit()
    {
        base.Exit();
        RB.gravityScale = Data.gravityScale;
    }

    public override void Update()
    {
        #region TIMERS

        controller.coyoteTime -= Time.deltaTime;
        controller.LastPressedDashTime -= Time.deltaTime;
        controller.dashRechargeTime -= Time.deltaTime;

        #endregion

        #region INPUT

        if (Input.GetAxisRaw("Jump") > 0)
        {
            OnJumpInput();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDashInput();
        }
        
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        #endregion

        if (controller.coyoteTime > 0 &&
            controller.LastPressedJumpTime > 0)
        {
            controller.coyoteTime = 0;
            FSM.SetState<JumpState>();
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsDoubleJumped == false) 
        {
            FSM.SetState<DoubleJump>();
        }

        if (controller.LastPressedDashTime > 0)
        {
            FSM.SetState<DashState>();
        }

        if (IsGrounded())
        {
            IsDoubleJumped = false;
            FSM.SetState<IDLE>();
        }

        if (IsTouchWall())
        {
            IsDoubleJumped = false;
            FSM.SetState<TouchWall>();
        }
        
        base.Update();
    }

    public override void FixedUpdate()
    {
        if (RB.velocity.y < -Data.maxFallSpeed)
        {
            RB.velocity = new Vector2(RB.velocity.x, -Data.maxFallSpeed);
        }
        
        if (_moveInput.x != 0)
        {
            float force = _moveInput.x * Data.jumpHorizontalSpeed;
            RB.AddForce(new Vector2(force, RB.velocity.y));
        }

        if (Math.Abs(RB.velocity.x) > Data.maxVelocityValueX)
        {
            RB.velocity = new Vector2(Data.maxVelocityValueX * Math.Sign(RB.velocity.x), RB.velocity.y);
        }

        base.FixedUpdate();
    }
}