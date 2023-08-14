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
        controller.LastPressedJumpTime -= Time.deltaTime;
        controller.LastPressedDashTime -= Time.deltaTime;
        controller.dashRechargeTime -= Time.deltaTime;
        
        #region Input

        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (_moveInput.x > 0)
        {
            playerTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (_moveInput.x < 0)
        {
            playerTransform.localScale = new Vector3(-1, 1, 1);
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && 
            controller.dashRechargeTime < 0)
        {
            OnDashInput();
        }
        
        #endregion
        
        
        if (controller.LastPressedJumpTime > 0)
        {
            FSM.SetState<JumpState>();
        }

        if (controller.LastPressedDashTime > 0)
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