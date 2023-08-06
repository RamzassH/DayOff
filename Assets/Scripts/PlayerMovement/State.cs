using Unity.VisualScripting;
using UnityEngine;

public abstract class State
{
    protected StateMachine FSM;
    protected Rigidbody2D RB;
    protected MovementData Data;

    protected ChController controller;

    protected Vector2 groundCheckSize;
    protected Vector2 wallCheckSize;

    protected LayerMask groundLayer;
    
    protected Vector2 _moveInput;

    protected bool IsFacingRight;
    
    public State(ChController controller)
    {
        FSM = controller.FSM;
        RB = controller.RB;
        Data = controller.data;
        this.controller = controller;

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

    public virtual void LateUpdate()
    {
        
    }
    protected bool IsFalling()
    {
        return RB.velocity.y <= 0 && !Physics2D.OverlapBox(controller.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    protected bool IsCanClimb()
    {
        float distance = 1f;
        return !Physics2D.Raycast(controller.headRayCastPos.position, controller.transform.localScale,
            distance, groundLayer);
    }

    protected bool IsGrounded()
    {
        return Physics2D.OverlapBox(controller.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    protected bool IsInAir()
    {
        return !Physics2D.OverlapBox(controller.groundCheck.position, groundCheckSize,
            0, groundLayer);
    }

    
    protected bool IsTouchWall()
    {
        return Physics2D.OverlapBox(controller.rightWallCheck.position, wallCheckSize,
                   0, groundLayer) ||
               Physics2D.OverlapBox(controller.leftWallCheck.position, wallCheckSize,
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

        float speedDif = targetSpeed - RB.velocity.x;
    
        float movement = speedDif * accelerationRate;
    
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    
    public void OnJumpInput()
    {
        controller.LastPressedJumpTime = controller.data.jumpInputBufferTime;
    }

    public void OnDashInput()
    {
        if (controller.dashRechargeTime <= 0)
        {
            controller.LastPressedDashTime = controller.data.dashInputBufferTime;
        } 
    }

    public void RechargeCoyoteTime()
    {
        controller.coyoteTime = controller.data.coyoteTime;
    }
}