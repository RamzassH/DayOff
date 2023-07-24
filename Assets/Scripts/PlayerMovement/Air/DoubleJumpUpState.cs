using UnityEngine;

public class DoubleJumpUpState : AirState
{
    public DoubleJumpUpState(tmpMovement playerMovement) :
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