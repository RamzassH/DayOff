using Unity.VisualScripting;
using UnityEngine;

public abstract class State
{
    protected StateMachine FSM;
    protected Rigidbody2D RB;
    protected MovementData Data;

    protected Transform groundCheck;
    protected Transform rightWallCheck;
    protected Transform leftWallCheck;

    protected Vector2 groundCheckSize;
    protected Vector2 wallCheckSize;

    protected LayerMask groundLayer;
    
    protected bool IsFacingRight;
    
    public State(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck)
    {
        this.FSM = FSM;
        this.RB = RB;
        this.Data = Data;
        this.groundCheck = groundCheck;
        this.rightWallCheck = rightWallCheck;
        this.leftWallCheck = leftWallCheck;

        groundCheckSize = new Vector2(0.5f, 0.03f);
        wallCheckSize = new Vector2(0.03f, 1f);

        groundLayer = 8;
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
    
    protected bool IsFalling(Vector2 checkPosition, Vector2 checkSize, LayerMask groundLayerParam)
    {
        return RB.velocity.y <= 0 && !Physics2D.OverlapBox(checkPosition, checkSize, 0, groundLayerParam);
    }

    
    protected bool IsTouchWall(Vector2 checkRightWallPosition, Vector2 checkLeftWallPosition, Vector2 checkSize, LayerMask groundLayerParam)
    {
        return Physics2D.OverlapBox(checkRightWallPosition, checkSize, 0, groundLayerParam) ||
               Physics2D.OverlapBox(checkLeftWallPosition, checkSize, 0, groundLayerParam);
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