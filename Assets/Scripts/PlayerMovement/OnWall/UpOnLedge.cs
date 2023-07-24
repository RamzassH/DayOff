using UnityEngine;

public class UpOnLedge : OnWall
{

    private Vector2 _moveInput;
    public UpOnLedge(tmpMovement playerMovement) : base(playerMovement)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        RB.gravityScale = 0;
        RB.velocity = new Vector2(RB.velocity.x, 0);
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
        bool isTouchingRightWall = IsTouchingRightWall();
        bool isTouchingLeftWall = IsTouchingLeftWall();

        if (!(isTouchingRightWall && _moveInput.x > 0 ||
            !isTouchingLeftWall && _moveInput.x < 0))
        {
            FSM.SetState<TouchWall>();
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            //TODO Сделать подъем
        }

        if (Input.GetAxisRaw("Jump") > 0)
        {
            FSM.SetState<WallJumpState>();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}