using UnityEngine;

public class IDLE : GroundedState
{
    public IDLE(ChController controller) :
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
        
        controller.coyoteTime = controller.data.coyoteTime;

        controller.LastPressedJumpTime -= Time.deltaTime;
        controller.LastPressedDashTime -= Time.deltaTime;
        
        controller.dashRechargeTime -= Time.deltaTime;
        
        if (controller.dashRechargeTime < -100f)
        {
            controller.dashRechargeTime = 0;
        }
        #endregion
        
        #region INPUT

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetAxisRaw("Jump") > 0)
        {
            OnJumpInput();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && 
            controller.dashRechargeTime < 0)
        {
            OnDashInput();
        }

        #endregion

        if (IsTouchWall())
        {
            FSM.SetState<TouchWall>();
        }

        if (_moveInput.x != 0)
        {
            FSM.SetState<RunState>();
        }

        if (controller.LastPressedJumpTime > 0)
        {
            FSM.SetState<JumpState>();
        }

        if (controller.LastPressedDashTime > 0 && 
            controller.dashRechargeTime < 0)
        {
            FSM.SetState<DashState>();
        }

        if (IsFalling())
        {
            RechargeCoyoteTime();
            FSM.SetState<FallingState>();
        }
    }
}