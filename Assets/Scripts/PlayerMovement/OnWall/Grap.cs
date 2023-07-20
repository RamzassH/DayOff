using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrapState : OnWall
{
    private Vector2 _moveInput;

    private bool _isTouchRightWall;
    private bool _isTouchLeftWall;

    public GrapState(tmpMovement playerMovement) :
        base(playerMovement)
    {
    }

    public override void Enter()
    {
        base.Enter();
        RB.gravityScale = 0;
        RB.velocity = new Vector2(RB.velocity.x, 0);
        _isTouchRightWall = IsTouchingRightWall();
        _isTouchLeftWall = IsTouchingLeftWall();
    }

    public override void Exit()
    {
        base.Exit();
        RB.gravityScale = Data.gravityScale;
    }

    public override void Update()
    {
        base.Update();
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (!(_isTouchRightWall && _moveInput.x > 0 ||
            _isTouchLeftWall && _moveInput.x < 0))
        {
            FSM.SetState<TouchWall>();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<WallJumpState>();
        }
    }
    
    public override void FixedUpdate()
    {
        // Grap();
    }

    // private void Grap()
    // {
    //     RB.velocity = new Vector2(RB.velocity.x, 0);
    // }
}