using UnityEngine;

public class SlideState : OnWall
{
    private bool _isTouchRightWall;
    private bool _isTouchLeftWall;
    public SlideState(ChController playerMovement) :
        base(playerMovement)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _isTouchRightWall = IsTouchingRightWall();
        _isTouchLeftWall = IsTouchingLeftWall();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_moveInput.x == 0)
        {
            FSM.SetState<TouchWall>();
            return;
        }

        if (RB.velocity.y >= 0 &&
            (_isTouchRightWall && _moveInput.x > 0 || 
             _isTouchLeftWall && _moveInput.x < 0))
        {
            FSM.SetState<GrapState>();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        Slide();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        
        if (controller.lastPressedJumpTime > 0)
        {
            FSM.SetState<WallJumpState>();
        }
    }

    private void Slide()
    {
        float speedDif = Data.slideSpeed - RB.velocity.y;
        float movement = speedDif * Data.slideAccel;

        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime),
            Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    } 
}