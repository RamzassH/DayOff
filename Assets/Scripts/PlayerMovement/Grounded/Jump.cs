using UnityEngine;

public class JumpState : GroundedState
{
    public JumpState(tmpMovement playerMovement) :
        base(playerMovement)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Jump();
        FSM.SetState<UpState>();
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