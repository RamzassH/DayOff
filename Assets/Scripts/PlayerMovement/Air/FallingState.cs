using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingState : AirState
{

    private Vector2 _moveInput;
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
        
        Debug.Log(IsDoubleJumped);
        
        #region TIMERS

        controller.coyoteTime -= Time.deltaTime;

        #endregion

        #region INPUT

        if (Input.GetAxisRaw("Jump") > 0)
        {
            OnJumpInput();
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FSM.SetState<DashState>();
        }

        if (_moveInput.x != 0)
        {
            RB.AddForce(new Vector2(_moveInput.x * 30, RB.velocity.y));
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

        base.FixedUpdate();
    }
}