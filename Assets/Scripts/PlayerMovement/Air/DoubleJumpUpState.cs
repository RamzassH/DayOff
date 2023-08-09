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
    }

    public override void Exit()
    {
        base.Exit();
        IsDoubleJumped = true;
    }

    public override void Update()
    {
        base.Update();
        controller.LastPressedDashTime -= Time.deltaTime;
        controller.dashRechargeTime -= Time.deltaTime;

        #region INPUT

        _moveInput.x = Input.GetAxisRaw("Horizontal");

        #endregion


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDashInput();
        }

        if (controller.LastPressedDashTime > 0)
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