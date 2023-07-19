using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchWall : OnWall
{
    private float _timer;

    private Vector2 _moveInput;
    private bool _isFalling;

    public TouchWall(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, leftWallCheck, leftWallCheck)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _timer = Data.slideInputBufferTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        _isFalling = IsFalling(groundCheck.position, groundCheckSize, groundLayer);
        
        if (_isFalling) 
        {
            _timer -= Time.deltaTime;
        }
        
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space) && _isFalling)
        {
            FSM.SetState<WallJumpState>();
        }
        
        if (_timer > 0 && _isFalling && 
            (IsTouchingRightWall() && _moveInput.x > 0 ||
             !IsTouchingRightWall() && _moveInput.x < 0))
        {
            FSM.SetState<Slide>();
        }
        
        if (!_isFalling &&
            (IsTouchingRightWall() && _moveInput.x > 0 ||
            !IsTouchingRightWall() && _moveInput.x < 0))
        {
            FSM.SetState<Grap>();
        }
        
        if (_timer <= 0)
        {
            FSM.SetState<FallingState>();
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            FSM.SetState<DashState>();
        }
        
        if (_moveInput.x != 0 && !_isFalling)
        {
            FSM.SetState<RunState>();
        }
    }

    bool IsTouchingRightWall()
    {
        return Physics2D.OverlapBox(rightWallCheck.position, wallCheckSize, 0, groundLayer);
    }
}