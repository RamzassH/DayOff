using Unity.VisualScripting;
using UnityEngine;

public class UpState : AirState
{
    public UpState(ChController controller) :
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
    }

    public override void Update()
    {
        base.Update();

        #region TIMERS

        controller.coyoteTime -= Time.deltaTime;

        #endregion

        _moveInput.y = Input.GetAxisRaw("Vertical");
        
        if (IsTouchWall())
        {
            FSM.SetState<TouchWall>();
        }

        if (IsFalling())
        {
            FSM.SetState<FallingState>();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<DoubleJump>();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FSM.SetState<DashState>();
        }
        if (_moveInput.y < 0)
        {
            FSM.SetState<JumpCutState>();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (RB.velocity.y < Data.jumpVelocityFallOff)
        {
            RB.velocity += Vector2.up * Physics.gravity.y * Data.fallGravityMultiplier * Time.deltaTime;
        }
    }
}