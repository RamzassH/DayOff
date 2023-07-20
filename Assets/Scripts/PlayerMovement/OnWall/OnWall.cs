using UnityEngine;

public class OnWall : State
{
    public OnWall(tmpMovement playerMovement) :
        base(playerMovement)
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
        return Physics2D.OverlapBox(playerMovement.rightWallCheck.position, wallCheckSize, 0, groundLayer);
    }
    protected bool IsTouchingLeftWall()
    {
        return Physics2D.OverlapBox(playerMovement.leftWallCheck.position, wallCheckSize, 0, groundLayer);
    }
}