using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grap : OnWall
{
    private Vector2 _moveInput;

    public Grap(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, leftWallCheck, leftWallCheck)
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
            FSM.SetState<Slide>();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<WallJumpState>();
        }
    }
}