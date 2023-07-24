using UnityEngine;

public class UpState : AirState
{
    public UpState(tmpMovement playerMovement) :
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

        playerMovement.coyoteTime -= Time.deltaTime;

        #endregion

        _moveInput.y = Input.GetAxisRaw("Vertical");
        
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

        if (_moveInput.y < 0)
        {
            FSM.SetState<JumpCutState>();
        }
    }
}