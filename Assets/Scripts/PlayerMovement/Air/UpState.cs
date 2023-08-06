using Unity.VisualScripting;
using System;
using UnityEngine;

public class UpState : AirState
{
    public UpState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        #region TIMERS

        controller.coyoteTime -= Time.deltaTime;
        controller.LastPressedDashTime -= Time.deltaTime;
        controller.dashRechargeTime -= Time.deltaTime;

        #endregion

        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDashInput();
        }

        if (IsTouchWall())
        {
            FSM.SetState<TouchWall>();
        }

        if (IsFalling())
        {
            FSM.SetState<FallingState>();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<DoubleJump>();
        }
        if (controller.LastPressedDashTime > 0)
        {
            FSM.SetState<DashState>();
        }
        if (_moveInput.y < 0)
        {
            FSM.SetState<JumpCutState>();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (RB.velocity.y < Data.jumpVelocityFallOff)
        {
            RB.velocity += Vector2.up * Physics.gravity.y * Data.fallGravityMultiplier * Time.deltaTime;
        }
        if (RB.velocity.y < -Data.maxFallSpeed)
        {
            RB.velocity = new Vector2(RB.velocity.x, -Data.maxFallSpeed);
        }
        
        if (_moveInput.x != 0)
        {
            float force = _moveInput.x * Data.jumpHorizontalSpeed;
            RB.AddForce(new Vector2(force, RB.velocity.y), ForceMode2D.Force);
        }

        if (Math.Abs(RB.velocity.x) > Data.maxVelocityValueX)
        {
            RB.velocity = new Vector2(Data.maxVelocityValueX * Math.Sign(RB.velocity.x), RB.velocity.y);
        }
        
    }
}