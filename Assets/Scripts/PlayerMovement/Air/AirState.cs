using UnityEngine;

public class AirState : State
{
    public AirState(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
    {
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
