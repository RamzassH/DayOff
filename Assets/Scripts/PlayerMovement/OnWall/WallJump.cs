using UnityEngine;

public class WallJumpState : OnWall
{
    public WallJumpState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        WallJump();
        CameraShake.Instance.DoShakeCamera(3,0.1f);
        FSM.SetState<DoubleJumpUpState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
    
    private void WallJump()
    {
        #region Perform Wall Jump
        
        Vector2 tmp = new Vector2(controller.playerBody.localScale.x, 0);
        
        if (tmp.x > 0 && IsTouchingRightWall() ||
            tmp.x < 0 && IsTouchingLeftWall())
        {
            Vector3 scale = controller.playerBody.localScale;
            scale.x *= -1;
            tmp.x *= -1;
            controller.playerBody.localScale = scale;

        }
        
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= tmp.x;


        //if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
        //{
        //    force.x -= RB.velocity.x;
        //}
        //
        //if (RB.velocity.y < 0)
        //{
        //    force.y -= RB.velocity.y;
        //}
            
        Debug.Log(force);
        RB.AddForce(force, ForceMode2D.Impulse);

        #endregion
    }
}