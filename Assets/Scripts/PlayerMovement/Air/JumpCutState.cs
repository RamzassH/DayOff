using Unity.VisualScripting;
using UnityEngine;

public class JumpCutState : State
{
    private Transform _groundCheck;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    public JumpCutState(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, leftWallCheck, leftWallCheck)
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