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
        base.Update();
        
        if (controller.coyoteTime > 0 &&
            controller.lastPressedJumpTime > 0)
        {
            controller.coyoteTime = 0;
            FSM.SetState<JumpState>();
        }

        if (controller.lastPressedJumpTime > 0 && IsDoubleJumped == false) 
        {
            FSM.SetState<DoubleJump>();
        }

        if (IsDashing && IsFalling())
        {
            FSM.SetState<FallingState>();
        }

        if (IsGrounded())
        {
            IsDoubleJumped = false;
            FSM.SetState<IDLE>();
        }
        
    }

    public override void FixedUpdate()
    {
        if (_moveInput.x != 0)
        {
            float force = _moveInput.x * Data.jumpHorizontalSpeed;
            RB.AddForce(new Vector2(force, RB.velocity.y));
        }
        base.FixedUpdate();
    }
}