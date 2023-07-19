using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GroundedState : State
{
    protected Transform playerTransform;

    public GroundedState(StateMachine FSM, Rigidbody2D RB, MovementData Data, Transform transform,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck)
    {
        playerTransform = transform;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}