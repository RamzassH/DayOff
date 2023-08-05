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