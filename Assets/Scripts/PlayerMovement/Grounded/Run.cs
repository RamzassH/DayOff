using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class RunState : GroundedState
{
    private MovementData _data;

    private Transform _checkPosition;
    private Vector2 _checkSize;
    private LayerMask _groundLayer;

    #region INPUT PARAMETERS

    private Vector2 _moveInput;

    #endregion

    public RunState(StateMachine FSM, Rigidbody2D RB, MovementData Data, Transform transform,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, transform, groundCheck, rightWallCheck, leftWallCheck)
    {
        _data = Data;
        _checkPosition = GameObject.FindWithTag("checkGround").GetComponent<Transform>();
        _checkSize = new Vector2(0.49f, 0.03f);
        _groundLayer = 8;
    }

    public override void Enter()
    {
        base.Enter();
        // _moveInput.x = Input.GetAxisRaw("Horizontal");
        // Run(1, _moveInput);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        #region Input

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        
        if (IsTouchWall(rightWallCheck.position, leftWallCheck.position, wallCheckSize, groundLayer))
        {
            FSM.SetState<TouchWall>();
        }
        
        if (_moveInput.x > 0)
        {
            playerTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (_moveInput.x < 0)
        {
            playerTransform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<JumpState>();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FSM.SetState<DashState>();
        }

        #endregion

        base.Update();

        if (RB.velocity.x == 0 && RB.velocity.y == 0 && _moveInput.x == 0)
        {
            FSM.SetState<IDLE>();
        }

        if (IsFalling(_checkPosition.position, _checkSize, _groundLayer))
        {
            FSM.SetState<FallingState>();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_moveInput.x != 0)
        {
            Run(1, _moveInput);
        }
    }
}