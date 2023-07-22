using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchWall : OnWall
{
    private Vector2 _moveInput;
    private bool _isFalling;

    public TouchWall(tmpMovement playerMovement) :
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
        _isFalling = IsFalling(playerMovement.groundCheck.position, groundCheckSize, groundLayer);
        
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (!_isFalling &&
            Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<JumpState>();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && _isFalling)
        {
            FSM.SetState<WallJumpState>();
            return;
        }

        bool isTouchingRightWall = IsTouchingRightWall();
        bool isTouchingLeftWall = IsTouchingLeftWall();
        
        if (_isFalling && 
            (isTouchingRightWall && _moveInput.x > 0 ||
             isTouchingLeftWall && _moveInput.x < 0))
        {
            FSM.SetState<SlideState>();
            return;
        }
        
        if (!_isFalling &&
            (isTouchingRightWall && _moveInput.x > 0 ||
             isTouchingLeftWall && _moveInput.x < 0))
        {
            FSM.SetState<GrapState>();
            return;
        }
        
        if (_isFalling && 
            !isTouchingRightWall && 
            !isTouchingLeftWall)
        {
            FSM.SetState<FallingState>();
            return;
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            FSM.SetState<DashState>();
            return;
        }
        
        if (_moveInput.x != 0 && !_isFalling)
        {
            FSM.SetState<RunState>();
            return;
        }
    }
    
}