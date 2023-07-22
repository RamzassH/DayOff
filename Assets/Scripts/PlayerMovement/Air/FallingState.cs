using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AirState
{
    public FallingState(tmpMovement playerMovement) :
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
        if (!IsFalling())
        {
            FSM.SetState<IDLE>();
        }

        if (IsTouchWall())
        {
            FSM.SetState<TouchWall>();
        }
        
        base.Update();
    }

    public override void FixedUpdate()
    {
        if (RB.velocity.y < -Data.maxFallSpeed)
        {
            RB.velocity = new Vector2(RB.velocity.x, -Data.maxFallSpeed);
        }

        base.FixedUpdate();
    }
}