using UnityEngine;

public abstract class State
{
    protected StateMachine Fsm;
    protected Rigidbody2D RB;
    protected MovementData Data;

    protected bool IsFacingRight;
    
    public State(StateMachine Fsm, Rigidbody2D RB, MovementData Data)
    {
        this.Fsm = Fsm;
        this.RB = RB;
        this.Data = Data;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void FixedUpdate()
    {
    }
    
    protected bool IsFalling(Vector2 checkPosition, Vector2 checkSize, LayerMask groundLayer)
    {
        return RB.velocity.y <= 0 && !Physics2D.OverlapBox(checkPosition, checkSize, 0, groundLayer);
    }

    protected bool IsOnWall()
    {
        return true;
    }
    
    protected void Run(float lerpAmount, Vector2 _moveInput)
    {
        // на старте мы движемся с меньшей скоростью
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);
    
        #region CALCULATE ACCELERATEION
    
        // Определяем ускорение в воздухе или на земле.
        float accelerationRate;
    
        if (this is GroundedState)
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelerationAmount :
                                                                    Data.runDeccelerationAmount;
        else
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelerationAmount * Data.accelerationInAir :
                                                                    Data.runDeccelerationAmount * Data.deccelerationInAir;
    
        #endregion
        
        // тут была хуйня снизу
        
        float speedDif = targetSpeed - RB.velocity.x;
    
        float movement = speedDif * accelerationRate;
    
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
        
        // if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        // {
        //     accelerationRate *= Data.jumpHangAccelerationMult;
        //     targetSpeed *= Data.jumpHangMaxSpeedMult;
        // }
    
        // #region Conserve Momentum
        //
        // if (Data.doConserveMomentum &&
        //     Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) &&
        //     Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) &&
        //     Mathf.Abs(targetSpeed) > 0.01f &&
        //     LastOnGroundTime < 0)
        // {
        //     accelerationRate = 0;
        // }
        //
        // #endregion
        
    }
}