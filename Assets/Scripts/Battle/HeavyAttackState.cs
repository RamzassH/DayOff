using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : BattleState
{
    private float _duration;
    private float _startTime;
    private bool _isInput;
    private bool _isInputLightAttack;
    private bool _isInputHeavyAttack;

    public HeavyAttackState(tmpMovement tmp): base(tmp) { }

    public override void Enter()
    {
        base.Enter();
        _duration = 0.5f;    
        _startTime = 0.1f;
        _isInput = false;
        _isInputLightAttack = false;
        _isInputHeavyAttack = false;
    }

    public override void Exit() { base.Exit();}

    public override void Update()
    {
        base.Update();
        _duration -= Time.deltaTime;
        _startTime -= Time.deltaTime;
        if (_duration < 0 && !_isInput)
        {
            playerMovement.SetNullCombo();
            FSM.SetState<BattleIDLEState>();
            return;
        }
        if (_duration < 0 && _isInputLightAttack) 
        {
            if (playerMovement.IsActionEqualCurrentComboEvent(ComboEvents.LightAttack))
            {
                playerMovement.IncreaseIndexCombo();
            }
            else
            {
                playerMovement.SetCurrentCombo(ComboEvents.LightAttack);
            }
            FSM.SetState<LightAttackState>();
            return;
        }
        if (_duration < 0 && _isInputHeavyAttack) 
        {
            if (playerMovement.IsActionEqualCurrentComboEvent(ComboEvents.HeavyAttack))
            {
                playerMovement.IncreaseIndexCombo();
            }
            else
            {
                playerMovement.SetCurrentCombo(ComboEvents.HeavyAttack);
            }
            FSM.SetState<HeavyAttackState>();
            return;
        }


        if (Input.GetAxis("Fire1") > 0 && !_isInput && _startTime < 0f)
        {
            //if (playerMovement.IsActionEqualCurrentComboEvent(ComboEvents.LightAttack))
            //{
            //    playerMovement.IncreaseIndexCombo();
            //}
            //else
            //{
            //    playerMovement.SetCurrentCombo(ComboEvents.LightAttack);
            //}
            //FSM.SetState<LightAttackState>();
            _isInput = true;
            _isInputLightAttack = true;
            return;
        }

        if (Input.GetAxis("Fire2") > 0 && !_isInput && _startTime < 0f)
        {
            //if (playerMovement.IsActionEqualCurrentComboEvent(ComboEvents.HeavyAttack))
            //{
            //    playerMovement.IncreaseIndexCombo();
            //}
            //else
            //{
            //    playerMovement.SetCurrentCombo(ComboEvents.HeavyAttack);
            //}
            //FSM.SetState<HeavyAttackState>();
            _isInput = true;
            _isInputHeavyAttack = true;
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
