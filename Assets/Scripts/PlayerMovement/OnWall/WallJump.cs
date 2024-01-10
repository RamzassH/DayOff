using UnityEngine;

public class WallJumpState : OnWall
{
    private float timer;

    public WallJumpState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        WallJump();
        CameraShake.Instance.DoShakeCamera(3,0.1f);
        timer = 0;
        //FSM.SetState<DoubleJumpUpState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (timer > 0.15)
        {
            FSM.SetState<DoubleJumpUpState>();
            return;
        }
        timer += Time.deltaTime;
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

        RB.velocity = new Vector2(0, RB.velocity.y);

        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= tmp.x;
        
        Debug.Log(force);
        RB.AddForce(force, ForceMode2D.Impulse);

        #endregion
    }
}