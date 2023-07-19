using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AirState
{
    private Transform _groundCheck;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    private Vector2 _moveInput;

    public FallingState(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck)
    {
        _groundCheck = GameObject.FindWithTag("checkGround").GetComponent<Transform>();
        _groundCheckSize = new Vector2(0.49f, 0.03f);
        _groundLayer = 8;
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
        if (!IsFalling(_groundCheck.position, _groundCheckSize, _groundLayer))
        {
            FSM.SetState<IDLE>();
        }

        base.Update();
    }

    public override void FixedUpdate()
    {
        if (IsTouchWall(rightWallCheck.position, leftWallCheck.position, wallCheckSize, groundLayer))
        {
            FSM.SetState<TouchWall>();
        }

        if (RB.velocity.y < -Data.maxFallSpeed)
        {
            RB.velocity = new Vector2(RB.velocity.x, -Data.maxFallSpeed);
        }

        base.FixedUpdate();
    }
}