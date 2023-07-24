using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleIDLEState : BattleState
{
    private ComboEvents _startAction;

    public BattleIDLEState(tmpMovement tmp) : base(tmp) 
    {
        _startAction = ComboEvents.None;
    }

    public override void Enter()
    {
        base.Enter();
        _startAction = ComboEvents.None;
    }

    public override void Exit() { base.Exit();}
    
    public override void Update()
    {
        base.Update();

        if (Input.GetAxis("Fire1") > 0 || 
            _startAction == ComboEvents.LightAttack) 
        {
            playerMovement.SetCurrentCombo(ComboEvents.LightAttack);
            FSM.SetState<LightAttackState>();
            return;
        }

        if (Input.GetAxis("Fire2") > 0 || 
            _startAction == ComboEvents.HeavyAttack) 
        {
            playerMovement.SetCurrentCombo(ComboEvents.HeavyAttack);
            FSM.SetState<HeavyAttackState>();
            return;
        }

        if (Input.GetKey(KeyCode.Q) || 
            _startAction == ComboEvents.Block) 
        {
            FSM.SetState<BlockState>();
            return;
        }
        
    }

    public void SetStartAction(ComboEvents action) 
    {
        _startAction = action;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
