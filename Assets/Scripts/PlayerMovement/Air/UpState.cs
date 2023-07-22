using UnityEngine;

public class UpState : AirState
{
    private Transform _groundCheck;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    private Vector2 _moveInput;

    public UpState(tmpMovement playerMovement) :
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

        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (IsTouchWall(playerMovement.rightWallCheck.position, playerMovement.leftWallCheck.position, wallCheckSize, groundLayer))
        {
            FSM.SetState<TouchWall>();
        }

        if (IsFalling(_groundCheck.position, _groundCheckSize, _groundLayer))
        {
            FSM.SetState<FallingState>();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<DoubleJump>();
        }

        if (_moveInput.y < 0)
        {
            FSM.SetState<JumpCutState>();
        }
    }
}