using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
}