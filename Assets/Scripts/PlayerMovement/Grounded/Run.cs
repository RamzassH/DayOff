using UnityEngine;

public class RunState : GroundedState
{
    #region INPUT PARAMETERS

    #endregion

    public RunState(ChController controller) :
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
        if (controller.lastPressedJumpTime > 0)
        {
            FSM.SetState<JumpState>();
        }

        if (controller.lastPressedDashTime > 0 && 
            controller.dashRechargeTime < 0)
        {
            FSM.SetState<DashState>();
        }
        
        base.Update();

        if (RB.velocity.x == 0 && RB.velocity.y == 0 && _moveInput.x == 0)
        {
            FSM.SetState<IDLE>();
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
    
    protected void Run(float lerpAmount, Vector2 _moveInput)
    {
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);
    
        #region CALCULATE ACCELERATEION
    
        // Определяем ускорение в воздухе или на земле.
        float accelerationRate;
    
        if (FSM.GetCurrentState() is GroundedState)
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelerationAmount :
                Data.runDeccelerationAmount;
        else
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelerationAmount * Data.accelerationInAir :
                Data.runDeccelerationAmount * Data.deccelerationInAir;
    
        #endregion

        float speedDif = targetSpeed - RB.velocity.x;
    
        float movement = speedDif * accelerationRate;
    
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}