using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockState : BattleState
{
    public BlockState(ChController controller): base(controller) { }

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
        if (!Input.GetKey(KeyCode.Q)) 
        {
            FSM.SetState<BattleIDLEState>();
        }
    }
}
