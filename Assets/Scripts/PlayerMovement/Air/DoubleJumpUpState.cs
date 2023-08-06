using UnityEngine;

public class DoubleJumpUpState : AirState
{
    public DoubleJumpUpState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        IsDoubleJumped = true;
    }

    public override void Update()
    {
        base.Update();
        controller.LastPressedDashTime -= Time.deltaTime;
        controller.dashRechargeTime -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDashInput();
        }

        if (controller.LastPressedDashTime > 0)
        {
            FSM.SetState<DashState>();
        }

        if (IsTouchWall())
        {
            FSM.SetState<TouchWall>();
        }

        if (IsFalling())
        {
            FSM.SetState<FallingState>();
        }
    }
}