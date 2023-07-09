using UnityEngine;

public class WallJumpState : State
{
    public WallJumpState(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        //WallJump();
        Fsm.SetState<UpState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }}
