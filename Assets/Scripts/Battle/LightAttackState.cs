using UnityEngine;

public class LightAttackState : BattleState
{

    // Animator
    private float _endAttackTime;
    private float _nextAttackInputBuffer;
    private float _timer;

    public LightAttackState(tmpMovement tmp): base(tmp) { }

    public override void Enter()
    {
        _endAttackTime = 0.5f;
        _nextAttackInputBuffer = 0.3f;
        _timer = 0f;
        
        // _isInput = false;
        // _isInputLightAttack = false;
        // _isInputHeavyAttack = false;
        base.Enter();
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
            else 
            {
                playerMovement.SetCurrentCombo(ComboEvents.HeavyAttack);
            }
            FSM.SetState<HeavyAttackState>();
            return;
        }
        
        if (_timer >= _endAttackTime)
        {
            playerMovement.SetNullCombo();
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

