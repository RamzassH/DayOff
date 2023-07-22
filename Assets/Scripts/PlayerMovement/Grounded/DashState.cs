using System.Collections;
using UnityEngine;

public class DashState : GroundedState
{
    private bool _isDashing;

    public DashState(tmpMovement playerMovement) :
        base(playerMovement)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 direction = new Vector2(playerTransform.localScale.x, 0);
        playerMovement.StartCoroutine(StartDash(direction));
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (IsTouchWall())
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

        if (IsInAir() &&
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