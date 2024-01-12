using Unity.VisualScripting;
using System;
using UnityEngine;

public class UpState : AirState
{
    private float _jumpCutBlockTime;

    public UpState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _jumpCutBlockTime = Data.jumpCutBlockTimeBuffer;
        controller.lastPressedJumpTime = -1;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        #region TIMER
        
        _jumpCutBlockTime -= Time.deltaTime;

        #endregion
        
        if (IsFalling() && !IsDashing)
        {
            FSM.SetState<FallingState>();
        }
        
        if (controller.lastPressedJumpTime > 0)
        {
            FSM.SetState<DoubleJump>();
        }
        
        if (_moveInput.y < 0 && _jumpCutBlockTime <= 0)
        {
            FSM.SetState<JumpCutState>();
        }
    }

    public override void FixedUpdate()
    {
        if (RB.velocity.y < Data.jumpVelocityFallOff)
        {
            RB.velocity += Vector2.up * Physics.gravity.y * Data.fallGravityMultiplier * Time.deltaTime;
        }
        if (_moveInput.x != 0)
        {
            float force = _moveInput.x * Data.jumpHorizontalSpeed;
            RB.AddForce(new Vector2(force, RB.velocity.y), ForceMode2D.Force);
        }

        base.FixedUpdate();
    }
}