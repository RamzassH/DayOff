using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : BattleState
{
    private float _endAttackTime;
    private float _nextAttackInputBuffer;
    private float _timer;

    public HeavyAttackState(ChController controller): base(controller) { }

    public override void Enter()
    {
        base.Enter();
        _endAttackTime = 0.5f;
        _nextAttackInputBuffer = 0.3f;
        _timer = 0f;
    }

    public override void Exit() { base.Exit();}

    public override void Update()
    {
        base.Update();
        _timer += Time.deltaTime;
        
        if (Input.GetAxis("Fire1") > 0 && _timer >= _nextAttackInputBuffer)
        {
            if (controller.IsActionEqualCurrentComboEvent(ComboEvents.LightAttack))
            {
                controller.IncreaseComboIndex();
            }
            else
            {
                controller.SetCurrentCombo(ComboEvents.LightAttack);
            }
            FSM.SetState<LightAttackState>();
            return;
        }

        if (Input.GetAxis("Fire2") > 0 && _timer >= _nextAttackInputBuffer)
        {
            if (controller.IsActionEqualCurrentComboEvent(ComboEvents.HeavyAttack))
            {
                controller.IncreaseComboIndex();
            }
            else
            {
                controller.SetCurrentCombo(ComboEvents.HeavyAttack);
            }
            FSM.SetState<HeavyAttackState>();
            return;
        }
        
        if (_timer >= _endAttackTime)
        {
            controller.SetNullCombo();
            FSM.SetState<BattleIDLEState>();
            return;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            controller.SetNullCombo();
            FSM.SetState<BlockState>();
            return;
        }
    }
}
