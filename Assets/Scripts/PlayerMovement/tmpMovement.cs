using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class tmpMovement : MonoBehaviour
{
    public MovementData Data;
    private StateMachine _FSM;
    private Rigidbody2D _RB;

    public Transform groundCheck;
    public Transform rightWallCheck;
    public Transform leftWallCheck;

    public TextMeshProUGUI text;
    public TextMeshProUGUI infoCombo;
    
    private GameObject obj;

    [SerializeField] private List<Combo> comboList;

    private List<Combo> _currentComboList;
    private int _indexAttackInCombo;

    public void Awake()
    {
        _RB = GetComponent<Rigidbody2D>();
        _FSM = new StateMachine();

        #region GRAVITY

        _RB.gravityScale = Data.gravityScale;

        #endregion

        #region GROUND

        _FSM.AddState(new IDLE(this));
        _FSM.AddState(new RunState(this));
        _FSM.AddState(new JumpState(this));
        _FSM.AddState(new DashState(this));
        
        #endregion
        
        #region AIR

        _FSM.AddState(new UpState(this));
        _FSM.AddState(new DoubleJump(this));
        _FSM.AddState(new DoubleJumpUpState(this));
        _FSM.AddState(new FallingState(this));
        _FSM.AddState(new JumpCutState(this));
        
        #endregion

        #region ON WALL

        _FSM.AddState(new TouchWall(this));
        _FSM.AddState(new OnWall(this));
        _FSM.AddState(new GrapState(this));
        _FSM.AddState(new SlideState(this));
        _FSM.AddState(new WallJumpState(this));

        #endregion

        #region BATTLE

        _FSM.AddState(new BattleIDLEState(this));
        _FSM.AddState(new LightAttackState(this));
        _FSM.AddState(new HeavyAttackState(this));
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
        _FSM.FixedUpdate();
    }


    public StateMachine FSM
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
                if (combo != null && combo.GetActionInIndex(0) == startAction) 
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

    public void SetNullCombo() 
    {
        _currentComboList.Clear();
        _indexAttackInCombo = 0;
    }

    public void IncreaseIndexCombo() 
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
