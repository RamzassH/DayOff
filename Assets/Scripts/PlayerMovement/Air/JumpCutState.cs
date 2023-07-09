using UnityEngine;

public class JumpCutState : State
{
    private Transform _groundCheck;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    public JumpCutState(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        JumpCut();
        Fsm.SetState<FallingState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void JumpCut()
    {
        
    }
}