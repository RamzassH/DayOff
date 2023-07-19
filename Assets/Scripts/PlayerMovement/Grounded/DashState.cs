using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashState : GroundedState
{
    private Vector2 _moveInput;
    private Transform _checkPosition;
    private Vector2 _checkSize;

    private bool _isDashing;
    private Vector2 direction;

    public DashState(StateMachine FSM, Rigidbody2D RB, MovementData Data, Transform transform,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, transform, groundCheck, rightWallCheck, leftWallCheck)
    {
        _checkPosition = GameObject.FindWithTag("checkGround").GetComponent<Transform>();
        _checkSize = new Vector2(0.49f, 0.03f);
    }

    public override void Enter()
    {
        base.Enter();
        direction = new Vector2(playerTransform.localScale.x, 0);
        Coroutines.StartRoutine(StartDash(direction));
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (IsTouchWall(rightWallCheck.position, leftWallCheck.position, wallCheckSize, groundLayer))
        {
            FSM.SetState<TouchWall>();
        }
        
        if (_moveInput.x != 0 && !_isDashing)
        {
            FSM.SetState<RunState>();
        }

        if (_moveInput.x == 0 && !_isDashing)
        {
            FSM.SetState<IDLE>();
        }

        if (IsFalling(_checkPosition.position, _checkSize, groundLayer) &&
            !_isDashing)
        {
            FSM.SetState<FallingState>();
        }
    }

    public IEnumerator StartDash(Vector2 dir)
    {
        _isDashing = true;
        float startTime = Time.time;
        RB.gravityScale = 0;

        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
            yield return null;
        }

        startTime = Time.time;
        RB.gravityScale = Data.gravityScale;
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        _isDashing = false;
    }
}