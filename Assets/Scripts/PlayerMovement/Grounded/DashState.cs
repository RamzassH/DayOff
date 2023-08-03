using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashState : GroundedState
{
    private bool _isDashing;

    public DashState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.LastPressedDashTime = 0;
        RB.gravityScale = 0;
        Vector2 direction = new Vector2(playerTransform.localScale.x, 0);
        CameraShake.Instance.DoShakeCamera(2f,0.2f);
        controller.StartCoroutine(StartDash(direction));
    }

    public override void Exit()
    {
        base.Exit();
        controller.dashRechargeTime = controller.data.dashRefillTime;
        RB.gravityScale = Data.gravityScale;
    }

    public override void Update()
    {
        base.Update();
        
        #region INPUT

        _moveInput.x = Input.GetAxisRaw("Horizontal");

        #endregion
        
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
            RechargeCoyoteTime();
            FSM.SetState<FallingState>();
        }
    }

    public IEnumerator StartDash(Vector2 dir)
    {
        _isDashing = true;
        float startTime = Time.time;
        //RB.gravityScale = 0;

        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
            yield return null;
        }

        startTime = Time.time;
        //RB.gravityScale = Data.gravityScale;
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        _isDashing = false;
    }
}