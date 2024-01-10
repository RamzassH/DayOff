using UnityEngine;
using System;

public class DoubleJumpUpState : AirState
{
    public DoubleJumpUpState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (IsFalling())
        {
            FSM.SetState<FallingState>();
            return;
        }
        if (IsGrounded()) { 
            FSM.SetState<IDLE>();
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        IsDoubleJumped = true;
    }

    public override void Update()
    {
        base.Update();
        
        if (controller.lastPressedDashTime > 0)
        {
            FSM.SetState<DashState>();
        }

        if (IsTouchWall())
        {
            FSM.SetState<TouchWall>();
        }

        if (IsFalling())
        {
            FSM.SetState<FallingState>();
        }
    }
    
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        #region VELOCITY CONTROL

        if (RB.velocity.y < Data.jumpVelocityFallOff)
        {
            RB.velocity += Vector2.up * Physics.gravity.y * Data.fallGravityMultiplier * Time.deltaTime;
        }

        if (_moveInput.x != 0)
        {
            float force = _moveInput.x * Data.jumpHorizontalSpeed;
            RB.AddForce(new Vector2(force, RB.velocity.y), ForceMode2D.Force);
        }

        #endregion
    }
}