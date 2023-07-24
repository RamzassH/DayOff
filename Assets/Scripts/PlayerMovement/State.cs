using Unity.VisualScripting;
using UnityEngine;

public abstract class State
{
    protected StateMachine FSM;
    protected Rigidbody2D RB;
    protected MovementData Data;

    protected tmpMovement playerMovement;

    protected Vector2 groundCheckSize;
    protected Vector2 wallCheckSize;

    protected LayerMask groundLayer;
    
    protected Vector2 _moveInput;

    protected bool IsFacingRight;
    
    public State(tmpMovement playerMovement)
    {
        FSM = playerMovement.FSM;
        RB = playerMovement.RB;
        Data = playerMovement.data;
        this.playerMovement = playerMovement;

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
    
    protected bool IsFalling()
    {
        return RB.velocity.y <= 0 && !Physics2D.OverlapBox(playerMovement.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    protected bool IsCanClimb()
    {
        float distance = 0.5f;
        return !Physics2D.Raycast(playerMovement.headRayCastPos.position, playerMovement.transform.forward,
            distance, groundLayer);
    }

    protected bool IsGrounded()
    {
        return Physics2D.OverlapBox(playerMovement.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    protected bool IsInAir()
    {
        return !Physics2D.OverlapBox(playerMovement.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    
    protected bool IsTouchWall()
    {
        return Physics2D.OverlapBox(playerMovement.rightWallCheck.position, wallCheckSize,
                   0, groundLayer) ||
               Physics2D.OverlapBox(playerMovement.leftWallCheck.position, wallCheckSize,
                   0, groundLayer);
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
    
    public void OnJumpInput()
    {
        playerMovement.LastPressedJumpTime = playerMovement.data.jumpInputBufferTime;
    }

    public void OnDashInput()
    {
        playerMovement.LastPressedDashTime = playerMovement.data.dashInputBufferTime;
    }

    public void RechargeCoyoteTime()
    {
        playerMovement.coyoteTime = playerMovement.data.coyoteTime;
    }
}