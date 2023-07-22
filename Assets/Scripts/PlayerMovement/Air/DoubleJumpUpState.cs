using UnityEngine;

public class DoubleJumpUpState : AirState
{
    private Transform _groundCheck;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    public DoubleJumpUpState(tmpMovement playerMovement) :
        base(playerMovement)
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
        base.Update();
        
        if (IsTouchWall(playerMovement.rightWallCheck.position,playerMovement.leftWallCheck.position, wallCheckSize, groundLayer))
        {
            FSM.SetState<TouchWall>();
        }

        if (IsFalling(_groundCheck.position, _groundCheckSize, _groundLayer))
        {
            FSM.SetState<FallingState>();
        }
    }
}