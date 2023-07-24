using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : BattleState
{
    private float _endAttackTime;
    private float _nextAttackInputBuffer;
    private float _timer;

    public HeavyAttackState(tmpMovement tmp): base(tmp) { }

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
            if (playerMovement.IsActionEqualCurrentComboEvent(ComboEvents.LightAttack))
            {
                playerMovement.IncreaseComboIndex();
            }
            else if (playerMovement.ChangeCombo(ComboEvents.LightAttack))
            {
                playerMovement.SetCurrentCombo(ComboEvents.LightAttack);
            }
            else
            {
                playerMovement.SetCurrentCombo(ComboEvents.LightAttack);
            }
            FSM.SetState<LightAttackState>();
            return;
        }

        if (Input.GetAxis("Fire2") > 0 && _timer >= _nextAttackInputBuffer)
        {
            if (playerMovement.IsActionEqualCurrentComboEvent(ComboEvents.HeavyAttack))
            {
                playerMovement.IncreaseComboIndex();
            }
            else if (playerMovement.ChangeCombo(ComboEvents.HeavyAttack))
            {
                playerMovement.SetCurrentCombo(ComboEvents.HeavyAttack);
            }
            else
            {
                playerMovement.SetCurrentCombo(ComboEvents.HeavyAttack);
            }
            FSM.SetState<HeavyAttackState>();
            return;
        }
        
        if (_timer >= _endAttackTime)
        {
            playerMovement.ResetCombo();
            FSM.SetState<BattleIDLEState>();
            return;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            playerMovement.SetNullCombo();
            FSM.SetState<BlockState>();
            return;
        }
    }
}
