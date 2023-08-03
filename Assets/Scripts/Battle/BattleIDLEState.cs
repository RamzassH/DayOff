using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleIDLEState : BattleState
{
    private ComboEvents _startAction;

    public BattleIDLEState(ChController controller) : base(controller) 
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
            controller.SetCurrentCombo(ComboEvents.LightAttack);
            FSM.SetState<LightAttackState>();
            return;
        }

        if (Input.GetAxis("Fire2") > 0 || 
            _startAction == ComboEvents.HeavyAttack) 
        {
            controller.SetCurrentCombo(ComboEvents.HeavyAttack);
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
