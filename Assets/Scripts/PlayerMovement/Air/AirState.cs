using UnityEngine;

public class AirState : State
{
    public AirState(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck)
    {
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}