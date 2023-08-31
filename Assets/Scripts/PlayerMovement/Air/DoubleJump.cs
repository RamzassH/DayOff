using UnityEngine;

public class DoubleJump : AirState
{
    public DoubleJump(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        Jump();

        CameraShake.Instance.DoShakeCamera(5f, 0.2f);
            
        FSM.SetState<DoubleJumpUpState>();
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    private void Jump()
    {
        #region Perform Jump

        float force = Data.doubleJumpForce;
        
        if (RB.velocity.y < 0)
        {
            force -= RB.velocity.y;
        }

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        #endregion
    }

}