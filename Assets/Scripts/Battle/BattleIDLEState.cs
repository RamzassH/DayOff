using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
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
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            controller.Animator.SetInteger("ChangeStance", 1);
            Debug.Log("1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            controller.Animator.SetInteger("ChangeStance", 2);
            Debug.Log("2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            controller.Animator.SetInteger("ChangeStance", 3);
            Debug.Log("3");
        }
    

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
