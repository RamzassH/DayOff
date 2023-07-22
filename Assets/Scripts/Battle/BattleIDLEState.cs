using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleIDLEState : State
{
    public BattleIDLEState(tmpMovement tmp) : base(tmp) 
    { 
    
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit() { base.Exit();}
    
    public override void Update()
    {
        base.Update();

        if (Input.GetAxis("Fire1") > 0) 
        {
            playerMovement.SetCurrentCombo(ComboEvents.LightAttack);
            FSM.SetState<LightAttackState>();
            return;
        }

        if (Input.GetAxis("Fire2") > 0) 
        {
            playerMovement.SetCurrentCombo(ComboEvents.HeavyAttack);
            FSM.SetState<HeavyAttackState>();
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
