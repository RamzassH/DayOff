using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tmpMovement : MonoBehaviour
{
    public MovementData data;
    private StateMachine _fsm;
    private Rigidbody2D _rb;
    
    public Transform groundCheck;
    public Transform rightWallCheck;
    public Transform leftWallCheck;
    public Transform headRayCastPos;


    public TextMeshProUGUI infoMovement;
    public TextMeshProUGUI infoCombo;

    [SerializeField] private List<Combo> comboList;

    private Combo _currentCombo;
    private int _indexAttackInCombo;
    
    #region Timers

    public float coyoteTime;

    public float lastOnWallTime;

    public float dashRechargeTime;
    
    public float LastPressedJumpTime;

    public float LastPressedDashTime;

    #endregion


    public StateMachine FSM
    {
        get { return _fsm; }
    }

    public Rigidbody2D RB
    {
        get { return _rb; }
    }

    public void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _fsm = new StateMachine();

        #region GRAVITY

        _rb.gravityScale = data.gravityScale;

        #endregion

        #region GROUND

        _fsm.AddState(new IDLE(this));
        _fsm.AddState(new RunState(this));
        _fsm.AddState(new JumpState(this));
        _fsm.AddState(new DashState(this));

        #endregion

        #region AIR

        _fsm.AddState(new UpState(this));
        _fsm.AddState(new DoubleJump(this));
        _fsm.AddState(new DoubleJumpUpState(this));
        _fsm.AddState(new FallingState(this));
        _fsm.AddState(new JumpCutState(this));

        #endregion

        #region ON WALL

        _fsm.AddState(new TouchWall(this));
        _fsm.AddState(new OnWall(this));
        _fsm.AddState(new UpOnLedge(this));
        _fsm.AddState(new GrapState(this));
        _fsm.AddState(new SlideState(this));
        _fsm.AddState(new WallJumpState(this));

        #endregion

        #region BATTLE

        _fsm.AddState(new BattleIDLEState(this));
        _fsm.AddState(new LightAttackState(this));
        _fsm.AddState(new HeavyAttackState(this));

        #endregion
    }

    void Start()
    {
        //TODO IDLE!!!
        _fsm.SetState<IDLE>();
        //_fsm.SetState<BattleIDLEState>();
        //_indexAttackInCombo = 0;
    }

    void Update()
    {
        _fsm.Update();

        infoMovement.text = _fsm.GetCurrentState().ToString();

        if (_currentCombo is null)
        {
            infoCombo.text = "None Combo";
        }
        else
        {
            infoCombo.text = _currentCombo._name + " Action number " + _indexAttackInCombo;
        }
    }

    private void FixedUpdate()
    {
        _fsm.FixedUpdate();
    }

    public void SetCurrentCombo(ComboEvents startAction)
    {
        if (startAction != ComboEvents.None)
        {
            foreach (var combo in comboList)
            {
                if (combo is not null && combo.GetActionByIndex(0) == startAction)
                {
                    _currentCombo = combo;
                    _indexAttackInCombo = 1;
                    return;
                }
            }
        }

        _currentCombo = null;
    }


    public bool IsActionEqualCurrentComboEvent(ComboEvents action)
    {
        if (_currentCombo is null)
        {
            return false;
        }

        return _currentCombo.GetActionByIndex(_indexAttackInCombo) == action;
    }

    public bool ChangeCombo(ComboEvents action)
    {
        if (_currentCombo is null)
        {
            return false;
        }

        foreach (var combo in comboList)
        {
            if (combo is not null && _indexAttackInCombo < combo._inputs.Capacity)
            {
                if (combo.GetActionByIndex(_indexAttackInCombo) == action)
                {
                    _currentCombo = combo;
                    return true;
                }
            }
        }

        return false;
    }

    public void ResetCombo()
    {
        _currentCombo = null;
        _indexAttackInCombo = 0;
    }

    public void IncreaseComboIndex()
    {
        _indexAttackInCombo++;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundCheck.position, new Vector2(0.5f, 0.03f));
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(rightWallCheck.position, new Vector2(0.03f, 1f));
        Gizmos.DrawCube(leftWallCheck.position, new Vector2(0.03f, 1f));
    }

    public void ChangeDirection()
    {
        if (this.rightWallCheck.position.x < this.leftWallCheck.position.x)
        {
            var tmp = rightWallCheck;
            rightWallCheck = leftWallCheck;
            leftWallCheck = tmp;
        }
    }
}