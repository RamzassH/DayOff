using Unity.VisualScripting;
using UnityEngine;

public class JumpState : PlayerState
{
    private float timer;

    public JumpState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.lastPressedJumpTime = 0;
        timer = 0;
        Jump();
    }

    public override void Update()
    {
        base.Update();
        if (timer > 0.15)
        {
            FSM.SetState<UpState>();
            return;
        }
        timer += Time.deltaTime;
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