using UnityEngine;

public class TouchWall : OnWall
{
    private bool _isInAir;
    private bool _isFalling;

    public TouchWall(ChController playerMovement) :
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
        _isInAir = IsInAir();
        _isFalling = IsFalling();
        
        bool isTouchingRightWall = IsTouchingRightWall();
        bool isTouchingLeftWall = IsTouchingLeftWall();

        if (controller.lastPressedJumpTime > 0 && _isInAir)
        {
            controller.lastPressedJumpTime = 0f;
            FSM.SetState<WallJumpState>();
            return;
        }
        
        if (!_isInAir &&
            controller.lastPressedJumpTime > 0)
        {
            FSM.SetState<JumpState>();
            return;
        }
        
        if (IsCanClimb() && 
            (isTouchingRightWall && _moveInput.x > 0 ||
             isTouchingLeftWall && _moveInput.x < 0))
        {
            FSM.SetState<UpOnLedge>();
            return;
        }

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
            (!isTouchingRightWall && !isTouchingLeftWall || 
            isTouchingRightWall && _moveInput.x < 0 || 
            isTouchingLeftWall && _moveInput.x >0))
        {
            FSM.SetState<FallingState>();
            return;
        }

        if (!_isInAir && !_isFalling &&
            !isTouchingRightWall &&
            !isTouchingLeftWall)
        {
            FSM.SetState<IDLE>();
            return;
        }
        
        if (_isInAir && !_isFalling &&
            !isTouchingRightWall &&
            !isTouchingLeftWall)
        {
            FSM.SetState<UpState>();
            return;
        }

        if (controller.lastPressedJumpTime > 0)
        {
            FSM.SetState<DashState>();
            return;
        }

        if (_moveInput.x != 0 && !IsInAir())
        {
            FSM.SetState<RunState>();
            return;
        }
    }
}