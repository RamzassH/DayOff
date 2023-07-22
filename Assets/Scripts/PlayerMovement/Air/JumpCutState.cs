using Unity.VisualScripting;
using UnityEngine;

public class JumpCutState : State
{
    private Transform _groundCheck;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    public JumpCutState(tmpMovement playerMovement) :
        base(playerMovement)
    {
    }

    public override void Enter()
    {
        base.Enter();
        JumpCut();
        FSM.SetState<FallingState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void JumpCut()
    {
        RB.velocity = new Vector2(RB.velocity.x, -RB.velocity.y);
    }
}