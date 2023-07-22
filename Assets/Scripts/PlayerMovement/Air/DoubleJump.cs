using UnityEngine;

public class DoubleJump : State
{
    private Vector2 _moveInput;

    public DoubleJump(tmpMovement playerMovement) :
        base(playerMovement)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Jump();
        FSM.SetState<DoubleJumpUpState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void Jump()
    {
        #region Perform Jump

        float force = Data.jumpForce;
        
        if (RB.velocity.y < 0)
        {
            force -= RB.velocity.y;
        }

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        #endregion
    }
}