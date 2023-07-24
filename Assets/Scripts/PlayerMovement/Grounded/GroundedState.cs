using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class GroundedState : State
{
    protected Transform playerTransform;
    
    public GroundedState(tmpMovement playerMovement) :
        base(playerMovement)
    {
        playerTransform = playerMovement.transform;
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
        bool isFalling = IsFalling();
        if (Input.GetAxis("Fire1") > 0f && !isFalling && 
            (this is IDLE || this is RunState)) 
        {
            FSM.SetState<BattleIDLEState>();
            (FSM.GetCurrentState() as BattleIDLEState).SetStartAction(ComboEvents.LightAttack);
            return;
        }
        if (Input.GetAxis("Fire2") > 0f && !isFalling &&
            (this is IDLE || this is RunState)) 
        {
            FSM.SetState<BattleIDLEState>();
            (FSM.GetCurrentState() as BattleIDLEState).SetStartAction(ComboEvents.HeavyAttack);
            return;
        }
        if (Input.GetKey(KeyCode.Q) && !isFalling &&
            (this is IDLE || this is RunState)) 
        {
            FSM.SetState<BattleIDLEState>();
            (FSM.GetCurrentState() as BattleIDLEState).SetStartAction(ComboEvents.Block);
            return;
        }
    }
}