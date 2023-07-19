using UnityEngine;

public class DoubleJumpUpState : AirState
{
    private Transform _groundCheck;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    public DoubleJumpUpState(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck)
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
        
        if (IsTouchWall(rightWallCheck.position, leftWallCheck.position, wallCheckSize, groundLayer))
        {
            FSM.SetState<TouchWall>();
        }

        if (IsFalling(_groundCheck.position, _groundCheckSize, _groundLayer))
        {
            FSM.SetState<FallingState>();
        }
    }
}