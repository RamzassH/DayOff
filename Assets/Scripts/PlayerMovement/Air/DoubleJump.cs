using UnityEngine;

public class DoubleJump : State
{
    private Vector2 _moveInput;
    public DoubleJump(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.LogError("дубль джумп)))"); 
        Jump();
        Fsm.SetState<DoubleJumpUpState>();
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
