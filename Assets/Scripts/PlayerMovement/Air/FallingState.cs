using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AirState
{
    private Transform _groundCheck;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    private Vector2 _moveInput;
    
    public FallingState(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
    {
        _groundCheck = GameObject.FindWithTag("checkGround").GetComponent<Transform>();
        _groundCheckSize = new Vector2(0.49f, 0.03f);
        _groundLayer = 8;
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
        if (!IsFalling(_groundCheck.position, _groundCheckSize, _groundLayer))
        {
            Fsm.SetState<IDLE>();
        }
        base.Update();
    }
}
