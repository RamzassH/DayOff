using UnityEngine;

public class SlideState : OnWall
{
    private bool _isTouchRightWall;
    private bool _isTouchLeftWall;
    public SlideState(tmpMovement playerMovement) :
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

        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (_moveInput.x == 0)
        {
            FSM.SetState<TouchWall>();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<WallJumpState>();
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


    private void Slide()
    {
        float speedDif = Data.slideSpeed - RB.velocity.y;
        float movement = speedDif * Data.slideAccel;

        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime),
            Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    } 
}