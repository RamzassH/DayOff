using UnityEngine;

public class RunState : GroundedState
{
    #region INPUT PARAMETERS

    #endregion

    public RunState(tmpMovement playerMovement) :
        base(playerMovement)
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
        playerMovement.LastPressedJumpTime -= Time.deltaTime;
        playerMovement.LastPressedDashTime -= Time.deltaTime;
        
        playerMovement.dashRechargeTime -= Time.deltaTime;
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
        playerMovement.ChangeDirection();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && 
            playerMovement.dashRechargeTime < 0)
        {
            OnDashInput();
        }
        
        #endregion
        
        
        if (playerMovement.LastPressedJumpTime > 0)
        {
            FSM.SetState<JumpState>();
        }

        if (playerMovement.LastPressedDashTime > 0)
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