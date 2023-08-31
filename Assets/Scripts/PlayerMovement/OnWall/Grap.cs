using UnityEngine;

public class GrapState : OnWall
{
    private bool _isTouchRightWall;
    private bool _isTouchLeftWall;

    public GrapState(ChController controller) :
        base(controller)
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

        if (!(_isTouchRightWall && _moveInput.x > 0 ||
              _isTouchLeftWall && _moveInput.x < 0))
        {
            FSM.SetState<TouchWall>();
        }

        if (controller.lastPressedJumpTime > 0)
        {
            FSM.SetState<WallJumpState>();
        }
    }
    
    public override void FixedUpdate()
    {
    }
}