using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State
{
    public BattleState(tmpMovement tmp) : base(tmp)
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

        float moveInputX = Input.GetAxisRaw("Horizontal");
        bool isFalling = IsFalling();
        if (isFalling)
        {
            FSM.SetState<FallingState>();
            return;
        }
        if (moveInputX != 0 && this is BattleIDLEState ) 
        {
            FSM.SetState<RunState>();
            return;
        }
        if (Input.GetKey(KeyCode.Space) && this is BattleIDLEState ) 
        {
            FSM.SetState<JumpState>();
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift) && this is BattleIDLEState) 
        {
            FSM.SetState <DashState>();
            return;
        }

    }
}

