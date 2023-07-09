using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grap : OnWall
{
    private Vector2 _moveInput;
    
    public Grap(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
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
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (_moveInput.x == 0)
        {
            Fsm.SetState<Slide>();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fsm.SetState<WallJumpState>();
        }
    }
}
