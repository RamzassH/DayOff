using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashState : GroundedState
{
    //private bool _isDashing;

    public DashState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();

        controller.lastPressedDashTime = 0;
        RB.gravityScale = 0;
        Vector2 direction = new Vector2(playerTransform.localScale.x, 0);

        CameraShake.Instance.DoShakeCamera(2f, 0.2f);
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

        if (_moveInput.x != 0 && !IsDashing)
        {
            FSM.SetState<RunState>();
        }

        if (_moveInput.x == 0 && !IsDashing)
        {
            FSM.SetState<IDLE>();
        }

        // if (IsInAir() &&
        //     !IsDashing)
        // {
        //     RechargeCoyoteTime();
        //     FSM.SetState<FallingState>();
        // }
    }

    public IEnumerator StartDash(Vector2 dir)
    {
        IsDashing = true;
        float startTime = Time.time;

        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
            yield return null;
        }

        startTime = Time.time;
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        IsDashing = false;
    }
}