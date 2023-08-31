using UnityEngine;

public class OnWall : State
{
    public OnWall(ChController controller) :
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
        controller.lastPressedJumpTime -= Time.deltaTime;
        controller.lastPressedDashTime -= Time.deltaTime;
        controller.dashRechargeTime -= Time.deltaTime;

        #endregion

        #region MOVE INPUT

        _moveInput = controller.playerInput.Player.Move.ReadValue<Vector2>();
        
        if(controller.playerInput.Player.Jump.ReadValue<float>() > 0.1)
        {
            OnJumpInput();
        }
        if (controller.playerInput.Player.Dash.ReadValue<float>() > 0.1
            && controller.dashRechargeTime < 0)
        {
            OnDashInput();
        }

        #endregion
    }
    
    protected bool IsTouchingRightWall()
    {
        return Physics2D.OverlapBox(controller.rightWallCheck.position, wallCheckSize, 0, groundLayer);
    }
    protected bool IsTouchingLeftWall()
    {
        return Physics2D.OverlapBox(controller.leftWallCheck.position, wallCheckSize, 0, groundLayer);
    }
}