using UnityEngine;

public class RunState : GroundedState
{
    #region INPUT PARAMETERS

    #endregion

    public RunState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // _moveInput.x = Input.GetAxisRaw("Horizontal");
        // Run(1, _moveInput);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (controller.lastPressedJumpTime > 0)
        {
            FSM.SetState<JumpState>();
        }

        if (controller.lastPressedDashTime > 0)
        {
            FSM.SetState<DashState>();
        }
        
        if (IsTouchWall())
        {
            FSM.SetState<TouchWall>();
        }

        base.Update();

        if (RB.velocity.x == 0 && RB.velocity.y == 0 && _moveInput.x == 0)
        {
            FSM.SetState<IDLE>();
        }

        if (IsFalling())
        {
            RechargeCoyoteTime();
            FSM.SetState<FallingState>();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_moveInput.x != 0)
        {
            Run(1, _moveInput);
        }
    }
}