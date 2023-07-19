using UnityEngine;

public class DoubleJump : State
{
    private Vector2 _moveInput;

    public DoubleJump(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, leftWallCheck, leftWallCheck)
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