using UnityEngine;

public class IDLE : GroundedState
{
    public IDLE(tmpMovement playerMovement) :
        base(playerMovement)
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
        
        playerMovement.coyoteTime = playerMovement.data.coyoteTime;

        playerMovement.LastPressedJumpTime -= Time.deltaTime;
        playerMovement.LastPressedDashTime -= Time.deltaTime;
        
        playerMovement.dashRechargeTime -= Time.deltaTime;
        
        if (playerMovement.dashRechargeTime < -100f)
        {
            playerMovement.dashRechargeTime = 0;
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
            playerMovement.dashRechargeTime < 0)
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

        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.LastPressedJumpTime > 0)
        {
            FSM.SetState<JumpState>();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && playerMovement.LastPressedDashTime > 0 && 
            playerMovement.dashRechargeTime < 0)
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