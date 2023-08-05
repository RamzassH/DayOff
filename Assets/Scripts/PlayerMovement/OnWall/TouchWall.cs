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

        _moveInput.x = Input.GetAxisRaw("Horizontal");

        bool isTouchingRightWall = IsTouchingRightWall();
        bool isTouchingLeftWall = IsTouchingLeftWall();

        if (IsCanClimb() && 
            (isTouchingRightWall && _moveInput.x > 0 ||
             isTouchingLeftWall && _moveInput.x < 0))
        {
            FSM.SetState<UpOnLedge>();
            return;
        }
        
        if (!_isInAir &&
            Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<JumpState>();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isInAir)
        {
            FSM.SetState<WallJumpState>();
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

        if (_moveInput.x != 0 && !IsInAir())
        {
            FSM.SetState<RunState>();
            return;
        }
    }
}