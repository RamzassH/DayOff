using UnityEngine;

public class OnWall : State
{
    public OnWall(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
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
}
