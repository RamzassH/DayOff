using UnityEngine;

public class WallJumpState : State
{
    public WallJumpState(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, leftWallCheck, leftWallCheck)
    {
    }

    public override void Enter()
    {
        base.Enter();
        FSM.SetState<UpState>();
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