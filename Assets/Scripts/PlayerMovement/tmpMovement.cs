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

    private List<Combo> _currentComboList;
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
        _FSM.AddState(new BlockState(this));
        #endregion
    }

    void Start()
    {
        // Вернуть IDLE!!!
        _FSM.SetState<IDLE>();
        //_FSM.SetState<BattleIDLEState>();
        _currentComboList = new List<Combo>();
        _indexAttackInCombo = 0;
    }

    void Update()
    {
        _FSM.Update();
        text.text = _FSM.GetCurrentState().ToString();
        if (_currentComboList.Count == 0)
        {
            infoCombo.text = "None Combo";
        }
        else
        {
            infoCombo.text = "Count combo: " + _currentComboList.Count.ToString() + "\n" + 
                             "Index Action: " + _indexAttackInCombo.ToString();
        }
    }

    private void FixedUpdate()
    {
        _fsm.FixedUpdate();
    }

    public void SetCurrentCombo(ComboEvents startAction)
    {
        get { return _FSM; }
    }
    
    public Rigidbody2D RB
    {
        get { return _RB; }
    }

    public void SetCurrentCombo(ComboEvents startAction) {
        SetNullCombo();
        if (startAction != ComboEvents.None) 
        {
            foreach (var combo in comboList)
            {
                if (combo is not null && combo.GetActionByIndex(0) == startAction)
                {
                    _currentComboList.Add(combo);
                    _indexAttackInCombo = 1;
                }
            }
        }
    }


    public bool IsActionEqualCurrentComboEvent(ComboEvents action)
    {
        if (_currentComboList.Count == 0) 
        { 
            return false;
        }
        List<Combo> listCombo = new List<Combo>();
        for (var i = 0; i < _currentComboList.Count; i++) 
        {
            if (_currentComboList[i].GetActionInIndex(_indexAttackInCombo) == action) 
            {
                listCombo.Add(_currentComboList[i]);
            }
        }
        _currentComboList.Clear();

        foreach (var combo in listCombo) 
        {
            _currentComboList.Add(combo);
        }
        return _currentComboList.Count != 0;
    }

    public bool ChangeCombo(ComboEvents action)
    {
        _currentComboList.Clear();
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